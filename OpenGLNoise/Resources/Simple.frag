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

///////////
/// Main //
///////////
void main() {	 
    float maxC = max(max(Object.MainColor.r, Object.MainColor .g), max(Object.MainColor.b, 0.01));
	gl_FragColor = (Object.MainColor/maxC) * (0.6 + abs(mod(Global.Time, 2)-1) * 0.4);	
}
