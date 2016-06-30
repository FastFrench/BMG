#version 330 core
const float ambient = 0.2;
const int LIGHTCOUNT = 3;

struct LightInfo
{
	vec3 Position;		//Light Position in eye-coords
	vec3 La;			//Ambient light intensity
	vec3 Ld;			//Diffuse light intensity
	vec3 Ls;			//Specular light intensity
};
uniform LightInfo Light[LIGHTCOUNT];

//const vec3 diffuseLightVecNormalized = normalize(vec3(0.5, 0.5, 2.0));
const vec3 specularLightVecNormalized = normalize(vec3(3, 0.5, 0.0));
const vec3 specularLightVecNormalized2 = normalize(vec3(-3, -5.5, -4.0));
const vec3 specularLightVecNormalized3 = normalize(vec3(0, 3.5, -12.0));
const vec4 lightColor = vec4(0.9, 0.9, 0.7, 1);
const vec4 lightColor2 = vec4(0, 1, 0, 1);
const vec4 lightColor3 = vec4(1, 0, 0, 1);
const float shininess = 20;
//in vec3 normal;
varying vec3 fragt_normal;
varying vec3 eye;
out vec4 PixelColor;

uniform vec4 GlobalColor1;
uniform vec4 GlobalColor2;
void main() {
    // compose the colour using the UV coordinate
    // and modulate it with the noise like ambient occlusion
	//float colorRange = clamp(vUv * ( 1. - 2. * noise ), 0, (1-ambient));
	//vec3 lightColor = GlobalColor2.xyz;
	// We could normalize fragt_normal, but doesn't seems to have visual impact
	vec3 n = normalize(fragt_normal);
    vec3 e = normalize(eye);

	float diffuse = clamp(dot(specularLightVecNormalized, n), 0.0, 1) * 0.4;

	// compute the half vector
    vec3 h = normalize(specularLightVecNormalized + e);  
    // compute the specular term into spec
    float intSpec = max(dot(h,n), 0.0);
    float spec = pow(intSpec,shininess) * 1.0;

	float diffuse2 = clamp(dot(specularLightVecNormalized2, n), 0.0, 1) * 0.4;
	vec3 h2 = normalize(specularLightVecNormalized2 + e);  
    // compute the specular term into spec
    float intSpec2 = max(dot(h2,n), 0.0);
    float spec2 = pow(intSpec2,shininess);

	float diffuse3 = clamp(dot(specularLightVecNormalized3, n), 0.0, 1) * 0.4;
	vec3 h3 = normalize(specularLightVecNormalized3 + e);  
    // compute the specular term into spec
    float intSpec3 = max(dot(h3,n), 0.0);
    float spec3 = pow(intSpec3,shininess);

	//vec4 lightColor = (ambient + diffuse )* (GlobalColor2 / 2 + 0.5);//clamp(, 1.0);
	//vec4 lightedResult = mix(GlobalColor1, GlobalColor2, colorRange); // vec4( colorRed, colorGreen, colorBlue, 1.0 );
	//float colorRed = clamp(colorRange*3, 0, 1);
	//float colorGreen = clamp(colorRange*3, 1, 2)-1;
	//float colorBlue = clamp(colorRange*3, 2, 3)-2;
	 
    PixelColor = max(GlobalColor1 * ambient, lightColor * diffuse + lightColor2 * diffuse2 + lightColor3 * diffuse3 + lightColor * spec + lightColor2 * spec2 + lightColor3 * spec3) ;//) //mix(GlobalColor1 * 0.1, GlobalColor2, diffuse);
}
