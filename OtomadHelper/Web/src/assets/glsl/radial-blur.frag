#version 300 es

precision highp float;

in vec2 v_textureCoordinate;
out vec4 outColor;
uniform sampler2D u_image;

const float strength = 0.15;
const vec2 center = vec2(0.5, 0.5);
const vec2 radius = vec2(0, -1);

const float MAX_KERNEL_SIZE = 32.0;

highp float rand(vec2 co, float seed) {
	const highp float a = 12.9898, b = 78.233, c = 43758.5453;
	highp float dt = dot(co + seed, vec2(a, b)), sn = mod(dt, 3.14159);
	return fract(sin(sn) * c + seed);
}

void main() {
	float minGradient = radius[0] * 0.3;
	float innerRadius = (radius[0] + minGradient * 0.5);

	float gradient = radius[1] * 0.3;
	float radius = (radius[1] - gradient * 0.5);

	float countLimit = MAX_KERNEL_SIZE;

	vec2 dir = vec2(center.xy - v_textureCoordinate);
	float dist = length(vec2(dir.x, dir.y));

	float strength = strength;

	float delta = 0.0;
	float gap;
	if (dist < innerRadius) {
		delta = innerRadius - dist;
		gap = minGradient;
	} else if (radius >= 0.0 && dist > radius) { // radius < 0 means it's infinity
		delta = dist - radius;
		gap = gradient;
	}

	if (delta > 0.0) {
		float normalCount = gap;
		delta = (normalCount - delta) / normalCount;
		countLimit *= delta;
		strength *= delta;
		if (countLimit < 1.0) {
			outColor = texture(u_image, v_textureCoordinate);
			return;
		}
	}

	// randomize the lookup values to hide the fixed number of samples
	float offset = rand(v_textureCoordinate, 0.0);

	float total = 0.0;
	vec4 color = vec4(0.0);

	dir *= strength;

	for (float t = 0.0; t < MAX_KERNEL_SIZE; t++) {
		float percent = (t + offset) / MAX_KERNEL_SIZE;
		float weight = 4.0 * (percent - percent * percent);
		vec2 p = v_textureCoordinate + dir * percent;
		vec4 sampleColor = texture(u_image, p);

		// switch to pre-multiplied alpha to correctly blur transparent images
		// sampleColor.rgb *= sampleColor.a;

		color += sampleColor * weight;
		total += weight;

		if (t > countLimit)
			break;
	}

	color /= total;
	// switch back from pre-multiplied alpha
	// color.rgb /= color.a + 0.00001;

	outColor = color;
}
