#version 330 core

// Output data ; will be interpolated for each fragment.
layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;

struct FragDataStruct
{
	vec3 Normal;
	vec3 Position;	
	vec3 Eye;
};
out FragDataStruct FragData;

// Values that stay constant for the whole mesh.
uniform mat4 MVP;
uniform mat4 View;

void main() {
    vec4 pos4 = vec4( position, 1.0 );
	FragData.Normal = normal;
    FragData.Position = position;
    gl_Position =  MVP * pos4;
	FragData.Eye = -(View * pos4).xyz;
}