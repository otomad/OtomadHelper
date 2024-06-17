const float frequency = 72.0;
const float amplitude = 48.0;
const float time = 0.0;

vec4 frag() {
	vec2 uv = textureCoordinate;
	uv.y += sin(uv.x * frequency + time) / amplitude;
	vec4 color = texture(image, uv);
	color.a = 1.0;
	return color;
}
