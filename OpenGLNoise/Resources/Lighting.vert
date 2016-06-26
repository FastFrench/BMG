#version 330 core

// Output data ; will be interpolated for each fragment.
varying vec3  fragt_normal;
varying vec3 eye;
layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;

// Values that stay constant for the whole mesh.
uniform mat4 MVP;
uniform mat4 View;

void main() {
    vec4 pos4 = vec4( position, 1.0 );
	fragt_normal = normal;
    gl_Position =  MVP * pos4;
	eye = -(View * pos4).xyz;
}