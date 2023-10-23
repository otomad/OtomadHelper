import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import "./locales/config.ts";

// window.oncontextmenu = () => false;

ReactDOM.createRoot(document.getElementById("root")!).render(
	<React.StrictMode>
		<App />
	</React.StrictMode>,
);
