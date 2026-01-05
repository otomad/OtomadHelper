uniform float strength = 5.0;
uniform float accuracy = 500.0; // Use 0.0 to match the image real resolution.

vec4 frag() {
	vec2 onePixel = vec2(1.0 / resolution);
	if (accuracy != 0.0) onePixel = vec2(1.0 / accuracy);

	vec4 color = texture(image, textureCoordinate);

	color -= texture(image, textureCoordinate - onePixel) * strength;
	color += texture(image, textureCoordinate + onePixel) * strength;

	float alpha = texture(image, textureCoordinate).a;

	return vec4(color.rgb * alpha, alpha);
}
