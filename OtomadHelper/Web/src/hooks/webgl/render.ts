import fragmentShaderMainSource from "assets/glsl/main.frag";
import vertexShaderSource from "assets/glsl/main.vert";
import fragmentFilter from "assets/glsl/mosaic.frag";
import * as webglUtils from "./utils";

type UniformType = ValueOf<{
	[key in keyof WebGL2RenderingContext]: key extends `uniform${infer type}` ? type : never;
}>;

class WebGLFilter {
	constructor(
		public readonly gl: WebGL2RenderingContext,
		public readonly program: WebGLProgram | null,
		public readonly canvas: HTMLCanvasElement,
	) { }

	uniform(type: UniformType, name: string, v1: unknown, v2?: unknown, v3?: unknown, v4?: unknown) {
		if (!this.program) throw new Error("program not ready");
		const { gl } = this;
		const uniformLocation = gl.getUniformLocation(this.program, name);
		if (uniformLocation)
			(gl as AnyObject)["uniform" + type](uniformLocation, v1, v2, v3, v4);
		return this;
	}

	apply() {
		if (!this.program) throw new Error("program not ready");
		const { gl, canvas } = this;
		gl.viewport(0, 0, canvas.width, canvas.height); // Tell WebGL how to convert from clip space to pixels.
		gl.clearColor(0, 0, 0, 0); // Clear the canvas.
		gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
		gl.drawArrays( // Draw the rectangle.
			gl.TRIANGLES, // primitiveType
			0, // offset
			6, // count
		);
		return this;
	}

	private isImageInitialized: boolean = false;
	changeImage(image: HTMLImageElement, resize: boolean = true) {
		const { gl, canvas } = this;
		const requiredResize = resize || this.isImageInitialized;
		if (requiredResize) {
			canvas.width = image.width;
			canvas.height = image.height;
		}
		gl.texImage2D( // Upload the image into the texture.
			gl.TEXTURE_2D,
			0, // mipLevel: The largest mip
			gl.RGBA, // internalFormat: Format we want in the texture
			gl.RGBA, // srcFormat: Format of data we are supplying
			gl.UNSIGNED_BYTE, // srcType: Type of data we are supplying
			image,
		);
		if (requiredResize) {
			setRectangle(gl, 0, 0, image.width, image.height); // Set a rectangle the same size as the image.
			this.uniform("2f", "resolution", canvas.width, canvas.height); // Pass in the canvas resolution so we can convert from pixels to clipSpace in the shader.
		}
		this.isImageInitialized = true;
		this.apply();
	}
}

function initWebgl2(/* image: HTMLImageElement, fragmentFilter: string, uniforms: Record<string, number> = {}, canvas?: HTMLCanvasElement */) {
	// Get A WebGL 2 context.
	const canvas = document.createElement("canvas");
	const gl = canvas.getContext("webgl2");
	if (!gl) return null!;
	// Merge the fragmentShaderSource. Split the two, to ensure that the macro definition (#) wraps correctly after the minified code.
	const fragmentShaderSource = fragmentShaderMainSource + "\n" + fragmentFilter;

	// Setup GLSL program.
	const program = webglUtils.createProgramFromSources(gl, [vertexShaderSource, fragmentShaderSource])!;

	// Look up where the vertex data needs to go.
	const positionAttributeLocation = gl.getAttribLocation(program, "positionAttribute");
	const textureCoordinateAttributeLocation = gl.getAttribLocation(program, "textureCoordinateAttribute");

	const vao = gl.createVertexArray(); // Create a vertex array object (attribute state).
	gl.bindVertexArray(vao); // And make it the one we're currently working with.

	const positionBuffer = gl.createBuffer(); // Create a buffer and put a single pixel space rectangle in it (2 triangles).
	gl.enableVertexAttribArray(positionAttributeLocation); // Turn on the attribute.
	gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer); // Bind it to ARRAY_BUFFER (think of it as ARRAY_BUFFER = positionBuffer).

	const filter = new WebGLFilter(gl, program, canvas);

	const attribPointer = [
		2, // size: 2 components per iteration.
		gl.FLOAT, // type: the data is 32bit floats.
		false, // normalize: don't normalize the data.
		0, // stride: 0 = move forward size * sizeof(type) each iteration to get the next position.
		0, // offset: start at the beginning of the buffer.
	] as const;

	gl.vertexAttribPointer(positionAttributeLocation, ...attribPointer); // Tell the attribute how to get data out of positionBuffer (ARRAY_BUFFER).

	const textureCoordinateBuffer = gl.createBuffer(); // Provide texture coordinates for the rectangle.
	gl.bindBuffer(gl.ARRAY_BUFFER, textureCoordinateBuffer);
	gl.bufferData(gl.ARRAY_BUFFER, new Float32Array([
		0.0, 0.0,
		1.0, 0.0,
		0.0, 1.0,
		0.0, 1.0,
		1.0, 0.0,
		1.0, 1.0,
	]), gl.STATIC_DRAW);

	gl.enableVertexAttribArray(textureCoordinateAttributeLocation); // Turn on the attribute.
	gl.vertexAttribPointer(textureCoordinateAttributeLocation, ...attribPointer); // Tell the attribute how to get data out of textureCoordinateBuffer (ARRAY_BUFFER).

	const texture = gl.createTexture(); // Create a texture.
	gl.activeTexture(gl.TEXTURE0); // Make unit 0 the active texture uint (ie, the unit all other texture commands will affect).
	gl.bindTexture(gl.TEXTURE_2D, texture); // Bind it to texture unit 0' 2D bind point.

	// Set the parameters so we don't need mips and so we're not filtering and we don't repeat.
	gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
	gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
	gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.NEAREST);
	gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.NEAREST);

	gl.useProgram(program); // Tell it to use our program (pair of shaders).
	gl.bindVertexArray(vao); // Bind the attribute/buffer set we want.

	filter.uniform("1i", "image", 0); // Tell the shader to get the texture from texture unit 0 .

	// Bind the position buffer so gl.bufferData that will be called in setRectangle puts data in the position buffer.
	gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer);

	return filter;
}

function setRectangle(gl: WebGL2RenderingContext, x: number, y: number, width: number, height: number) {
	const x1 = x;
	const x2 = x + width;
	const y1 = y;
	const y2 = y + height;
	gl.bufferData(gl.ARRAY_BUFFER, new Float32Array([
		x1, y1,
		x2, y1,
		x1, y2,
		x1, y2,
		x2, y1,
		x2, y2,
	]), gl.STATIC_DRAW);
}

const filter = initWebgl2();
export default filter;
