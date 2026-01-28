export type ApiError = {
    status: number;
    message: string;
};

export type PagedResult<T> = {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
};

type CsrfResponse = { token: string };

let csrfToken: string | null = null;

async function ensureCsrfToken() {
    if (csrfToken) return csrfToken;
    const response = await fetch("/_api/auth/csrf", { credentials: "include" });
    if (!response.ok) {
        throw { status: response.status, message: await response.text() } satisfies ApiError;
    }
    const data = (await response.json()) as CsrfResponse;
    csrfToken = data.token;
    return csrfToken;
}

export async function apiFetch<T>(url: string, options: RequestInit = {}) {
    const method = (options.method ?? "GET").toUpperCase();
    const needsCsrf = method !== "GET" && method !== "HEAD" && method !== "OPTIONS";
    const csrfHeader = needsCsrf ? { "X-CSRF-TOKEN": await ensureCsrfToken() } : {};

    const response = await fetch(url, {
        ...options,
        headers: {
            "Content-Type": "application/json",
            ...csrfHeader,
            ...(options.headers || {}),
        },
        credentials: "include",
    });

    if (!response.ok) {
        const message = await response.text();
        throw { status: response.status, message } satisfies ApiError;
    }

    if (response.status === 204) {
        return null as T;
    }

    return (await response.json()) as T;
}

export async function getAuthStatus() {
    const response = await fetch("/_api/auth/status", { credentials: "include" });
    if (!response.ok) {
        return { isAuthenticated: false, roles: [] as string[] };
    }
    return (await response.json()) as { isAuthenticated: boolean; roles: string[] };
}

export async function logout() {
    await apiFetch("/_api/auth/logout", { method: "POST" });
}
