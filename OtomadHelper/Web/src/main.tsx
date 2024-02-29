import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import "./initial.ts";
import "./locales/config.ts";

ReactDOM.createRoot(document.getElementById("root")!).render(
	// <React.StrictMode>
	<App />,
	// </React.StrictMode>,
);
