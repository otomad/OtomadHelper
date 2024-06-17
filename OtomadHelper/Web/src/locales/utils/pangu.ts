import type { PostProcessorModule } from "i18next";
import { spacing } from "pangu";

const panguProcessor: PostProcessorModule = {
	type: "postProcessor",
	name: "pangu",
	process: (value: string) => spacing(value),
};

export default panguProcessor;
