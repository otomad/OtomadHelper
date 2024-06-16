#version 300 es

// fragment shaders don't have a default precision so we need
// to pick one. highp is a good default. It means "high precision"
precision highp float;

// our texture
uniform sampler2D u_image;

// the textureCoordinates passed in from the vertex shader.
in vec2 v_textureCoordinate;

// we need to declare an output for the fragment shader
out vec4 outColor;

void main() {
	outColor = texture(u_image, v_textureCoordinate).bgra;
}
