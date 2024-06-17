const vec2 size = vec2(10);

vec2 pixelate(vec2 coord, vec2 size) {
	return (floor(coord / size) + 0.5) * size;
}

vec4 frag() {
	vec2 coord = textureCoordinate;
	coord *= resolution;
	coord = pixelate(coord, size);
	coord /= resolution;
	return texture(image, coord);
}
