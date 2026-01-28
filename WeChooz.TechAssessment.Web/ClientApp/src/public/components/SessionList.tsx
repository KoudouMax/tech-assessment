import { Button, Card, Group, Stack, Text, Title } from "@mantine/core";

export type PublicSessionSummary = {
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
};

type SessionListProps = {
    sessions: PublicSessionSummary[];
    onOpen: (id: string) => void;
};

export function SessionList(props: SessionListProps) {
    return (
        <Stack>
            {props.sessions.map((session) => (
                <Card key={session.id} withBorder>
                    <Stack gap="xs">
                        <Title order={3}>{session.courseName}</Title>
                        <Text c="dimmed">{session.shortDescription}</Text>
                        <Group gap="lg">
                            <Text>Audience: {session.targetAudience}</Text>
                            <Text>Début: {session.startDate}</Text>
                            <Text>Durée: {session.durationDays} jours</Text>
                        </Group>
                        <Group gap="lg">
                            <Text>Mode: {session.deliveryMode}</Text>
                            <Text>Places restantes: {session.remainingSeats}</Text>
                            <Text>
                                Formateur: {session.trainerFirstName} {session.trainerLastName}
                            </Text>
                        </Group>
                        <Group justify="flex-end">
                            <Button variant="light" onClick={() => props.onOpen(session.id)}>
                                Voir le détail
                            </Button>
                        </Group>
                    </Stack>
                </Card>
            ))}
            {props.sessions.length === 0 && <Text c="dimmed">Aucune session disponible.</Text>}
        </Stack>
    );
}
