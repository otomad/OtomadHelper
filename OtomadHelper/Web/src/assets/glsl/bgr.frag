vec4 frag() {
	return texture(image, textureCoordinate).bgra;
}
