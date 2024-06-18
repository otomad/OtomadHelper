uniform vec2 center = vec2(0.5);
// float radius = 100.0;
uniform float strength = 1.0;

vec4 frag() {
	vec2 coord = textureCoordinate * resolution;
	coord -= center * resolution;
	float distance = length(coord);

	float radius = min(resolution.x, resolution.y) / 2.0;

	if (distance < radius) {
		float percent = distance / radius;
		if (strength > 0.0)
			coord *= mix(1.0, smoothstep(0.0, radius / distance, percent), strength * 0.75);
		else
			coord *= mix(1.0, pow(percent, 1.0 + strength * 0.75) * radius / distance, 1.0 - percent);
	}

	coord += center * resolution;
	coord /= resolution;
	vec2 clampedCoord = coord;
	vec4 color = texture(image, coord);

	return color;
}
