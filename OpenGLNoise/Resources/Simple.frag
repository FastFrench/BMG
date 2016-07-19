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
    float maxC = max(max(GlobalColor1.r, GlobalColor1 .g), max(GlobalColor1.b, 0.01));
	gl_FragColor = (GlobalColor1/maxC) * (0.6 + abs(mod(Global.Time, 2)-1) * 0.4);	
}
