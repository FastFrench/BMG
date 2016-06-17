#version 330 core


varying float vUv;
varying float noise;

void main() {

    // compose the colour using the UV coordinate
    // and modulate it with the noise like ambient occlusion
    gl_FragColor = vec4( vUv * ( 1. - 2. * noise ), 0., 0., 1.);
}