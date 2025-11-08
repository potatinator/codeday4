shader_type canvas_item;
float random(vec2 uv) {
    return fract(sin(dot(uv.xy,
        vec2(12.9898,78.233))) *
            43758.5453123);
}
vec2 randomu(vec2 uv) {
	return vec2(fract(sin(dot(uv.xy,
		vec2(12.9898,78.233))) * 43758.5453123));
}

float noise(vec2 uv) {
    vec2 uv_index = floor(uv);
    vec2 uv_fract = fract(uv);

    // Four corners in 2D of a tile
    float a = random(uv_index);
    float b = random(uv_index + vec2(1.0, 0.0));
    float c = random(uv_index + vec2(0.0, 1.0));
    float d = random(uv_index + vec2(1.0, 1.0));

    vec2 blur = smoothstep(0.0, 1.0, uv_fract);

    return mix(a, b, blur.x) +
            (c - a) * blur.y * (1.0 - blur.x) +
            (d - b) * blur.x * blur.y;
}

float fbm(vec2 uv) {
    int octaves = 6;
    float amplitude = 0.5;
    float frequency = 3.0;
	float value = 0.0;
	
    for(int i = 0; i < octaves; i++) {
        value += amplitude * noise(frequency * uv);
        amplitude *= 0.5;
        frequency *= 2.0;
    }
    return value;
}

vec2 worley(vec2 uv, float columns, float rows) {
	
	vec2 index_uv = floor(vec2(uv.x * columns, uv.y * rows));
	vec2 fract_uv = fract(vec2(uv.x * columns, uv.y * rows));
	
	vec2 minimum_dist = vec2(1.0);  
	vec2 minimum_point;

	for (int y= -1; y <= 1; y++) {
		for (int x= -1; x <= 1; x++) {
			vec2 neighbor = vec2(float(x),float(y));
			vec2 point = randomu(index_uv + neighbor);

			vec2 diff = neighbor + point - fract_uv;
			
			if(length(diff) < length(minimum_dist)) {
				minimum_dist = diff;
				minimum_point = point;
			}
		}
	}
	return minimum_dist;
}
vec2 quant(vec2 a, float b){
	return vec2(float(int(a.x*b))/b, float(int(a.y*b))/b);
}

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	//float a = 5.0;
	//float c = mix(0.9, 1, fbm(UV*a));
	//c = clamp(c, 0, 1);
	COLOR = texture(TEXTURE,UV*vec2(350.0));
	//COLOR = mix(vec4(1), COLOR, c);
	
	//COLOR = vec4(worley(quant(UV, 6000), 150.0, 150.0), 0, 1);
	//COLOR = vec4(quant(UV, 10), 0, 1);
	//COLOR = texture(TEXTURE, worley(quant(UV, 6000), 150.0, 150.0)*vec2(4.0));
	COLOR = COLOR/vec4(vec3(2.5), 1);
}


//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
