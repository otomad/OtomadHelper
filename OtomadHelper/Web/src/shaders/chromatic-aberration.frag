uniform float scale = 1.2;
uniform float horizontalIntensity = 0.5;
uniform float verticalIntensity = 0.5;

vec4 frag() {
	vec2 newTextureCoordinate = vec2((scale - 1.0) * 0.5 + textureCoordinate.x / scale, (scale - 1.0) * 0.5 + textureCoordinate.y / scale);
	vec4 textureColor = texture(image, newTextureCoordinate);

	// shift color
	vec4 shiftColor1 = texture(image, newTextureCoordinate + vec2(-0.05 * (scale - 1.0) * horizontalIntensity * 2.0, -0.05 * (scale - 1.0) * verticalIntensity * 2.0));
	vec4 shiftColor2 = texture(image, newTextureCoordinate + vec2(-0.1 * (scale - 1.0) * horizontalIntensity * 2.0, -0.1 * (scale - 1.0) * verticalIntensity * 2.0));

	// 3d blend color
	vec3 blendFirstColor = vec3(textureColor.r, textureColor.g, shiftColor1.b);
	vec3 blend3DColor = vec3(shiftColor2.r, blendFirstColor.g, blendFirstColor.b);
	return vec4(blend3DColor, textureColor.a);
}
