import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import "./initial.ts";
import "./locales/config.ts";

const StyledTest = styled.div`
	width: 500px;
	height: 500px;
	background-color: red;

	&.enter {
		scale: 0.5;
		opacity: 0;
	}
	&.enter-active {
		scale: 1;
		opacity: 1;
		transition: all ease-out 250ms;
	}
	&.leave {
		scale: 1;
		opacity: 1;
	}
	&.leave-active {
		scale: 0.5;
		opacity: 0;
		transition: all ease-out 250ms;
	}
`;

const Test = () => {
	const [shown, setShown] = useState(true);
	return (
		<>
			<button type="button" onClick={() => setShown(shown => !shown)}>Toggle</button>
			<CssTransition in={shown}>
				<StyledTest />
			</CssTransition>
		</>
	);
};

ReactDOM.createRoot(document.getElementById("root")!).render(
	<React.StrictMode>
		{/* <Test /> */}
		<App />
	</React.StrictMode>,
);
