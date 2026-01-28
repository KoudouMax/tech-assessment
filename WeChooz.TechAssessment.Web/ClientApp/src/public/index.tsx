import "@mantine/core/styles.css";
import "@mantine/notifications/styles.css";
import { Button, Container, Group, Loader, MantineProvider, Pagination, Stack, Text, Title } from "@mantine/core";
import React, { useMemo, useState } from "react";
import ReactDOM, { Container as ReactContainer } from "react-dom/client";
import { BrowserRouter, Route, Routes, useNavigate, useParams } from "react-router-dom";
import { QueryClientProvider, useQuery } from "@tanstack/react-query";
import { getAuthStatus, logout, PagedResult } from "../shared/api";
import { queryClient } from "../shared/queryClient";
import { AppNotifications } from "../shared/ui";
import { notifyError, notifySuccess } from "../shared/toast";
import { FilterPanel } from "./components/FilterPanel";
import { PublicSessionDetail, SessionDetail } from "./components/SessionDetail";
import { PublicSessionSummary, SessionList } from "./components/SessionList";

function PublicHeader({ isAuthenticated, onLogout }: { isAuthenticated: boolean; onLogout: () => Promise<void> }) {
    return (
        <Group justify="space-between" align="center">
            <Title order={1}>Catalogue des formations</Title>
            <Group>
                {isAuthenticated ? (
                    <>
                        <Button component="a" href="/admin" variant="light">
                            Administration
                        </Button>
                        <Button
                            variant="outline"
                            onClick={async () => {
                                await onLogout();
                            }}
                        >
                            Se déconnecter
                        </Button>
                    </>
                ) : (
                    <Button component="a" href="/Account/Login?ReturnUrl=%2Fadmin" variant="light">
                        Se connecter
                    </Button>
                )}
            </Group>
        </Group>
    );
}

function PublicListPage(props: {
    targetAudience: string | null;
    deliveryMode: string | null;
    startDateFrom: string;
    startDateTo: string;
    audienceOptions: { value: string; label: string }[];
    deliveryModeOptions: { value: string; label: string }[];
    onTargetAudienceChange: (value: string | null) => void;
    onDeliveryModeChange: (value: string | null) => void;
    onStartDateFromChange: (value: string) => void;
    onStartDateToChange: (value: string) => void;
    sessions: PublicSessionSummary[];
    loading: boolean;
}) {
    const navigate = useNavigate();
    return (
        <>
            <FilterPanel
                targetAudience={props.targetAudience}
                deliveryMode={props.deliveryMode}
                startDateFrom={props.startDateFrom}
                startDateTo={props.startDateTo}
                audienceOptions={props.audienceOptions}
                deliveryModeOptions={props.deliveryModeOptions}
                onTargetAudienceChange={props.onTargetAudienceChange}
                onDeliveryModeChange={props.onDeliveryModeChange}
                onStartDateFromChange={props.onStartDateFromChange}
                onStartDateToChange={props.onStartDateToChange}
            />
            {props.loading ? (
                <Group justify="center" py="xl">
                    <Loader />
                </Group>
            ) : (
                <SessionList sessions={props.sessions} onOpen={(id) => navigate(`/session/${id}`)} />
            )}
        </>
    );
}

function PublicDetailPage() {
    const { id } = useParams();
    const navigate = useNavigate();

    const { data, isLoading } = useQuery({
        queryKey: ["public-session", id],
        enabled: Boolean(id),
        queryFn: async () => {
            const response = await fetch(`/_api/public/sessions/${id}`);
            if (!response.ok) return null;
            return (await response.json()) as PublicSessionDetail;
        },
    });

    if (isLoading) {
        return (
            <Group justify="center" py="xl">
                <Loader />
            </Group>
        );
    }

    return <SessionDetail session={data ?? null} onBack={() => navigate("/")} />;
}

function App() {
    const [targetAudience, setTargetAudience] = useState<string | null>(null);
    const [deliveryMode, setDeliveryMode] = useState<string | null>(null);
    const [startDateFrom, setStartDateFrom] = useState("");
    const [startDateTo, setStartDateTo] = useState("");
    const [page, setPage] = useState(1);
    const pageSize = 10;

    const queryString = useMemo(() => {
        const params = new URLSearchParams();
        params.set("page", page.toString());
        params.set("pageSize", pageSize.toString());
        if (targetAudience) params.set("targetAudience", targetAudience);
        if (deliveryMode) params.set("deliveryMode", deliveryMode);
        if (startDateFrom) params.set("startDateFrom", startDateFrom);
        if (startDateTo) params.set("startDateTo", startDateTo);
        const value = params.toString();
        return value ? `?${value}` : "";
    }, [targetAudience, deliveryMode, startDateFrom, startDateTo, page]);

    const { data: authStatus } = useQuery({
        queryKey: ["auth-status"],
        queryFn: getAuthStatus,
    });

    const { data: audiences = [] } = useQuery({
        queryKey: ["metadata-audiences"],
        queryFn: async () => {
            const response = await fetch("/_api/metadata/audiences");
            if (!response.ok) return [] as string[];
            return (await response.json()) as string[];
        },
    });

    const { data: deliveryModes = [] } = useQuery({
        queryKey: ["metadata-delivery-modes"],
        queryFn: async () => {
            const response = await fetch("/_api/metadata/delivery-modes");
            if (!response.ok) return [] as string[];
            return (await response.json()) as string[];
        },
    });

    const audienceOptions = audiences.map((value) => ({
        value,
        label: value === "ELU" ? "Élu" : value === "PRESIDENT" ? "Président" : value,
    }));

    const deliveryModeOptions = deliveryModes.map((value) => ({
        value,
        label: value === "PRESENTIEL" ? "Présentiel" : value === "DISTANCIEL" ? "Distanciel" : value,
    }));

    const {
        data: sessionsResult,
        isLoading: sessionsLoading,
        isError: sessionsError,
    } = useQuery({
        queryKey: ["public-sessions", queryString],
        queryFn: async () => {
            const response = await fetch(`/_api/public/sessions${queryString}`);
            if (!response.ok) return null;
            return (await response.json()) as PagedResult<PublicSessionSummary>;
        },
    });

    const totalPages = Math.max(1, Math.ceil((sessionsResult?.totalCount ?? 0) / pageSize));

    if (sessionsError) {
        notifyError("Impossible de charger les sessions.");
    }

    return (
        <MantineProvider>
            <AppNotifications />
            <Container size="md" py="xl">
                <Stack gap="lg">
                    <PublicHeader
                        isAuthenticated={Boolean(authStatus?.isAuthenticated)}
                        onLogout={async () => {
                            await logout();
                            queryClient.setQueryData(["auth-status"], { isAuthenticated: false, roles: [] });
                            await queryClient.invalidateQueries({ queryKey: ["auth-status"] });
                            notifySuccess("Déconnexion effectuée.");
                        }}
                    />
                    <Routes>
                        <Route
                            path="/"
                            element={
                                <PublicListPage
                                    targetAudience={targetAudience}
                                    deliveryMode={deliveryMode}
                                    startDateFrom={startDateFrom}
                                    startDateTo={startDateTo}
                                    audienceOptions={audienceOptions}
                                    deliveryModeOptions={deliveryModeOptions}
                                    onTargetAudienceChange={(value) => {
                                        setPage(1);
                                        setTargetAudience(value);
                                    }}
                                    onDeliveryModeChange={(value) => {
                                        setPage(1);
                                        setDeliveryMode(value);
                                    }}
                                    onStartDateFromChange={(value) => {
                                        setPage(1);
                                        setStartDateFrom(value);
                                    }}
                                    onStartDateToChange={(value) => {
                                        setPage(1);
                                        setStartDateTo(value);
                                    }}
                                    sessions={sessionsResult?.items ?? []}
                                    loading={sessionsLoading}
                                />
                            }
                        />
                        <Route path="/session/:id" element={<PublicDetailPage />} />
                    </Routes>
                    {totalPages > 1 && (
                        <Group justify="center">
                            <Pagination value={page} onChange={setPage} total={totalPages} />
                        </Group>
                    )}
                    {!sessionsLoading && (sessionsResult?.items?.length ?? 0) === 0 && <Text c="dimmed">Aucune session disponible.</Text>}
                </Stack>
            </Container>
        </MantineProvider>
    );
}

const root = ReactDOM.createRoot(document.getElementById("react-app") as ReactContainer);
root.render(
    <QueryClientProvider client={queryClient}>
        <BrowserRouter>
            <App />
        </BrowserRouter>
    </QueryClientProvider>
);
