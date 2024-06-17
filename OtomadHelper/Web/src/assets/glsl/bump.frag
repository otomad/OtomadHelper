const float strength = 8.0;

vec4 frag() {
	vec2 onePixel = vec2(1.0 / resolution);

	vec4 color = texture(image, textureCoordinate);

	color -= texture(image, textureCoordinate - onePixel) * strength;
	color += texture(image, textureCoordinate + onePixel) * strength;

	float alpha = texture(image, textureCoordinate).a;

	return vec4(color.rgb * alpha, alpha);
}
