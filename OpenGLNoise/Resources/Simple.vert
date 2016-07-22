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
	float Size;
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

//////////
// Main //
//////////
void main() {
	gl_Position =  Global.MVP * vec4( position + Object.Speed * (Global.Time - Object.StartingTime), 1.0 );
}