#version 440 core

// Output data ; will be interpolated for each fragment.
//varying vec3  fragt_normal;
//varying vec3 eye;
layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;

// Values that stay constant for the whole mesh.
uniform mat4 MVP;
uniform mat4 View;

out struct DataStruct 
{
	vec3 Normal;
	vec3 Position;
	vec3 Eye;
} Data;

struct MaterialInfo
{
	vec3 Ka;
	vec3 Kd;
	vec3 Ks;
	float Shininess;
	bool Visible;
	bool UsingNoise;
	float Size;
};

uniform MaterialInfo Object;

void main() {
    vec4 pos4 = vec4( position, 1.0 );
	if (Object.UsingNoise)
		pos4 += vec4(Object.Size * normal, 0.0);
	Data.Normal = normal;
    gl_Position =  MVP * pos4;
	Data.Eye = -(View * pos4).xyz;
	Data.Position = position;
}