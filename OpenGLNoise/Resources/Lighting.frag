#version 430 core
const float ambient = 0.2;
const int LIGHTCOUNT = 3;

uniform LightInfo
{
	vec4 Position;		//Light Position in eye-coords
	//float unused1;
	vec4 La;			//Ambient light intensity
	//float unused2;
	vec4 Ld;			//Diffuse light intensity
	//float unused3;
	vec3 Ls;			//Specular light intensity
	bool Visible;	
} Light[LIGHTCOUNT];

in struct DataStruct 
{
	vec3 Normal;
	vec3 Position;
	vec3 Eye;
} Data;

const vec3 Material_Ka = vec3(1., 1., 1.);
const vec3 Material_Ks = vec3(1., 1., 1.);
const vec3 Material_Kd = vec3(1., 1., 1.);
const float Material_Shininess = 20.0;
void light( int lightIndex, vec3 position, vec3 norm, out vec3 ambient, out vec3 diffuse, out vec3 spec )
{
	vec3 n = normalize( norm );
	vec3 s = normalize( vec3(Light[lightIndex].Position) - position );
	vec3 v = normalize( -position );
	vec3 r = reflect( -s, n );
 
	ambient = vec3(Light[lightIndex].La) * Material_Ka;
 
	float sDotN = max( dot( s, n ), 0.0 );
	diffuse = vec3(Light[lightIndex].Ld) * Material_Kd * sDotN;
 
	spec = vec3(Light[lightIndex].Ls) * Material_Ks * pow( max( dot(r,v) , 0.0 ), Material_Shininess ); 
}
const int mode = 2;

//const vec3 diffuseLightVecNormalized = normalize(vec3(0.5, 0.5, 2.0));
const vec3 specularLightVecNormalized = normalize(vec3(3, 0.5, 0.0));
const vec3 specularLightVecNormalized2 = normalize(vec3(-3, -5.5, -4.0));
const vec3 specularLightVecNormalized3 = normalize(vec3(0, 3.5, -12.0));
const vec4 lightColor = vec4(0.9, 0.9, 0.7, 1);
const vec4 lightColor2 = vec4(0, 1, 0, 1);
const vec4 lightColor3 = vec4(1, 0, 0, 1);
const vec3 EyePosition = vec3(7,0,0);
const float shininess = 20;
const float screenGamma = 2.2; // Assume the monitor is calibrated to the sRGB color space
//in vec3 normal;
//varying vec3 fragt_normal;
//varying vec3 eye;
out vec4 PixelColor;

uniform vec4 GlobalColor1;
uniform vec4 GlobalColor2;

void main() {

  vec3 normal = normalize(Data.Normal);
  vec3 lightDir = normalize(vec3(Light[0].Position) - Data.Position);

  float lambertian = max(dot(lightDir,normal), 0.0);
  float specular = 0.0;

  if(lambertian > 0.0) {

    vec3 viewDir = normalize(-Data.Position);

    // this is blinn phong
    vec3 halfDir = normalize(lightDir + viewDir);
    float specAngle = max(dot(halfDir, normal), 0.0);
    specular = pow(specAngle, Material_Shininess);
       
    // this is phong (for comparison)
    if(mode == 2) {
      vec3 reflectDir = reflect(-lightDir, normal);
      specAngle = max(dot(reflectDir, viewDir), 0.0);
      // note that the exponent is different here
      specular = pow(specAngle, Material_Shininess/4.0);
    }
  }
  vec3 colorLinear = vec3(Light[0].La) +
                     lambertian * vec3(Light[0].Ld) +
                     specular * vec3(Light[0].Ls);
  // apply gamma correction (assume ambientColor, diffuseColor and specColor
  // have been linearized, i.e. have no gamma correction in them)
  vec3 colorGammaCorrected = colorLinear;//pow(colorLinear, vec3(1.0/screenGamma));
  // use the gamma corrected color in the fragment
  gl_FragColor = /*vec4(vec3(Light[0].La),1.0);*/vec4(colorGammaCorrected, 1.0);
}

