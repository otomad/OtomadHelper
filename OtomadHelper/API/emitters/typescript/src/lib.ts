import { createTypeSpecLibrary } from "@typespec/compiler";

export const $lib = createTypeSpecLibrary({
	name: "typescript-emitter",
	diagnostics: {},
});

export const { reportDiagnostic, createDiagnostic } = $lib;
