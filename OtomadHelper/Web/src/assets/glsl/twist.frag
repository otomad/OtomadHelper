float radius = 200.0;
const float angle = -5.0;
const vec2 offset = vec2(0.5);
const vec4 uInputSize = vec4(720.0,404.0,1.0/720.0,1.0/404.0);

vec2 twisting(vec2 coord) {
	coord *= resolution;
	coord -= offset * resolution;

	float dist = length(coord);

	if (dist < radius) {
		float ratioDist = (radius - dist) / radius;
		float angleMod = ratioDist * ratioDist * angle;
		float s = sin(angleMod);
		float c = cos(angleMod);
		coord = vec2(coord.x * c - coord.y * s, coord.x * s + coord.y * c);
	}

	coord += offset * resolution;
	coord /= resolution;

	return coord;
}

vec4 frag() {
	radius = min(resolution.x, resolution.y) / 2.0;
	vec2 coord = twisting(textureCoordinate);
	return texture(image, coord);
}
