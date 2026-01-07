import { enableSvsQuotes } from "fullwidth-quotes";
import type { PostProcessorModule } from "i18next";

export const fullwidthQuotesProcessor: PostProcessorModule = {
	type: "postProcessor",
	name: "fullwidth-quotes",
	process: (value: string) => enableSvsQuotes(value),
};

export const addWbrAfterSlashProcessor: PostProcessorModule = {
	type: "postProcessor",
	name: "add-wbr-after-slash",
	process: (value: string) => value.replaceAll(/\/+/g, "$&\u200b"),
};
