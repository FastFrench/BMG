#version 440 core
precision highp float;
const int LIGHTCOUNT = 3;

//////////////////////////////////////////////
/// Global data (common to the whole scene) //
//////////////////////////////////////////////
struct LightInfo
{
	vec3 La;			//Ambient light intensity
	vec3 Ld;			//Diffuse light intensity
	vec3 Ls;			//Specular light intensity
	vec3 Position;		//Light Position in eye-coords
	bool Visible;   
	float MaxDistance;  // Max distance over which the light is not visible anymore
};

layout (std140) uniform Lights
{
	float		Gamma;
	int			NbLights;
	LightInfo	Light[LIGHTCOUNT];
} toto;

struct GlobalData
{
	mat4 MVP;
	mat4 View;
	float Time;	
};

layout (std140) uniform GlobalSettings 
{
	GlobalData Global;
};

uniform vec4 GlobalColor1;
uniform vec4 GlobalColor2;

///////////////////////////////////////////////
/// Object data (common to current objected) //
///////////////////////////////////////////////
struct ObjectInfo
{
	vec3	Ka;
	vec3	Kd;
	vec3	Ks;
	float	Shininess;
	bool	Visible;
	bool	UsingNoise;
	float Size;
	vec3 Speed;
	float StartingTime;
};

uniform ObjectInfo Object;


////////////////////////////////////////////
// Input data, specific for each fragment //
////////////////////////////////////////////
in struct DataStruct 
{
	vec3 Normal;
	vec3 Position;
	vec3 Eye;
} Data;

//////////////////////////////////////
// Output data (color of the pixel) //
//////////////////////////////////////
out vec4 FragColor;

/////////////////
// SubRoutines //
/////////////////
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
	if (toto.Light[lightIndex].MaxDistance > 0.0)
	{
		float relativDistance = length(position - vec3(toto.Light[lightIndex].Position)) / toto.Light[lightIndex].MaxDistance;
		if (relativDistance > 1.0) 
		{
			diffuse = vec3(0);
			ambient = vec3(0);
		}
		else
		{
			diffuse *= (1-relativDistance);
			ambient *= (1-relativDistance);
		}
	}
}


//////////
// Main //
//////////
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
	//if (nbVis>0)
	//	ambientSum /= nbVis;
	vec4 texColor = GlobalColor1;//texture(Tex, data.TexCoord);
	vec4 colorLinear = vec4( ambientSum + diffuseSum, 1 ) * texColor + vec4( specSum, 1 );  
	vec4 colorGammaCorrected = pow(colorLinear, vec4(1.0/toto.Gamma));

	FragColor = colorGammaCorrected;
}

