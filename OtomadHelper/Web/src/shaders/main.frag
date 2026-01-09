// GLSL ES 3.00 shader version.
#version 300 es

// Fragment shaders don't have a default precision so we need
// to pick one. highp is a good default. It means "high precision".
precision highp float;

// Our texture.
uniform sampler2D image;

// Used to pass in the resolution of the canvas.
uniform vec2 resolution;

// Select the correct fragment filter.
uniform int fragIndex;

// The textureCoordinates passed in from the vertex shader.
in vec2 textureCoordinate;

// We need to declare an output for the fragment shader.
out vec4 outColor;

// Declare the frag() function which would be implemented.
vec4 frag();

// Utils

float map(float x, float min, float max, float a, float b) {
	return (b - a) * (x - min) / (max - min) + a;
}

/**
 * Crop the square area in the center of the image and overlay the filter in a linear gradient from left to right
 * according to the pixels in each column. Get column progress value (0 ~ 1).
 */
float getSquareGradientColumn() {
	float column = floor(textureCoordinate.x * resolution.x);
	if (resolution.x > resolution.y) {
		float offcut = (resolution.x - resolution.y) / 2.0;
		column = map(column, offcut, offcut + resolution.y, 0.0, resolution.x);
	}
	return column / resolution.x;
}

$fragments

vec4 selectFrag(int index) {
	$switch
}

// All fragments shared main function.
void main() {
	// Use frag() function to define your own filter.
	// outColor = frag();
	outColor = selectFrag(fragIndex);
}

/**
 * frag() function implement example:
 *
 * vec4 frag() {
 * 	// returns the new result out color.
 * 	// This example shows to exchange red and blue in the colors.
 * 	return texture(image, textureCoordinate).bgra;
 * }
 */
