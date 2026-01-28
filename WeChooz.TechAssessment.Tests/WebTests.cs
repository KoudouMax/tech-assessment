using System.Net;
using System.Net.Http.Json;

namespace WeChooz.TechAssessment.Tests;

public class WebTests
{
    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.WeChooz_TechAssessment_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
        // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging

        await using var app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        // Act
        var httpClient = await CreateCookieClientAsync(app, resourceNotificationService);
        var response = await httpClient.GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PublicSessionsEndpointReturnsOk()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.WeChooz_TechAssessment_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        await using var app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        var httpClient = await CreateCookieClientAsync(app, resourceNotificationService);
        var response = await httpClient.GetAsync("/_api/public/sessions?page=1&pageSize=5");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AdminLoginAcceptsKnownRoles()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.WeChooz_TechAssessment_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        await using var app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        var httpClient = await CreateCookieClientAsync(app, resourceNotificationService);
        var csrf = await GetCsrfTokenAsync(httpClient);
        var response = await PostJsonWithCsrfAsync(httpClient, "/_api/admin/login", new { login = "formation" }, csrf);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AdminCoursesRequiresAuthentication()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.WeChooz_TechAssessment_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        await using var app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        var httpClient = await CreateCookieClientAsync(app, resourceNotificationService);
        var response = await httpClient.GetAsync("/_api/admin/courses");

        Assert.True(response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AdminCoursesReturnsOkAfterLogin()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.WeChooz_TechAssessment_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        await using var app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        var httpClient = await CreateCookieClientAsync(app, resourceNotificationService);

        var csrf = await GetCsrfTokenAsync(httpClient);
        var loginResponse = await PostJsonWithCsrfAsync(httpClient, "/_api/admin/login", new { login = "formation" }, csrf);
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        var response = await httpClient.GetAsync("/_api/admin/courses");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AdminLoginWithoutCsrfReturnsBadRequest()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.WeChooz_TechAssessment_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        await using var app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        var httpClient = await CreateCookieClientAsync(app, resourceNotificationService);
        var response = await httpClient.PostAsJsonAsync("/_api/admin/login", new { login = "formation" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private static async Task<HttpClient> CreateCookieClientAsync(DistributedApplication app, ResourceNotificationService resourceNotificationService)
    {
        await resourceNotificationService.WaitForResourceAsync("webfrontend", KnownResourceStates.Running)
            .WaitAsync(TimeSpan.FromSeconds(30));

        var baseClient = app.CreateHttpClient("webfrontend");
        var handler = new HttpClientHandler
        {
            CookieContainer = new CookieContainer()
        };
        return new HttpClient(handler)
        {
            BaseAddress = baseClient.BaseAddress
        };
    }

    private static async Task<string> GetCsrfTokenAsync(HttpClient httpClient)
    {
        var response = await httpClient.GetFromJsonAsync<CsrfTokenResponse>("/_api/auth/csrf");
        return response?.Token ?? string.Empty;
    }

    private sealed record CsrfTokenResponse(string Token);

    private static async Task<HttpResponseMessage> PostJsonWithCsrfAsync<T>(HttpClient httpClient, string url, T payload, string csrfToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(payload)
        };
        request.Headers.TryAddWithoutValidation("X-CSRF-TOKEN", csrfToken);
        return await httpClient.SendAsync(request);
    }
}
