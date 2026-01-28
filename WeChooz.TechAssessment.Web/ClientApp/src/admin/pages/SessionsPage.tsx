import { Card, Stack, Text } from "@mantine/core";
import type React from "react";
import type { SessionFormState } from "../components/SessionForm";
import { SessionForm } from "../components/SessionForm";
import type { Course, Session } from "../types";
import { SessionList } from "../components/SessionList";

type SessionsPageProps = {
  sessions: Session[];
  courses: Course[];
  form: SessionFormState;
  deliveryModeOptions: { value: string; label: string }[];
  onChange: React.Dispatch<React.SetStateAction<SessionFormState>>;
  onSubmit: () => Promise<void>;
};

export function SessionsPage(props: SessionsPageProps) {
  return (
    <Stack>
      <Card withBorder>
        <SessionForm
          form={props.form}
          courses={props.courses}
          onChange={props.onChange}
          onSubmit={props.onSubmit}
          deliveryModeOptions={props.deliveryModeOptions}
        />
      </Card>
      <SessionList sessions={props.sessions} />
      {props.sessions.length === 0 && (
        <Text c="dimmed">Aucune session sur cette page.</Text>
      )}
    </Stack>
  );
}
