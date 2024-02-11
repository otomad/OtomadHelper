import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import initial from "./initial.ts";
import "./locales/config.ts";

await initial();

ReactDOM.createRoot(document.getElementById("root")!).render(
	// <React.StrictMode>
	<App />,
	// </React.StrictMode>,
);
