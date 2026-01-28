using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Vite.AspNetCore;
using WeChooz.TechAssessment.Web.Data;
using WeChooz.TechAssessment.Web.Services.Admin;
using WeChooz.TechAssessment.Web.Services.Public;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

var sqlServerConnectionString = builder.Configuration.GetConnectionString("formation") ?? throw new InvalidOperationException("Connection string 'formation' not found.");
var redisConnectionString = builder.Configuration.GetConnectionString("cache") ?? throw new InvalidOperationException("Connection string 'cache' not found.");

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});
builder.Services.AddProblemDetails();
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod
        | HttpLoggingFields.RequestPath
        | HttpLoggingFields.ResponseStatusCode
        | HttpLoggingFields.RequestHeaders;
    options.RequestHeaders.Add("User-Agent");
});
builder.Services.AddDbContext<FormationDbContext>(options =>
{
    options.UseSqlServer(sqlServerConnectionString);
});
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ITrainingSessionService, TrainingSessionService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IPublicSessionsService, PublicSessionsService>();
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Add("/{1}/_Views/{0}" + RazorViewEngine.ViewExtension);
});
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.Cookie.Name = "AspireAuthCookie";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization(options =>
{
    var defaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("Formation", policy => policy.Combine(defaultPolicy).RequireRole("formation"));
    options.AddPolicy("Sales", policy => policy.Combine(defaultPolicy).RequireRole("sales"));
});


builder.Services.AddViteServices(options =>
{
    options.Server.Port = 5180;
    options.Server.Https = true;
    options.Server.AutoRun = true;
    options.Server.UseReactRefresh = true;
    options.Manifest = "vite.manifest.json";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpLogging();
app.MapStaticAssets();

app.UseRouting();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultEndpoints();

app.MapControllers();

app.MapControllerRoute(
        name: "fallback_admin",
        pattern: "admin/{*subpath}",
        constraints: new { subpath = @"^(?!swagger).*$" },
        defaults: new { controller = "Admin", action = "Handle" }
);
app.MapControllerRoute(
        name: "fallback_admin_root",
        pattern: "admin/",
        defaults: new { controller = "Admin", action = "Handle" }
    );
app.MapControllerRoute(
        name: "fallback_home",
        pattern: "{*subpath}",
        constraints: new { subpath = @"^(?!swagger).*$" },
        defaults: new { controller = "Home", action = "Handle" }
);
app.MapControllerRoute(
        name: "fallback_home_root",
        pattern: "",
        defaults: new { controller = "Home", action = "Handle" }
    );

if (app.Environment.IsDevelopment())
{
    app.UseWebSockets();
    app.UseViteDevelopmentServer(true);
}
app.Run();
