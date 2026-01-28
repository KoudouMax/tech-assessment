import type { CourseFormState } from "./components/CourseForm";
import type { CourseSummary } from "./components/CourseList";
import type { Participant } from "./components/ParticipantsPanel";
import type { SessionFormState } from "./components/SessionForm";
import type { SessionSummary } from "./components/SessionList";
import type React from "react";

export type Course = CourseSummary & {
  longDescriptionMarkdown: string;
  durationDays: number;
  maxCapacity: number;
  trainerFirstName: string;
  trainerLastName: string;
};

export type Session = SessionSummary & {
  courseId: string;
  location?: string | null;
};

export type ApiError = { status: number; message: string };

export type AuthStatus = { isAuthenticated: boolean; roles: string[] };

export type AdminOutletContext = {
  canManageCourses: boolean;
  courses: Course[];
  sessions: Session[];
  participants: Participant[];
  courseForm: CourseFormState;
  sessionForm: SessionFormState;
  participantForm: {
    firstName: string;
    lastName: string;
    email: string;
    companyName: string;
  };
  isEditingCourse: boolean;
  audienceOptions: { value: string; label: string }[];
  deliveryModeOptions: { value: string; label: string }[];
  sessionOptions: { value: string; label: string }[];
  onCourseFormChange: React.Dispatch<React.SetStateAction<CourseFormState>>;
  onSessionFormChange: React.Dispatch<React.SetStateAction<SessionFormState>>;
  onParticipantFormChange: React.Dispatch<
    React.SetStateAction<{
      firstName: string;
      lastName: string;
      email: string;
      companyName: string;
    }>
  >;
  onCreateOrUpdateCourse: () => Promise<void>;
  onCancelCourseEdit: () => void;
  onEditCourse: (course: Course) => void;
  onDeleteCourse: (course: Course) => void;
  onCreateSession: () => Promise<void>;
  onAddParticipant: () => Promise<void>;
  onSelectSession: (sessionId: string | null) => void;
  selectedSessionId: string | null;
  coursesPage: number;
  sessionsPage: number;
  participantsPage: number;
  coursesTotalPages: number;
  sessionsTotalPages: number;
  participantsTotalPages: number;
  setCoursesPage: (page: number) => void;
  setSessionsPage: (page: number) => void;
  setParticipantsPage: (page: number) => void;
};
