const float direction = 1.0;

vec4 hsvToRgb(vec4 hsva) {
	vec3 hsv = hsva.xyz;
	vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	vec3 p = abs(fract(hsv.xxx + K.xyz) * 6.0 - K.www);
	vec3 rgb = hsv.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), hsv.y);
	return vec4(rgb, hsva.a);
}

vec4 frag() {
	vec4 color = texture(image, textureCoordinate);
	float lightness = (color.r + color.g + color.b) / 3.0;
	return hsvToRgb(vec4(lightness * direction, 1, 1, color.a));
}
