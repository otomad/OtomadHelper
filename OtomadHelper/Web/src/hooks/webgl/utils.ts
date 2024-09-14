/*
 * For more about webgl-utils.js see:
 * http://webgl2fundamentals.org/webgl/lessons/webgl-boilerplate.html
 * @see https://webgl2fundamentals.org/webgl/resources/webgl-utils.js
 */

const errorRegexp = /ERROR:\s*\d+:(\d+)/gi;
function addLineNumbersWithError(src: string, log = "") {
	// Note: Error message formats are not defined by any spec so this may or may not work.
	const matches = [...log.matchAll(errorRegexp)];
	const lineNoToErrorMap = new Map<number, string>(matches.map((match, index) => {
		const lineNo = parseInt(match[1], 10);
		const next = matches[index + 1];
		const end = next ? next.index : log.length;
		const msg = log.substring(match.index, end);
		return [lineNo - 1, msg] as const;
	}));
	return src.split("\n").map((line, lineNo) => {
		const err = lineNoToErrorMap.get(lineNo);
		return `${lineNo + 1}: ${line}${err ? `\n\n^^^ ${err}` : ""}`;
	}).join("\n");
}

/**
 * Error Callback
 * @callback ErrorCallback
 * @param {string} msg error message.
 * @memberOf module:webgl-utils
 */
interface ErrorCallback {
	(msg?: string): void;
}
const error: ErrorCallback = console.error.bind(console);

/**
 * Loads a shader.
 * @param {WebGLRenderingContext} gl The WebGLRenderingContext to use.
 * @param {string} shaderSource The shader source.
 * @param {number} shaderType The type of shader.
 * @param {ErrorCallback} errorCallback callback for errors.
 * @return {WebGLShader} The created shader.
 */
function loadShader(gl: WebGLRenderingContext, shaderSource: string, shaderType: number, errorCallback = error): WebGLShader | null {
	// Create the shader object
	const shader = gl.createShader(shaderType)!;

	// Load the shader source
	gl.shaderSource(shader, shaderSource);

	// Compile the shader
	gl.compileShader(shader);

	// Check the compile status
	const compiled = gl.getShaderParameter(shader, gl.COMPILE_STATUS);
	if (!compiled) {
		// Something went wrong during compilation; get the error
		const lastError = gl.getShaderInfoLog(shader) ?? undefined;
		errorCallback(`Error compiling shader: ${lastError}\n${addLineNumbersWithError(shaderSource, lastError)}`);
		gl.deleteShader(shader);
		return null;
	}

	return shader;
}

/**
 * Creates a program, attaches shaders, binds attrib locations, links the
 * program and calls useProgram.
 * @param {WebGLShader[]} shaders The shaders to attach
 * @param {string[]} [attribs] An array of attribs names. Locations will be assigned by index if not passed in
 * @param {number[]} [locations] The locations for the. A parallel array to attribs letting you assign locations.
 * @param {ErrorCallback} errorCallback callback for errors. By default it just prints an error to the console
 * on error. If you want something else pass an callback. It's passed an error message.
 * @memberOf module:webgl-utils
 */
export function createProgram(gl: WebGLRenderingContext, shaders: WebGLShader[], attribs?: string[], locations?: number[], errorCallback = error) {
	const program = gl.createProgram()!;
	shaders.forEach(function (shader) {
		gl.attachShader(program, shader);
	});
	if (attribs)
		attribs.forEach(function (attrib, index) {
			gl.bindAttribLocation(
				program,
				locations ? locations[index] : index,
				attrib,
			);
		});

	gl.linkProgram(program);

	// Check the link status
	const linked = gl.getProgramParameter(program, gl.LINK_STATUS);
	if (!linked) {
		// something went wrong with the link
		const lastError = gl.getProgramInfoLog(program);
		errorCallback(`Error in program linking: ${lastError}\n${shaders.map(shader => {
			const src = addLineNumbersWithError(gl.getShaderSource(shader) ?? "");
			const type = gl.getShaderParameter(shader, gl.SHADER_TYPE);
			return `${type}:\n${src}`;
		}).join("\n")}`);

		gl.deleteProgram(program);
		return null;
	}
	return program;
}

/**
 * Loads a shader from a script tag.
 * @param {WebGLRenderingContext} gl The WebGLRenderingContext to use.
 * @param {string} scriptId The id of the script tag.
 * @param {number} shaderType The type of shader. If not passed in it will be derived from the type of the script tag.
 * @param {ErrorCallback} errorCallback callback for errors.
 * @return {WebGLShader} The created shader.
 */
function createShaderFromScript(gl: WebGLRenderingContext, scriptId: string, shaderType?: number, errorCallback?: ErrorCallback): WebGLShader | null {
	let shaderSource = "";
	const shaderScript = document.getElementById(scriptId) as HTMLScriptElement | null;
	if (!shaderScript)
		throw "*** Error: unknown script element: " + scriptId;

	shaderSource = shaderScript.text;

	if (!shaderType)
		if (shaderScript.type === "x-shader/x-vertex")
			shaderType = gl.VERTEX_SHADER;
		else if (shaderScript.type === "x-shader/x-fragment")
			shaderType = gl.FRAGMENT_SHADER;
	if (shaderType !== gl.VERTEX_SHADER && shaderType !== gl.FRAGMENT_SHADER)
		throw "*** Error: unknown shader type";

	return loadShader(gl, shaderSource, shaderType, errorCallback);
}

const defaultShaderType = [
	"VERTEX_SHADER",
	"FRAGMENT_SHADER",
] as const;

/**
 * Creates a program from 2 script tags.
 *
 * @param {WebGLRenderingContext} gl The WebGLRenderingContext to use.
 * @param {string[]} shaderScriptIds Array of ids of the script tags for the shaders. The first is assumed to be the
 * vertex shader, the second the fragment shader.
 * @param {string[]} [attribs] An array of attribs names. Locations will be assigned by index if not passed in
 * @param {number[]} [locations] The locations for the. A parallel array to attribs letting you assign locations.
 * @param {ErrorCallback} errorCallback callback for errors. By default it just prints an error to the console
 * on error. If you want something else pass an callback. It's passed an error message.
 * @return {WebGLProgram} The created program.
 * @memberOf module:webgl-utils
 */
export function createProgramFromScripts(gl: WebGLRenderingContext, shaderScriptIds: string[], attribs?: string[], locations?: number[], errorCallback?: ErrorCallback): WebGLProgram | null {
	const shaders = [];
	for (let ii = 0; ii < shaderScriptIds.length; ++ii)
		shaders.push(createShaderFromScript(gl, shaderScriptIds[ii], gl[defaultShaderType[ii]], errorCallback)!);

	return createProgram(gl, shaders, attribs, locations, errorCallback);
}

/**
 * Creates a program from 2 sources.
 *
 * @param {WebGLRenderingContext} gl The WebGLRenderingContext to use.
 * @param {string[]} shaderSources Array of sources for the shaders. The first is assumed to be the vertex shader,
 * the second the fragment shader.
 * @param {string[]} [attribs] An array of attribs names. Locations will be assigned by index if not passed in
 * @param {number[]} [locations] The locations for the. A parallel array to attribs letting you assign locations.
 * @param {ErrorCallback} errorCallback callback for errors. By default it just prints an error to the console
 * on error. If you want something else pass an callback. It's passed an error message.
 * @return {WebGLProgram} The created program.
 * @memberOf module:webgl-utils
 */
export function createProgramFromSources(gl: WebGLRenderingContext, shaderSources: string[], attribs?: string[], locations?: number[], errorCallback?: ErrorCallback): WebGLProgram | null {
	const shaders = [];
	for (let ii = 0; ii < shaderSources.length; ++ii)
		shaders.push(loadShader(gl, shaderSources[ii], gl[defaultShaderType[ii]], errorCallback)!);

	return createProgram(gl, shaders, attribs, locations, errorCallback);
}

/**
 * Resize a canvas to match the size its displayed.
 * @param {HTMLCanvasElement} canvas The canvas to resize.
 * @param {number} [multiplier] amount to multiply by.
 * Pass in window.devicePixelRatio for native pixels.
 * @return {boolean} true if the canvas was resized.
 * @memberOf module:webgl-utils
 */
export function resizeCanvasToDisplaySize(canvas: HTMLCanvasElement, multiplier: number = 1): boolean {
	const width = canvas.clientWidth * multiplier | 0;
	const height = canvas.clientHeight * multiplier | 0;
	if (canvas.width !== width || canvas.height !== height) {
		canvas.width = width;
		canvas.height = height;
		return true;
	}
	return false;
}
