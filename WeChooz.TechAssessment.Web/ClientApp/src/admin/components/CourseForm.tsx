import { Button, Group, NumberInput, Select, Stack, Textarea, TextInput, Title } from "@mantine/core";
import type React from "react";

export type CourseFormState = {
    name: string;
    shortDescription: string;
    longDescriptionMarkdown: string;
    durationDays: number;
    targetAudience: string;
    maxCapacity: number;
    trainerFirstName: string;
    trainerLastName: string;
};

type CourseFormProps = {
    form: CourseFormState;
    onChange: React.Dispatch<React.SetStateAction<CourseFormState>>;
    onSubmit: () => Promise<void>;
    isEditing: boolean;
    onCancel?: () => void;
    audienceOptions: { value: string; label: string }[];
};

export function CourseForm(props: CourseFormProps) {
    return (
        <Stack>
            <Title order={3}>Créer un cours</Title>
            <TextInput label="Nom" value={props.form.name} onChange={(e) => props.onChange({ ...props.form, name: e.currentTarget.value })} />
            <Textarea
                label="Description courte"
                autosize
                minRows={2}
                value={props.form.shortDescription}
                onChange={(e) => props.onChange({ ...props.form, shortDescription: e.currentTarget.value })}
            />
            <Textarea
                label="Description longue (markdown)"
                autosize
                minRows={6}
                value={props.form.longDescriptionMarkdown}
                onChange={(e) => props.onChange({ ...props.form, longDescriptionMarkdown: e.currentTarget.value })}
            />
            <Group grow>
                <NumberInput label="Durée (jours)" min={1} value={props.form.durationDays} onChange={(value) => props.onChange({ ...props.form, durationDays: Number(value) })} />
                <NumberInput label="Capacité max" min={1} value={props.form.maxCapacity} onChange={(value) => props.onChange({ ...props.form, maxCapacity: Number(value) })} />
            </Group>
            <Select
                label="Population cible"
                data={props.audienceOptions}
                value={props.form.targetAudience}
                onChange={(value) => props.onChange({ ...props.form, targetAudience: value ?? "ELU" })}
            />
            <Group grow>
                <TextInput label="Prénom formateur" value={props.form.trainerFirstName} onChange={(e) => props.onChange({ ...props.form, trainerFirstName: e.currentTarget.value })} />
                <TextInput label="Nom formateur" value={props.form.trainerLastName} onChange={(e) => props.onChange({ ...props.form, trainerLastName: e.currentTarget.value })} />
            </Group>
            <Group justify="flex-end">
                {props.isEditing && (
                    <Button variant="default" onClick={props.onCancel}>
                        Annuler
                    </Button>
                )}
                <Button onClick={props.onSubmit}>{props.isEditing ? "Mettre à jour" : "Créer"}</Button>
            </Group>
        </Stack>
    );
}
