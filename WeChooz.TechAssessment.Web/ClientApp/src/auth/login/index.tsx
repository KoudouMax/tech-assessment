import "@mantine/core/styles.css";
import { Button, Card, Container, MantineProvider, Select, Stack, Text, Title } from "@mantine/core";
import React, { useMemo, useState } from "react";
import ReactDOM, { Container as ReactContainer } from "react-dom/client";
import { apiFetch } from "../../shared/api";

async function loginRequest(login: string) {
    await apiFetch("/_api/admin/login", {
        method: "POST",
        body: JSON.stringify({ login }),
    });
}

function App() {
    const [login, setLogin] = useState("");
    const [errorMessage, setErrorMessage] = useState<string | null>(null);

    const returnUrl = useMemo(() => {
        const params = new URLSearchParams(window.location.search);
        return params.get("ReturnUrl") ?? "/admin";
    }, []);

    const doLogin = async () => {
        setErrorMessage(null);
        try {
            await loginRequest(login);
            window.location.assign(returnUrl);
        } catch (error) {
            setErrorMessage(error instanceof Error ? error.message : "Erreur de connexion");
        }
    };

    return (
        <MantineProvider>
            <Container size="xs" py="xl">
                <Card withBorder>
                    <Stack>
                        <Title order={2}>Connexion admin</Title>
                        <Text c="dimmed">Choisis un rôle pour te connecter.</Text>
                        <Select
                            label="Login"
                            placeholder="Choisir un login"
                            data={[
                                { value: "formation", label: "formation" },
                                { value: "sales", label: "sales" },
                            ]}
                            value={login}
                            onChange={(value) => setLogin(value ?? "")}
                        />
                        {errorMessage && <Text c="red">{errorMessage}</Text>}
                        <Button onClick={doLogin} disabled={!login}>
                            Se connecter
                        </Button>
                    </Stack>
                </Card>
            </Container>
        </MantineProvider>
    );
}

const root = ReactDOM.createRoot(document.getElementById("react-app") as ReactContainer);
root.render(<App />);
