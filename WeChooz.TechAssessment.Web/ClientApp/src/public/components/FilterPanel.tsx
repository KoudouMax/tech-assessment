import { Card, Group, Select, Stack, TextInput } from "@mantine/core";

type FilterPanelProps = {
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
};

export function FilterPanel(props: FilterPanelProps) {
    return (
        <Card withBorder>
            <Stack gap="md">
                <Group grow>
                    <Select
                        label="Population cible"
                        placeholder="Toutes"
                        data={props.audienceOptions}
                        value={props.targetAudience}
                        onChange={props.onTargetAudienceChange}
                        clearable
                    />
                    <Select
                        label="Mode de délivrance"
                        placeholder="Tous"
                        data={props.deliveryModeOptions}
                        value={props.deliveryMode}
                        onChange={props.onDeliveryModeChange}
                        clearable
                    />
                </Group>
                <Group grow>
                    <TextInput
                        label="Date de début (après)"
                        type="date"
                        value={props.startDateFrom}
                        onChange={(event) => props.onStartDateFromChange(event.currentTarget.value)}
                    />
                    <TextInput
                        label="Date de début (avant)"
                        type="date"
                        value={props.startDateTo}
                        onChange={(event) => props.onStartDateToChange(event.currentTarget.value)}
                    />
                </Group>
            </Stack>
        </Card>
    );
}
