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
	float Deformation;
	vec3 Speed;
	float StartingTime;
	float Radius;
	vec3 Center;
	vec4 MainColor;
	vec4 SecondaryColor;
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
	vec3 s = normalize( toto.Light[lightIndex].Position - position);
	vec3 v = normalize( -position );
	vec3 r = reflect( -s, n );

	float ratio = 1;		
	if (toto.Light[lightIndex].MaxDistance > 0.0)
	{
		float relativDistance = length(position - toto.Light[lightIndex].Position) / toto.Light[lightIndex].MaxDistance;
		ratio = 0;
		if (relativDistance < 2.2) 
		if (relativDistance < 0.2) 
			ratio = 2;
		else
			ratio = (2.2-relativDistance);
	}

	ambient = vec3(toto.Light[lightIndex].La) * Object.Ka * ratio;

	float sDotN = max( dot( s, n ), 0.0 );
	diffuse = vec3(toto.Light[lightIndex].Ld) * Object.Kd * sDotN * ratio;
 
	spec = vec3(toto.Light[lightIndex].Ls) * Object.Ks * pow( max( dot(r,v) , 0.0 ), Object.Shininess ) * ratio; 
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
	vec4 texColor = Object.MainColor;//texture(Tex, data.TexCoord);
	vec4 colorLinear = vec4( ambientSum + diffuseSum, 1 ) * texColor + vec4( specSum, 1 );  
	vec4 colorGammaCorrected = pow(colorLinear, vec4(1.0/toto.Gamma));

	FragColor = colorGammaCorrected;
}

