uniform float strength = 0.15;
uniform vec2 center = vec2(0.5);
uniform float innerRadius = 0.0;
uniform float radius = -1.0;

highp float rand(vec2 co, float seed) {
	const highp float a = 12.9898, b = 78.233, c = 43758.5453;
	highp float dt = dot(co + seed, vec2(a, b)), sn = mod(dt, 3.14159);
	return fract(sin(sn) * c + seed);
}

vec4 frag() {
	const float MAX_KERNEL_SIZE = 32.0;

	float minGradient = innerRadius * 0.3;
	float innerRadius = (innerRadius + minGradient * 0.5);

	float gradient = radius * 0.3;
	float radius = (radius - gradient * 0.5);

	float countLimit = MAX_KERNEL_SIZE;

	vec2 dir = vec2(center.xy - textureCoordinate);
	float dist = length(vec2(dir.x, dir.y * resolution.y / resolution.x));

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
		if (countLimit < 1.0)
			return texture(image, textureCoordinate);
	}

	// randomize the lookup values to hide the fixed number of samples
	float offset = rand(textureCoordinate, 0.0);

	float total = 0.0;
	vec4 color = vec4(0.0);

	dir *= strength;

	for (float t = 0.0; t < MAX_KERNEL_SIZE; t++) {
		float percent = (t + offset) / MAX_KERNEL_SIZE;
		float weight = 4.0 * (percent - percent * percent);
		vec2 p = textureCoordinate + dir * percent;
		vec4 sampleColor = texture(image, p);

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

	return color;
}
