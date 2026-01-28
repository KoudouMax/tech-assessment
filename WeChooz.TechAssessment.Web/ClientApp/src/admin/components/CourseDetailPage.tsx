import { Anchor, Card, Group, Stack, Text, Title } from "@mantine/core";
import { useQuery } from "@tanstack/react-query";
import { Link, useParams } from "react-router-dom";
import { apiFetch } from "../../shared/api";
import type { CourseSummary } from "./CourseList";

export type CourseDetail = CourseSummary & {
    longDescriptionMarkdown: string;
    durationDays: number;
    maxCapacity: number;
    trainerFirstName: string;
    trainerLastName: string;
};

type CourseDetailPageProps = {
    enabled: boolean;
};

export function CourseDetailPage(props: CourseDetailPageProps) {
    const { id } = useParams();

    const { data } = useQuery({
        queryKey: ["admin-course", id],
        enabled: props.enabled && Boolean(id),
        queryFn: () => apiFetch<CourseDetail>(`/_api/admin/courses/${id}`),
    });

    if (!props.enabled) {
        return <Text c="dimmed">Accès réservé au rôle formation.</Text>;
    }

    if (!data) {
        return (
            <Card withBorder>
                <Text c="dimmed">Chargement...</Text>
            </Card>
        );
    }

    return (
        <Card withBorder>
            <Stack>
                <Anchor component={Link} to="/courses">&larr; Retour</Anchor>
                <Title order={2}>{data.name}</Title>
                <Text>{data.shortDescription}</Text>
                <Group gap="lg">
                    <Text>Audience: {data.targetAudience}</Text>
                    <Text>Durée: {data.durationDays} jours</Text>
                    <Text>Capacité: {data.maxCapacity}</Text>
                </Group>
                <Text>
                    Formateur: {data.trainerFirstName} {data.trainerLastName}
                </Text>
                <Text fw={600}>Description longue (markdown)</Text>
                <Text component="pre" style={{ whiteSpace: "pre-wrap", margin: 0 }}>
                    {data.longDescriptionMarkdown}
                </Text>
            </Stack>
        </Card>
    );
}
