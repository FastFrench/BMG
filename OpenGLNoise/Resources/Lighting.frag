#version 440 core
precision highp float;
const int LIGHTCOUNT = 3;

struct LightInfo
{
	vec3 La;			//Ambient light intensity
	vec3 Ld;			//Diffuse light intensity
	vec3 Ls;			//Specular light intensity
	vec3 Position;		//Light Position in eye-coords
	bool Visible;     
};

layout (std140) uniform Lights
{
	float		Gamma;
	int			NbLights;
	LightInfo	Light[LIGHTCOUNT];
} toto;

struct MaterialInfo
{
	vec3 Ka;
	vec3 Kd;
	vec3 Ks;
	float Shininess;
	bool Visible;
	//bool UsingNoise;
	//float Size;
};

uniform MaterialInfo Object;

in struct DataStruct 
{
	vec3 Normal;
	vec3 Position;
	vec3 Eye;
} Data;

void light( int lightIndex, vec3 position, vec3 norm, out vec3 ambient, out vec3 diffuse, out vec3 spec )
{
	vec3 n = normalize( -norm );
	vec3 s = normalize( position - vec3(toto.Light[lightIndex].Position));
	vec3 v = normalize( -position );
	vec3 r = reflect( -s, n );

	ambient = vec3(toto.Light[lightIndex].La) * Object.Ka;

	float sDotN = max( dot( s, n ), 0.0 );
	diffuse = vec3(toto.Light[lightIndex].Ld) * Object.Kd * sDotN;
 
	spec = vec3(toto.Light[lightIndex].Ls) * Object.Ks * pow( max( dot(r,v) , 0.0 ), Object.Shininess ); 
}

out vec4 FragColor;

uniform vec4 GlobalColor1;
uniform vec4 GlobalColor2;


void main() {
	if (!Object.Visible) return;

	vec3 ambientSum = vec3(0);
	vec3 diffuseSum = vec3(0);
	vec3 specSum = vec3(0);
	vec3 ambient, diffuse, spec;
    int nbVis = 0;
	if ( gl_FrontFacing )
	{
		for( int i=0; i<toto.NbLights; ++i )
		if (toto.Light[i].Visible)
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
		for( int i=0; i<toto.NbLights; ++i )
		if (toto.Light[i].Visible)
		{
			nbVis++;
			light( i, Data.Position, -Data.Normal, ambient, diffuse, spec );
			ambientSum += ambient;
			diffuseSum += diffuse;
			specSum += spec;
		}
	}
	if (nbVis>0)
		ambientSum /= nbVis;

	vec4 texColor = GlobalColor1;//texture(Tex, data.TexCoord);
	vec4 colorLinear = vec4( ambientSum + diffuseSum, 1 ) * texColor + vec4( specSum, 1 );  
	vec4 colorGammaCorrected = pow(colorLinear, vec4(1.0/toto.Gamma));

	FragColor = colorGammaCorrected;
}

