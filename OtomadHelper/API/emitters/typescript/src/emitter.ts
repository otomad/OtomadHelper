import { EmitContext, emitFile, resolvePath } from "@typespec/compiler";
import { TypeScriptInterfaceEmitter } from "./emitter-framework.js";

export async function $onEmit(context: EmitContext) {
	/* if (context.program.compilerOptions.noEmit) return;
	await emitFile(context.program, {
		path: resolvePath(context.emitterOutputDir, "output.txt"),
		content: "Hello world\n",
	}); */
	const assetEmitter = context.getAssetEmitter(TypeScriptInterfaceEmitter);

	// emit my entire TypeSpec program
	assetEmitter.emitProgram();

	// lastly, write your emit output into the output directory
	await assetEmitter.writeOutput();
}
