#version 330 core

// Output data ; will be interpolated for each fragment.
varying vec3  fragt_normal;
layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;

// Values that stay constant for the whole mesh.
uniform mat4 MVP;

void main() {
	fragt_normal = normal;
    gl_Position =  MVP * vec4( position, 1.0 );
}