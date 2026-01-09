vec4 frag() {
	vec4 color = texture(image, textureCoordinate);
	float column = getSquareGradientColumn();
	vec4 averageLuminance = vec4(column, column, column, 1.0);
	return mix(averageLuminance, color, 10.0);
}
