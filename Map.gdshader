shader_type canvas_item;

vec2 random(vec2 uv){
    uv = vec2( dot(uv, vec2(127.1,311.7) ),
               dot(uv, vec2(269.5,183.3) ) );
    return -1.0 + 2.0 * fract(sin(uv) * 43758.5453123);
}

float noise(vec2 uv) {
    vec2 uv_index = floor(uv);
    vec2 uv_fract = fract(uv);

    vec2 blur = smoothstep(0.0, 1.0, uv_fract);

    return mix( mix( dot( random(uv_index + vec2(0.0,0.0) ), uv_fract - vec2(0.0,0.0) ),
                     dot( random(uv_index + vec2(1.0,0.0) ), uv_fract - vec2(1.0,0.0) ), blur.x),
                mix( dot( random(uv_index + vec2(0.0,1.0) ), uv_fract - vec2(0.0,1.0) ),
                     dot( random(uv_index + vec2(1.0,1.0) ), uv_fract - vec2(1.0,1.0) ), blur.x), blur.y) + 0.5;
}


void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	float a = 15.0;
	float b = 5.0;
	float c = random(vec2(float(int((UV*vec2(a)).x)), float(int((UV*vec2(b)).y)))/vec2(a)).x;
	c = clamp(c, 0, 1);
	//if(c > 0.25){
		//COLOR = vec4(0, 0.5, 0, 1);
	//} else {
		//COLOR = vec4(0, 0.75, 0, 1);
	//}
	COLOR = vec4(0, float(int(mix(0.75, 0.6, c)*10.0))/10.0, 0, 1);
	COLOR = mix(COLOR, vec4(1), 0.6);
	COLOR = vec4(COLOR.rgb - vec3(0.75), 1);
}


//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
