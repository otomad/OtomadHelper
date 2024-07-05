import { resolvePath } from "@typespec/compiler";
import { createTestLibrary, TypeSpecTestLibrary } from "@typespec/compiler/testing";
import { fileURLToPath } from "url";

export const TypescriptEmitterTestLibrary: TypeSpecTestLibrary = createTestLibrary({
	name: "typescript-emitter",
	packageRoot: resolvePath(fileURLToPath(import.meta.url), "../../../../"),
});
