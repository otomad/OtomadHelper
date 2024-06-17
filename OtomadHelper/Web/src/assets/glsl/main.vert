// GLSL ES 3.00 shader version.
#version 300 es

// an attribute is an input (in) to a vertex shader.
// It will receive data from a buffer.
in vec2 positionAttribute;
in vec2 textureCoordinateAttribute;

// Used to pass in the resolution of the canvas.
uniform vec2 resolution;

// Used to pass the texture coordinates to the fragment shader.
out vec2 textureCoordinate;

// All shaders have a main function.
void main() {
	// Convert the position from pixels to 0.0 → 1.0 .
	vec2 zeroToOne = positionAttribute / resolution;

	// Convert from 0 → 1 to 0 → 2 .
	vec2 zeroToTwo = zeroToOne * 2.0;

	// Convert from 0 → 2 to -1 → +1 (clipSpace).
	vec2 clipSpace = zeroToTwo - 1.0;

	gl_Position = vec4(clipSpace * vec2(1, -1), 0, 1);

	// Pass the textureCoordinate to the fragment shader.
	// The GPU will interpolate this value between points.
	textureCoordinate = textureCoordinateAttribute;
}
