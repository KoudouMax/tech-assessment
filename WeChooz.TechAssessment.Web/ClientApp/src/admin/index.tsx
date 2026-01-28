import "@mantine/core/styles.css";
import "@mantine/notifications/styles.css";
import React from "react";
import ReactDOM, { Container as ReactContainer } from "react-dom/client";
import { QueryClientProvider } from "@tanstack/react-query";
import { RouterProvider } from "react-router-dom";
import { queryClient } from "../shared/queryClient";
import { adminRouter } from "./routes/adminRouter";

const root = ReactDOM.createRoot(
  document.getElementById("react-app") as ReactContainer,
);
root.render(
  <QueryClientProvider client={queryClient}>
    <RouterProvider router={adminRouter} />
  </QueryClientProvider>,
);
