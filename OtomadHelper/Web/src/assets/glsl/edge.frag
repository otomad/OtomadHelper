uniform float strength = 1.0;
uniform float accuracy = 500.0; // Use 0.0 to match the image real resolution.

vec4 frag() {
	vec2 onePixel = vec2(1.0 / resolution);
	if (accuracy != 0.0) onePixel = vec2(1.0 / accuracy);
	vec2 onePixelX = vec2(onePixel.x, 0);
	vec2 onePixelY = vec2(0, onePixel.y);

	// vec4 center = texture(image, textureCoordinate);
	vec4 north = texture(image, textureCoordinate + onePixelY);
	vec4 east = texture(image, textureCoordinate + onePixelX);
	vec4 south = texture(image, textureCoordinate - onePixelY);
	vec4 west = texture(image, textureCoordinate - onePixelX);

	vec4 dy = (north - south) * 0.5;
	vec4 dx = (east - west) * 0.5;

	vec4 edge = sqrt(dx * dx + dy * dy);
	// vec4 angle = atan(dy, dx);
	vec3 color = edge.rgb * 5.0;

	// Below is another way to make an edge detector.
	// It is invariant under intensity-scaling of the image and linear illumination differences.
	// This should make it more "human-sight" like.

	// vec4 laplacian = north + east + south + west - 4.0 * center;
	// vec3 color = abs(laplacian.rgb / center.rgb) * 1.0;

	float alpha = texture(image, textureCoordinate).a;
	return vec4(color * alpha, alpha);
}
