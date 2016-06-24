#version 330
 
//layout (std140) uniform Matrices {
    //mat4 m_pvm;
    //mat4 m_viewModel;
    //mat3 m_normal;
//};
 uniform mat4 View;
 // Output data ; will be interpolated for each fragment.
varying vec3  fragt_normal;
layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;

// Values that stay constant for the whole mesh.
uniform mat4 MVP;
//in vec4 position;   // local space
//in vec3 normal;     // local space
 
// the data to be sent to the fragment shader
//out Data {
//    vec3 normal;
//    vec4 eye;
//} DataOut;
out vec4 eye;

void main () { 
	fragt_normal = normal;
    gl_Position =  MVP * vec4( position, 1.0 );

    //DataOut.normal = normalize(m_normal * normal);
    //DataOut.eye 
	eye = -(View * position);
 
    gl_Position = MVP * position; 
}
