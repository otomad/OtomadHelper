import Navigation from "pages/Navigation";
import GlobalStyle from "./styles/global.ts";

export default function App() {
	const [ready, setReady] = useState(false);

	useEffect(() => void delay(100).then(() => setReady(true)));

	return (
		<>
			<GlobalStyle $ready={ready} />
			<Navigation />
		</>
	);
}
