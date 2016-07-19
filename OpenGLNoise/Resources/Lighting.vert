#version 440 core

//////////////////////////////////////////////
/// Global data (common to the whole scene) //
//////////////////////////////////////////////
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
	float	Size;
	vec3	Speed;
	float	StartingTime;
};

uniform ObjectInfo Object;

//////////////////////////////////////////
// Input data, specific for each vertex //
//////////////////////////////////////////
layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;

///////////////////////////////////////////////////////////
/// Output data (will be interpolated for each fragment) //
///////////////////////////////////////////////////////////
out struct DataStruct 
{
	vec3 Normal;
	vec3 Position;
	vec3 Eye;
} Data;

//////////
// Main //
//////////
void main() {
	vec3 currentPosition = position + Object.Speed * (Global.Time - Object.StartingTime);
    vec4 pos4 = vec4( currentPosition, 1.0 );
	if (Object.UsingNoise)
		pos4 += vec4(Object.Size * normal, 0.0);
	Data.Normal = normal;
    gl_Position =  Global.MVP * pos4;
	Data.Eye = -(Global.View * pos4).xyz;
	Data.Position = currentPosition;
}