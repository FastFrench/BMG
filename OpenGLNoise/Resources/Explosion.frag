#version 330 core


varying float vUv;
varying float noise;

void main() {

    // compose the colour using the UV coordinate
    // and modulate it with the noise like ambient occlusion
    vec3 color = vec3( vec2(vUv, 0) * ( 1. - 2. * noise ), 0.0 );
    gl_FragColor = vec4( color, 1.0 );
}