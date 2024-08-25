import { EmitContext, emitFile, resolvePath } from "@typespec/compiler";
import { CSharpInterfaceEmitter } from "./csharp-emitter.js";
import { AssetEmitter, createAssetEmitter, } from "@typespec/compiler/emitter-framework";
import type { EmitterOptions } from "./lib.js";

export async function $onEmit(context: EmitContext<EmitterOptions>) {
	class SingleFileEmitter extends CSharpInterfaceEmitter {
		programContext() {
			const outputFile = emitter.createSourceFile(context.options.outputDir);
			return { scope: outputFile.globalScope };
		}
	}
	const emitter: AssetEmitter<string> = createAssetEmitter(
		context.program,
		SingleFileEmitter,
		{
			emitterOutputDir: context.program.compilerOptions.outputDir!,
			options: {},
		} as never,
	);
	emitter.emitProgram();
	await emitter.writeOutput();
}
