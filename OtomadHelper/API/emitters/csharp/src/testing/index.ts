import { resolvePath } from "@typespec/compiler";
import { createTestLibrary, TypeSpecTestLibrary } from "@typespec/compiler/testing";
import { fileURLToPath } from "url";

export const TypescriptEmitterTestLibrary: TypeSpecTestLibrary = createTestLibrary({
	name: "csharp-emitter",
	packageRoot: resolvePath(fileURLToPath(import.meta.url), "../../../../"),
});
