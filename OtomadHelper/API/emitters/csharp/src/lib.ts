import { createTypeSpecLibrary } from "@typespec/compiler";

export const $lib = createTypeSpecLibrary({
	name: "csharp-emitter",
	diagnostics: {},
});

export const { reportDiagnostic, createDiagnostic } = $lib;
