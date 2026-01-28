import { Button, Card, Group, Select, Stack, Text, TextInput, Title } from "@mantine/core";
import type React from "react";

export type Participant = {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    companyName: string;
};

type ParticipantsPanelProps = {
    sessionOptions: { value: string; label: string }[];
    selectedSessionId: string | null;
    onSelectSession: (sessionId: string | null) => Promise<void>;
    form: {
        firstName: string;
        lastName: string;
        email: string;
        companyName: string;
    };
    onChange: React.Dispatch<React.SetStateAction<{
        firstName: string;
        lastName: string;
        email: string;
        companyName: string;
    }>>;
    onSubmit: () => Promise<void>;
    participants: Participant[];
};

export function ParticipantsPanel(props: ParticipantsPanelProps) {
    return (
        <Stack>
            <Card withBorder>
                <Stack>
                    <Title order={3}>Ajouter un participant</Title>
                    <Select
                        label="Session"
                        placeholder="Choisir une session"
                        data={props.sessionOptions}
                        value={props.selectedSessionId}
                        onChange={props.onSelectSession}
                    />
                    <Group grow>
                        <TextInput label="Prénom" value={props.form.firstName} onChange={(e) => props.onChange({ ...props.form, firstName: e.currentTarget.value })} />
                        <TextInput label="Nom" value={props.form.lastName} onChange={(e) => props.onChange({ ...props.form, lastName: e.currentTarget.value })} />
                    </Group>
                    <Group grow>
                        <TextInput label="Email" value={props.form.email} onChange={(e) => props.onChange({ ...props.form, email: e.currentTarget.value })} />
                        <TextInput label="Entreprise" value={props.form.companyName} onChange={(e) => props.onChange({ ...props.form, companyName: e.currentTarget.value })} />
                    </Group>
                    <Button onClick={props.onSubmit} disabled={!props.selectedSessionId || !props.form.email}>
                        Ajouter
                    </Button>
                </Stack>
            </Card>

            <Card withBorder>
                <Stack>
                    <Title order={3}>Participants</Title>
                    {props.participants.map((participant) => (
                        <Card key={participant.id} withBorder>
                            <Group justify="space-between">
                                <Text fw={600}>
                                    {participant.firstName} {participant.lastName}
                                </Text>
                                <Text c="dimmed">{participant.email}</Text>
                            </Group>
                            <Text size="sm">{participant.companyName}</Text>
                        </Card>
                    ))}
                    {props.participants.length === 0 && <Text c="dimmed">Aucun participant pour cette session.</Text>}
                </Stack>
            </Card>
        </Stack>
    );
}
