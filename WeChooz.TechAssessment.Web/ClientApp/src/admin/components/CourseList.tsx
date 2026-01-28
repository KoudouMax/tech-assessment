import { Button, Card, Group, Stack, Text, Title } from "@mantine/core";
import { Link } from "react-router-dom";

export type CourseSummary = {
    id: string;
    name: string;
    shortDescription: string;
    targetAudience: string;
};

type CourseListProps = {
    courses: CourseSummary[];
    onEdit: (course: CourseSummary) => void;
    onDelete: (course: CourseSummary) => void;
};

export function CourseList(props: CourseListProps) {
    return (
        <Card withBorder>
            <Stack>
                <Title order={3}>Cours existants</Title>
                {props.courses.map((course) => (
                    <Card key={course.id} withBorder>
                        <Group justify="space-between" align="flex-start">
                            <Stack gap={4}>
                                <Text fw={600}>{course.name}</Text>
                                <Text c="dimmed">{course.targetAudience}</Text>
                            </Stack>
                            <Group gap="xs">
                                <Button component={Link} to={`/courses/${course.id}`} variant="light">
                                    Voir
                                </Button>
                                <Button variant="default" onClick={() => props.onEdit(course)}>
                                    Modifier
                                </Button>
                                <Button color="red" variant="outline" onClick={() => props.onDelete(course)}>
                                    Supprimer
                                </Button>
                            </Group>
                        </Group>
                        <Text size="sm" mt="xs">
                            {course.shortDescription}
                        </Text>
                    </Card>
                ))}
                {props.courses.length === 0 && <Text c="dimmed">Aucun cours pour le moment.</Text>}
            </Stack>
        </Card>
    );
}
