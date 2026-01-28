import { notifications } from "@mantine/notifications";

export function notifySuccess(message: string) {
    notifications.show({
        color: "teal",
        title: "Succès",
        message,
    });
}

export function notifyError(message: string) {
    notifications.show({
        color: "red",
        title: "Erreur",
        message,
    });
}
