#version 430 core
precision highp float;
const int LIGHTCOUNT = 3;

//layout (std140, binding=0) uniform  
struct LightInfo
{
	//float unused1;
	vec4 La;			//Ambient light intensity
	//float unused2;
	vec4 Ld;			//Diffuse light intensity
	//float unused3;
	vec4 Ls;			//Specular light intensity
	vec3 Position;		//Light Position in eye-coords
	float Visible;	
};// Light[LIGHTCOUNT];

layout (std140, binding=0) uniform Lights
{
	LightInfo Light[LIGHTCOUNT];
};

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

const vec3 EyePosition = vec3(7,0,0);
const float shininess = 20;
const float screenGamma = 2.2; // Assume the monitor is calibrated to the sRGB color space

out vec4 FragColor;

uniform vec4 GlobalColor1;
uniform vec4 GlobalColor2;
uniform float Gamma;

void main() {

	vec3 ambientSum = vec3(0);
	vec3 diffuseSum = vec3(0);
	vec3 specSum = vec3(0);
	vec3 ambient, diffuse, spec;
    int nbVis = 0;
	if ( gl_FrontFacing )
	{
		for( int i=0; i<LIGHTCOUNT; ++i )
		if (Light[i].Visible > 0)
		{
			nbVis++;
			light( i, Data.Position, Data.Normal, ambient, diffuse, spec );
			ambientSum += ambient;
			diffuseSum += diffuse;
			specSum += spec;
		}
	}
	else
	{
		for( int i=0; i<LIGHTCOUNT; ++i )
		if (Light[i].Visible  > 0)
		{
			nbVis++;
			light( i, Data.Position, -Data.Normal, ambient, diffuse, spec );
			ambientSum += ambient;
			diffuseSum += diffuse;
			specSum += spec;
		}
	}
	if (nbVis>0)
		ambientSum /= nbVis++;
 
	vec4 texColor = GlobalColor1;//texture(Tex, data.TexCoord);
	vec4 colorLinear = vec4( ambientSum + diffuseSum, 1 ) * texColor + vec4( specSum, 1 );  
	vec4 colorGammaCorrected = pow(colorLinear, vec4(1.0/Gamma));

	FragColor = colorGammaCorrected;
}

