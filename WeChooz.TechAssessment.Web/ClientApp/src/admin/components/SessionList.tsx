import { Card, Group, Stack, Text, Title } from "@mantine/core";

export type SessionSummary = {
    id: string;
    courseName: string;
    startDate: string;
    deliveryMode: string;
    participantsCount: number;
};

type SessionListProps = {
    sessions: SessionSummary[];
};

export function SessionList(props: SessionListProps) {
    return (
        <Card withBorder>
            <Stack>
                <Title order={3}>Sessions existantes</Title>
                {props.sessions.map((session) => (
                    <Card key={session.id} withBorder>
                        <Group justify="space-between">
                            <Text fw={600}>{session.courseName}</Text>
                            <Text c="dimmed">{session.startDate}</Text>
                        </Group>
                        <Text size="sm">
                            {session.deliveryMode} • {session.participantsCount} participant(s)
                        </Text>
                    </Card>
                ))}
                {props.sessions.length === 0 && <Text c="dimmed">Aucune session pour le moment.</Text>}
            </Stack>
        </Card>
    );
}
