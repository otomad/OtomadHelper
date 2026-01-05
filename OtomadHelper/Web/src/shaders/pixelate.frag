uniform vec2 size = vec2(10);
uniform int tile = 50;

vec2 pixelate(vec2 coord, vec2 size) {
	return (floor(coord / size) + 0.5) * size;
}

vec4 frag() {
	vec2 coord = textureCoordinate;
	vec2 percent100 = size * vec2(tile); // resolution
	if (resolution.x >= resolution.y)
		percent100.y *= resolution.y / resolution.x;
	else
		percent100.x *= resolution.x / resolution.y;
	coord *= percent100;
	coord = pixelate(coord, size);
	coord /= percent100;
	return texture(image, coord);
}
