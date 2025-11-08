shader_type canvas_item;

uniform float illum;
uniform float sat;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	COLOR = vec4(mix(texture(TEXTURE, UV).rgb, vec3(1), -sat)*illum, texture(TEXTURE, UV).a);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
