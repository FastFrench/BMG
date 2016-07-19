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

//////////////////////////////////////////
// Input data, specific for each vertex //
//////////////////////////////////////////
layout(location = 0) in vec3 position;

//////////
// Main //
//////////
void main() {
	gl_Position =  Global.MVP * vec4( position, 1.0 );
}