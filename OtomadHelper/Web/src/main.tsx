import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import "./initial.ts";
import "./locales/config.ts";

const StyledTest = styled.div`
	width: 500px;
	height: 500px;

	&.enter {
		scale: 0.5;
		opacity: 0;
	}
	&.enter-active,
	&.enter-done {
		scale: 1;
		opacity: 1;
		transition: all ease-out 250ms;
	}
	&.exit {
		scale: 1;
		opacity: 1;
	}
	&.exit-active,
	&.exit-done {
		scale: 0.5;
		opacity: 0;
		transition: all ease-out 250ms;
	}
`;

const Test = () => {
	const [shown, setShown] = useState(false);
	const backgroundColor = shown ? "red" : "green";
	return (
		<>
			<button type="button" onClick={() => setShown(shown => !shown)}>Toggle</button>
			<SwitchTransition mode="out-in-preload">
				<CssTransition key={backgroundColor}>
					<StyledTest style={{ backgroundColor }} />
				</CssTransition>
			</SwitchTransition>
		</>
	);
};

ReactDOM.createRoot(document.getElementById("root")!).render(
	<React.StrictMode>
		{/* <Test /> */}
		<App />
	</React.StrictMode>,
);
