const float strength = 2.0;

vec4 frag() {
	vec2 onePixel = vec2(1.0 / resolution);

	vec4 color;

	color.rgb = vec3(0.5);

	color -= texture(image, textureCoordinate - onePixel) * strength;
	color += texture(image, textureCoordinate + onePixel) * strength;

	float alpha = texture(image, textureCoordinate).a;

	return vec4(color.rgb * alpha, alpha);
}
