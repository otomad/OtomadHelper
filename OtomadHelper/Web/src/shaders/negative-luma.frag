uniform float progress = 0.5;

vec4 frag() {
	vec4 positive = texture(image, textureCoordinate);
	vec4 negative = vec4(1.0 - positive.r, 1.0 - positive.g, 1.0 - positive.b, positive.a);
	float positiveLightness = (positive.r + positive.g + positive.b) / 3.0;
	float negativeLightness = 1.0 - positiveLightness;
	return progress == 0.0 ? positive :
		progress > 0.0 ? positiveLightness > progress ? positive : negative :
		negativeLightness > -progress ? positive : negative;
}
