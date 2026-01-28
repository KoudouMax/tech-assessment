import {
  Button,
  Container,
  Group,
  Loader,
  MantineProvider,
  Stack,
  Text,
  Title,
} from "@mantine/core";
import React, { useMemo, useState } from "react";
import { Link, Outlet, useRouteLoaderData } from "react-router-dom";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { apiFetch, logout, PagedResult } from "../../shared/api";
import { AppNotifications } from "../../shared/ui";
import { notifyError, notifySuccess } from "../../shared/toast";
import type { AdminOutletContext, ApiError, AuthStatus, Course, Session } from "../types";
import type { Participant } from "../components/ParticipantsPanel";
import type { CourseFormState } from "../components/CourseForm";
import type { SessionFormState } from "../components/SessionForm";

export function AdminLayout() {
  const queryClient = useQueryClient();
  const auth = useRouteLoaderData("root") as AuthStatus;
  const canManageCourses = auth.roles.includes("formation");
  const [selectedSessionId, setSelectedSessionId] = useState<string | null>(null);
  const [editingCourseId, setEditingCourseId] = useState<string | null>(null);
  const [coursesPage, setCoursesPage] = useState(1);
  const [sessionsPage, setSessionsPage] = useState(1);
  const [participantsPage, setParticipantsPage] = useState(1);
  const pageSize = 10;

  const [courseForm, setCourseForm] = useState<CourseFormState>({
    name: "",
    shortDescription: "",
    longDescriptionMarkdown: "",
    durationDays: 1,
    targetAudience: "ELU",
    maxCapacity: 10,
    trainerFirstName: "",
    trainerLastName: "",
  });

  const [sessionForm, setSessionForm] = useState<SessionFormState>({
    courseId: "",
    startDate: "",
    deliveryMode: "PRESENTIEL",
    location: "",
  });

  const [participantForm, setParticipantForm] = useState({
    firstName: "",
    lastName: "",
    email: "",
    companyName: "",
  });

  const sessionsQuery = useQuery<PagedResult<Session>, ApiError>({
    queryKey: ["admin-sessions", sessionsPage],
    queryFn: () =>
      apiFetch<PagedResult<Session>>(
        `/_api/admin/sessions?page=${sessionsPage}&pageSize=${pageSize}`,
      ),
    retry: false,
    onError: () => notifyError("Impossible de charger les sessions."),
  });

  const coursesQuery = useQuery<PagedResult<Course>, ApiError>({
    queryKey: ["admin-courses", coursesPage],
    queryFn: () =>
      apiFetch<PagedResult<Course>>(
        `/_api/admin/courses?page=${coursesPage}&pageSize=${pageSize}`,
      ),
    retry: false,
    enabled: canManageCourses,
    onError: (error) => {
      if (error.status !== 403) {
        notifyError("Impossible de charger les cours.");
      }
    },
  });

  const audiencesQuery = useQuery<string[], ApiError>({
    queryKey: ["metadata-audiences"],
    queryFn: () => apiFetch<string[]>("/_api/metadata/audiences"),
  });

  const deliveryModesQuery = useQuery<string[], ApiError>({
    queryKey: ["metadata-delivery-modes"],
    queryFn: () => apiFetch<string[]>("/_api/metadata/delivery-modes"),
  });

  const participantsQuery = useQuery<PagedResult<Participant>, ApiError>({
    queryKey: ["session-participants", selectedSessionId, participantsPage],
    queryFn: () =>
      apiFetch<PagedResult<Participant>>(
        `/_api/admin/sessions/${selectedSessionId}/participants?page=${participantsPage}&pageSize=${pageSize}`,
      ),
    enabled: Boolean(selectedSessionId),
    retry: false,
    onError: () => notifyError("Impossible de charger les participants."),
  });

  const sessionOptions = useMemo(
    () =>
      (sessionsQuery.data?.items ?? []).map((session) => ({
        value: session.id,
        label: `${session.courseName} — ${session.startDate}`,
      })),
    [sessionsQuery.data],
  );

  const coursesTotalPages = Math.max(
    1,
    Math.ceil((coursesQuery.data?.totalCount ?? 0) / pageSize),
  );
  const sessionsTotalPages = Math.max(
    1,
    Math.ceil((sessionsQuery.data?.totalCount ?? 0) / pageSize),
  );
  const participantsTotalPages = Math.max(
    1,
    participantsQuery.data &&
      typeof participantsQuery.data.totalCount === "number"
      ? Math.ceil(participantsQuery.data.totalCount / pageSize)
      : 1,
  );

  const audienceOptions = useMemo(
    () =>
      (audiencesQuery.data ?? []).map((value) => ({
        value,
        label: value === "ELU" ? "Élu" : value === "PRESIDENT" ? "Président" : value,
      })),
    [audiencesQuery.data],
  );

  const deliveryModeOptions = useMemo(
    () =>
      (deliveryModesQuery.data ?? []).map((value) => ({
        value,
        label:
          value === "PRESENTIEL"
            ? "Présentiel"
            : value === "DISTANCIEL"
              ? "Distanciel"
              : value,
      })),
    [deliveryModesQuery.data],
  );

  const createCourseMutation = useMutation({
    mutationFn: (payload: typeof courseForm) =>
      apiFetch<Course>("/_api/admin/courses", {
        method: "POST",
        body: JSON.stringify(payload),
      }),
    onSuccess: async () => {
      resetCourseForm();
      await queryClient.invalidateQueries({ queryKey: ["admin-courses"] });
      notifySuccess("Cours créé.");
    },
    onError: (error: ApiError) => notifyError(error.message),
  });

  const updateCourseMutation = useMutation({
    mutationFn: (payload: { id: string; data: typeof courseForm }) =>
      apiFetch<Course>(`/_api/admin/courses/${payload.id}`, {
        method: "PUT",
        body: JSON.stringify(payload.data),
      }),
    onSuccess: async () => {
      resetCourseForm();
      await queryClient.invalidateQueries({ queryKey: ["admin-courses"] });
      notifySuccess("Cours mis à jour.");
    },
    onError: (error: ApiError) => notifyError(error.message),
  });

  const deleteCourseMutation = useMutation({
    mutationFn: (id: string) =>
      apiFetch<void>(`/_api/admin/courses/${id}`, { method: "DELETE" }),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ["admin-courses"] });
      notifySuccess("Cours supprimé.");
    },
    onError: (error: ApiError) => notifyError(error.message),
  });

  const createSessionMutation = useMutation({
    mutationFn: (payload: typeof sessionForm) =>
      apiFetch<Session>("/_api/admin/sessions", {
        method: "POST",
        body: JSON.stringify(payload),
      }),
    onSuccess: async () => {
      setSessionForm({
        courseId: "",
        startDate: "",
        deliveryMode: "PRESENTIEL",
        location: "",
      });
      await queryClient.invalidateQueries({ queryKey: ["admin-sessions"] });
      notifySuccess("Session créée.");
    },
    onError: (error: ApiError) => notifyError(error.message),
  });

  const addParticipantMutation = useMutation({
    mutationFn: (payload: typeof participantForm) =>
      apiFetch(`/_api/admin/sessions/${selectedSessionId}/participants`, {
        method: "POST",
        body: JSON.stringify(payload),
      }),
    onSuccess: async () =>
    {
      setParticipantForm({
        firstName: "",
        lastName: "",
        email: "",
        companyName: "",
      });
      await queryClient.invalidateQueries({
        queryKey: ["session-participants", selectedSessionId],
      });
      await queryClient.invalidateQueries({ queryKey: ["admin-sessions"] });
      notifySuccess("Participant ajouté.");
    },
    onError: (error: ApiError) => notifyError(error.message),
  });

  const resetCourseForm = () => {
    setCourseForm({
      name: "",
      shortDescription: "",
      longDescriptionMarkdown: "",
      durationDays: 1,
      targetAudience: "ELU",
      maxCapacity: 10,
      trainerFirstName: "",
      trainerLastName: "",
    });
    setEditingCourseId(null);
  };

  if (sessionsQuery.isLoading) {
    return (
      <MantineProvider>
        <Container size="xs" py="xl">
          <Loader />
        </Container>
      </MantineProvider>
    );
  }

  const outletContext: AdminOutletContext = {
    canManageCourses,
    courses: coursesQuery.data?.items ?? [],
    sessions: sessionsQuery.data?.items ?? [],
    participants: participantsQuery.data?.items ?? [],
    courseForm,
    sessionForm,
    participantForm,
    isEditingCourse: Boolean(editingCourseId),
    audienceOptions,
    deliveryModeOptions,
    sessionOptions,
    onCourseFormChange: setCourseForm,
    onSessionFormChange: setSessionForm,
    onParticipantFormChange: setParticipantForm,
    onCreateOrUpdateCourse: () =>
      editingCourseId
        ? updateCourseMutation.mutateAsync({ id: editingCourseId, data: courseForm })
        : createCourseMutation.mutateAsync(courseForm),
    onCancelCourseEdit: resetCourseForm,
    onEditCourse: (course: Course) => {
      setEditingCourseId(course.id);
      setCourseForm({
        name: course.name,
        shortDescription: course.shortDescription,
        longDescriptionMarkdown: course.longDescriptionMarkdown,
        durationDays: course.durationDays,
        targetAudience: course.targetAudience,
        maxCapacity: course.maxCapacity,
        trainerFirstName: course.trainerFirstName,
        trainerLastName: course.trainerLastName,
      });
    },
    onDeleteCourse: (course: Course) => {
      if (!window.confirm(`Supprimer le cours "${course.name}" ?`)) return;
      void deleteCourseMutation.mutateAsync(course.id);
    },
    onCreateSession: () => createSessionMutation.mutateAsync(sessionForm),
    onAddParticipant: () => addParticipantMutation.mutateAsync(participantForm),
    onSelectSession: (value: string | null) => {
      setParticipantsPage(1);
      setSelectedSessionId(value);
    },
    selectedSessionId,
    coursesPage,
    sessionsPage,
    participantsPage,
    coursesTotalPages,
    sessionsTotalPages,
    participantsTotalPages,
    setCoursesPage,
    setSessionsPage,
    setParticipantsPage,
  };

  return (
    <MantineProvider>
      <AppNotifications />
      <Container size="lg" py="xl">
        <Stack gap="lg">
          <Group justify="space-between" align="center">
            <Title order={1}>Administration</Title>
            <Button
              variant="outline"
              onClick={async () => {
                await logout();
                window.location.assign("/");
              }}
            >
              Se déconnecter
            </Button>
          </Group>
          <Group>
            {canManageCourses && (
              <>
                <Button component={Link} to="/courses" variant="light">
                  Cours
                </Button>
                <Button component={Link} to="/sessions" variant="light">
                  Sessions
                </Button>
              </>
            )}
            <Button component={Link} to="/participants" variant="light">
              Participants
            </Button>
          </Group>

          <Outlet context={outletContext} />
        </Stack>
      </Container>
    </MantineProvider>
  );
}
