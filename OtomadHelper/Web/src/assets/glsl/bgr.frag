#version 300 es

// fragment shaders don't have a default precision so we need
// to pick one. highp is a good default. It means "high precision"
precision highp float;

// our texture
uniform sampler2D image;

// the textureCoordinates passed in from the vertex shader.
in vec2 textureCoordinate;

// we need to declare an output for the fragment shader
out vec4 outColor;

void main() {
	outColor = texture(image, textureCoordinate).bgra;
}
