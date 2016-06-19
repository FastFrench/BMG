#version 330 core


varying float vUv;
varying float noise;

void main() {

    // compose the colour using the UV coordinate
    // and modulate it with the noise like ambient occlusion
	float color = vUv * ( 1. - 2. * noise );
	 
    gl_FragColor = vec4( 0, 0, color, 1.0 );
}
