import { Diagnostic, resolvePath } from "@typespec/compiler";
import {
	createTestHost,
	createTestWrapper,
	expectDiagnosticEmpty,
} from "@typespec/compiler/testing";
import { TypescriptEmitterTestLibrary } from "../src/testing/index.js";

export async function createTypescriptEmitterTestHost() {
	return createTestHost({
		libraries: [TypescriptEmitterTestLibrary],
	});
}

export async function createTypescriptEmitterTestRunner() {
	const host = await createTypescriptEmitterTestHost();

	return createTestWrapper(host, {
		compilerOptions: {
			noEmit: false,
			emit: ["csharp-emitter"],
		},
	});
}

export async function emitWithDiagnostics(
	code: string
): Promise<[Record<string, string>, readonly Diagnostic[]]> {
	const runner = await createTypescriptEmitterTestRunner();
	await runner.compileAndDiagnose(code, {
		outputDir: "tsp-output",
	});
	const emitterOutputDir = "./tsp-output/csharp-emitter";
	const files = await runner.program.host.readDir(emitterOutputDir);

	const result: Record<string, string> = {};
	for (const file of files) {
		result[file] = (await runner.program.host.readFile(resolvePath(emitterOutputDir, file))).text;
	}
	return [result, runner.program.diagnostics];
}

export async function emit(code: string): Promise<Record<string, string>> {
	const [result, diagnostics] = await emitWithDiagnostics(code);
	expectDiagnosticEmpty(diagnostics);
	return result;
}
