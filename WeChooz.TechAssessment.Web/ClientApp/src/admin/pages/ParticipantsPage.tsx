import type { Participant } from "../components/ParticipantsPanel";
import { ParticipantsPanel } from "../components/ParticipantsPanel";
import type React from "react";

type ParticipantsPageProps = {
  sessionOptions: { value: string; label: string }[];
  selectedSessionId: string | null;
  onSelectSession: (sessionId: string | null) => void;
  form: {
    firstName: string;
    lastName: string;
    email: string;
    companyName: string;
  };
  onChange: React.Dispatch<
    React.SetStateAction<{
      firstName: string;
      lastName: string;
      email: string;
      companyName: string;
    }>
  >;
  onSubmit: () => Promise<void>;
  participants: Participant[];
};

export function ParticipantsPage(props: ParticipantsPageProps) {
  return (
    <ParticipantsPanel
      sessionOptions={props.sessionOptions}
      selectedSessionId={props.selectedSessionId}
      onSelectSession={props.onSelectSession}
      form={props.form}
      onChange={props.onChange}
      onSubmit={props.onSubmit}
      participants={props.participants}
    />
  );
}
