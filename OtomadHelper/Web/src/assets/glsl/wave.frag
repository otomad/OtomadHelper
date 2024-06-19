uniform float frequency = 72.0;
uniform float amplitude = 48.0;
uniform float time = 0.0;

vec4 frag() {
	vec2 uv = textureCoordinate;
	uv.y += sin(uv.x * frequency + time) / amplitude;
	vec4 color = texture(image, uv);
	return color;
}
