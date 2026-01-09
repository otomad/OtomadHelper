vec4 frag() {
	vec4 color = texture(image, textureCoordinate);
	float column = getSquareGradientColumn();
	const vec4 averageLuminance = vec4(0.5, 0.5, 0.5, 1.0);
	return mix(averageLuminance, color, column * 3.0);
}
