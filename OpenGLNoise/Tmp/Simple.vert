#version 430 core

layout(location = 0) in vec3 position;

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
	gl_Position =  Global.MVP * vec4( position, 1.0 );
}