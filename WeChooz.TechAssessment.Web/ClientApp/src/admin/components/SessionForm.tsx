import { Button, Group, Select, Stack, TextInput, Title } from "@mantine/core";
import type React from "react";
import { CourseSummary } from "./CourseList";

export type SessionFormState = {
    courseId: string;
    startDate: string;
    deliveryMode: string;
    location: string;
};

type SessionFormProps = {
    form: SessionFormState;
    courses: CourseSummary[];
    onChange: React.Dispatch<React.SetStateAction<SessionFormState>>;
    onSubmit: () => Promise<void>;
    deliveryModeOptions: { value: string; label: string }[];
};

export function SessionForm(props: SessionFormProps) {
    return (
        <Stack>
            <Title order={3}>Créer une session</Title>
            <Select
                label="Cours"
                placeholder="Choisir un cours"
                data={props.courses.map((course) => ({ value: course.id, label: course.name }))}
                value={props.form.courseId}
                onChange={(value) => props.onChange({ ...props.form, courseId: value ?? "" })}
            />
            <Group grow>
                <TextInput label="Date de début" type="date" value={props.form.startDate} onChange={(e) => props.onChange({ ...props.form, startDate: e.currentTarget.value })} />
            <Select
                label="Mode"
                data={props.deliveryModeOptions}
                value={props.form.deliveryMode}
                onChange={(value) => props.onChange({ ...props.form, deliveryMode: value ?? "PRESENTIEL" })}
            />
            </Group>
            <TextInput label="Lieu (optionnel)" value={props.form.location} onChange={(e) => props.onChange({ ...props.form, location: e.currentTarget.value })} />
            <Button onClick={props.onSubmit} disabled={!props.form.courseId || !props.form.startDate}>
                Créer
            </Button>
        </Stack>
    );
}
