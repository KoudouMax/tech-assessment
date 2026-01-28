import { Anchor, Card, Group, Stack, Text, Title } from "@mantine/core";
import { MarkdownRenderer } from "../../shared/MarkdownRenderer";

export type PublicSessionDetail = {
    id: string;
    courseName: string;
    shortDescription: string;
    targetAudience: string;
    startDate: string;
    durationDays: number;
    deliveryMode: string;
    remainingSeats: number;
    trainerFirstName: string;
    trainerLastName: string;
    longDescriptionMarkdown: string;
    longDescriptionHtml: string;
};

type SessionDetailProps = {
    session: PublicSessionDetail | null;
    onBack: () => void;
};

export function SessionDetail(props: SessionDetailProps) {
    return (
        <Card withBorder>
            {props.session ? (
                <Stack gap="md">
                    <Anchor onClick={props.onBack}>&larr; Retour à la liste</Anchor>
                    <Title order={2}>{props.session.courseName}</Title>
                    <Text>{props.session.shortDescription}</Text>
                    <Text c="dimmed">
                        {props.session.startDate} • {props.session.durationDays} jours • {props.session.deliveryMode}
                    </Text>
                    <Group gap="lg">
                        <Text>Audience: {props.session.targetAudience}</Text>
                        <Text>Places restantes: {props.session.remainingSeats}</Text>
                    </Group>
                    <Text>
                        Formateur: {props.session.trainerFirstName} {props.session.trainerLastName}
                    </Text>
                    <MarkdownRenderer html={props.session.longDescriptionHtml} />
                </Stack>
            ) : (
                <Text c="dimmed">Chargement...</Text>
            )}
        </Card>
    );
}
