import { createTypeSpecLibrary, type JSONSchemaType } from "@typespec/compiler";

export interface EmitterOptions {
	outputDir: string;
}

const EmitterOptionsSchema: JSONSchemaType<EmitterOptions> = {
	type: "object",
	additionalProperties: false,
	properties: {
		outputDir: { type: "string", default: "typescript-emitter/output.ts" },
	},
	required: [],
};

export const $lib = createTypeSpecLibrary({
	name: "typescript-emitter",
	diagnostics: {},
	emitter: {
		options: EmitterOptionsSchema,
	},
});

export const { reportDiagnostic, createDiagnostic } = $lib;
