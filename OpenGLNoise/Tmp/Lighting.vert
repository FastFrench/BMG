#version 430 core

// Output data ; will be interpolated for each fragment.
layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;

// Values that stay constant for the whole mesh.
struct MaterialInfo
{
	vec3 Ka;
	vec3 Kd;
	vec3 Ks;
	float Shininess;
	vec3 Speed;
	float StartingTime;
	float Size;
	bool Visible;
	bool UsingNoise;
};
uniform MaterialInfo Object;

out struct DataStruct 
{
	vec3 Normal;
	vec3 Position;
	vec3 Eye;
} Data;

struct GlobalData
{
	mat4 MVP;
	mat4 View;
	float Time;
	float Gamma;
	int NbLight;
}
layout (std140) uniform GlobalSettings 
{
	GlobalData Global;
}


void main() {
    vec4 pos4 = vec4( position + Object.Speed * (Global.Time - Object.StartingTime), 1.0 );
	Data.Normal = normal;
    gl_Position =  Global.MVP * pos4;
	Data.Eye = -(Global.View * pos4).xyz;
	Data.Position = position;
}