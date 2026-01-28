import { Group, Pagination, Stack } from "@mantine/core";
import type React from "react";
import {
  Navigate,
  createBrowserRouter,
  redirect,
  useOutletContext,
  useRouteLoaderData,
} from "react-router-dom";
import type { AdminOutletContext, AuthStatus } from "../types";
import { AdminLayout } from "./AdminLayout";
import { CoursesPage } from "../pages/CoursesPage";
import { ParticipantsPage } from "../pages/ParticipantsPage";
import { SessionsPage } from "../pages/SessionsPage";
import { CourseDetailPage } from "../components/CourseDetailPage";

function RequireRole(props: { role: "formation"; children: React.ReactNode }) {
  const auth = useRouteLoaderData("root") as AuthStatus;
  const hasRole = auth.roles.includes(props.role);

  if (!hasRole) {
    return <Navigate to="/participants" replace />;
  }

  return <>{props.children}</>;
}

function DefaultRoute() {
  const auth = useRouteLoaderData("root") as AuthStatus;
  const canManageCourses = auth.roles.includes("formation");
  return <Navigate to={canManageCourses ? "/courses" : "/participants"} replace />;
}

function CoursesRoute() {
  const context = useOutletContext<AdminOutletContext>();

  return (
    <Stack>
      <CoursesPage
        courses={context.courses}
        form={context.courseForm}
        isEditing={context.isEditingCourse}
        audienceOptions={context.audienceOptions}
        onChange={context.onCourseFormChange}
        onSubmit={context.onCreateOrUpdateCourse}
        onCancelEdit={context.onCancelCourseEdit}
        onEdit={context.onEditCourse}
        onDelete={context.onDeleteCourse}
      />
      <Group justify="center">
        <Pagination
          value={context.coursesPage}
          onChange={context.setCoursesPage}
          total={context.coursesTotalPages}
        />
      </Group>
    </Stack>
  );
}

function SessionsRoute() {
  const context = useOutletContext<AdminOutletContext>();

  return (
    <Stack>
      <SessionsPage
        sessions={context.sessions}
        courses={context.courses}
        form={context.sessionForm}
        deliveryModeOptions={context.deliveryModeOptions}
        onChange={context.onSessionFormChange}
        onSubmit={context.onCreateSession}
      />
      <Group justify="center">
        <Pagination
          value={context.sessionsPage}
          onChange={context.setSessionsPage}
          total={context.sessionsTotalPages}
        />
      </Group>
    </Stack>
  );
}

function ParticipantsRoute() {
  const context = useOutletContext<AdminOutletContext>();

  return (
    <Stack>
      <ParticipantsPage
        sessionOptions={context.sessionOptions}
        selectedSessionId={context.selectedSessionId}
        onSelectSession={context.onSelectSession}
        form={context.participantForm}
        onChange={context.onParticipantFormChange}
        onSubmit={context.onAddParticipant}
        participants={context.participants}
      />
      {context.selectedSessionId && (
        <Group justify="center">
          <Pagination
            value={context.participantsPage}
            onChange={context.setParticipantsPage}
            total={context.participantsTotalPages}
          />
        </Group>
      )}
    </Stack>
  );
}

export const adminRouter = createBrowserRouter(
  [
    {
      id: "root",
      path: "/",
      loader: async () => {
        const response = await fetch("/_api/auth/status", {
          credentials: "include",
        });
        if (!response.ok) {
          throw redirect("/Account/Login?ReturnUrl=%2Fadmin");
        }
        const data = (await response.json()) as AuthStatus;
        if (!data.isAuthenticated) {
          throw redirect("/Account/Login?ReturnUrl=%2Fadmin");
        }
        return data;
      },
      element: <AdminLayout />,
      children: [
        { index: true, element: <DefaultRoute /> },
        {
          path: "courses",
          element: (
            <RequireRole role="formation">
              <CoursesRoute />
            </RequireRole>
          ),
        },
        {
          path: "courses/:id",
          element: (
            <RequireRole role="formation">
              <CourseDetailPage enabled={true} />
            </RequireRole>
          ),
        },
        {
          path: "sessions",
          element: (
            <RequireRole role="formation">
              <SessionsRoute />
            </RequireRole>
          ),
        },
        { path: "participants", element: <ParticipantsRoute /> },
      ],
    },
  ],
  { basename: "/admin" },
);
