vec3 hueShift(vec3 color, float hueDeg) {
	float hue = hueDeg / 180.0 * 3.14159;
	const vec3 k = vec3(0.57735, 0.57735, 0.57735);
	float cosAngle = cos(hue);
	return vec3(color * cosAngle + cross(k, color) * sin(hue) + k * dot(k, color) * (1.0 - cosAngle));
}

vec4 frag() {
	vec4 color = texture(image, textureCoordinate);

	float column = floor(textureCoordinate.x * resolution.x);
	if (resolution.x > resolution.y) {
		float offcut = (resolution.x - resolution.y) / 2.0;
		column = map(column, offcut, offcut + resolution.y, 0.0, resolution.x);
	}
	float rotation = column / resolution.x * 360.0;

	vec3 rotatedColor = hueShift(color.rgb, rotation);

	return vec4(rotatedColor, color.a);
}
