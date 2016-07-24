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
	float Deformation;
	vec3 Speed;
	float StartingTime;
	float Radius;
	vec3 Center;
	vec4 MainColor;
	vec4 SecondaryColor;
};

uniform ObjectInfo Object;

//////////////////////////////////////////
// Input data, specific for each vertex //
//////////////////////////////////////////
layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;

//////////
// Main //
//////////
void main() {
	vec3 pos3 = position;
	pos3 += Object.Deformation * normal;
	gl_Position =  Global.MVP * vec4( pos3 + Object.Speed * (Global.Time - Object.StartingTime), 1.0 );
}