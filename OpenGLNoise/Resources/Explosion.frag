#version 330 core
const float ambient = 0.1;
const vec3 lightVecNormalized = normalize(vec3(0.5, 0.5, 2.0));
const vec3 lightColor = vec3(0.9, 0.9, 0.7);
//in vec3 normal;
varying vec3 fragt_normal;

varying float vUv;
varying float noise;
uniform vec4 GlobalColor1;
uniform vec4 GlobalColor2;
void main() {
    // compose the colour using the UV coordinate
    // and modulate it with the noise like ambient occlusion
	float colorRange = clamp(vUv * ( 1. - 2. * noise ), 0, 1);
	//vec3 lightColor = GlobalColor2.xyz;

	float diffuse = clamp(dot(lightVecNormalized, normalize(fragt_normal)), 0.0, 0.9);
	vec4 lightColor = (ambient + diffuse )* (GlobalColor2 / 2 + 0.5);//clamp(, 1.0);
	//vec4 lightedResult = mix(GlobalColor1, GlobalColor2, colorRange); // vec4( colorRed, colorGreen, colorBlue, 1.0 );
	//float colorRed = clamp(colorRange*3, 0, 1);
	//float colorGreen = clamp(colorRange*3, 1, 2)-1;
	//float colorBlue = clamp(colorRange*3, 2, 3)-2;
	 
    gl_FragColor = mix(GlobalColor1, lightColor, colorRange); // vec4( colorRed, colorGreen, colorBlue, 1.0 );
}
