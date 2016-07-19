#version 440 core

//////////////////////////////////////////////
/// Global data (common to the whole scene) //
//////////////////////////////////////////////
uniform vec4 GlobalColor1;

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

///////////
/// Main //
///////////
void main() {	 
    gl_FragColor = GlobalColor1;
	gl_FragColor = GlobalColor1 * abs(mod(Global.Time, 2)-1) / 2;
	
}
