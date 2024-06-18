/* import { ClickToComponent } from "click-to-react-component";
import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import "./initial";
import "./locales/config";

ReactDOM.createRoot(document.getElementById("root")!).render(
	<React.StrictMode>
		<App />
		<ClickToComponent />
	</React.StrictMode>,
); */

import imagePath0 from "assets/images/ヨハネの氷.jpg";
import imagePath1 from "assets/images/ヨハネの氷.png";
import "./initial";

// import filter from "hooks/webgl/render";
import fragments, { defaults, fragNames } from "virtual:fragment-filters";
console.log(fragments);
console.log(fragNames);
console.log(defaults);

/* const image0 = await createImageFromUrl(imagePath0);
const image1 = await createImageFromUrl(imagePath1);

document.body.append(filter.canvas);

let on = false;
setInterval(() => {
	on = !on;
	filter.changeImage(on ? image0 : image1);
}, 2000); */
