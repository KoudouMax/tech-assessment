import { Card, Stack, Text } from "@mantine/core";
import type React from "react";
import type { CourseFormState } from "../components/CourseForm";
import { CourseForm } from "../components/CourseForm";
import type { Course } from "../types";
import { CourseList } from "../components/CourseList";

type CoursesPageProps = {
  courses: Course[];
  form: CourseFormState;
  isEditing: boolean;
  audienceOptions: { value: string; label: string }[];
  onChange: React.Dispatch<React.SetStateAction<CourseFormState>>;
  onSubmit: () => Promise<void>;
  onCancelEdit: () => void;
  onEdit: (course: Course) => void;
  onDelete: (course: Course) => void;
};

export function CoursesPage(props: CoursesPageProps) {
  return (
    <Stack>
      <Card withBorder>
        <CourseForm
          form={props.form}
          onChange={props.onChange}
          onSubmit={props.onSubmit}
          isEditing={props.isEditing}
          onCancel={props.onCancelEdit}
          audienceOptions={props.audienceOptions}
        />
      </Card>
      <CourseList
        courses={props.courses}
        onEdit={props.onEdit}
        onDelete={props.onDelete}
      />
      {props.courses.length === 0 && (
        <Text c="dimmed">Aucun cours sur cette page.</Text>
      )}
    </Stack>
  );
}
