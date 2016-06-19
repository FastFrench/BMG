#version 330 core


varying float vUv;
varying float noise;
uniform vec4 GlobalColor1;
uniform vec4 GlobalColor2;
void main() {

    // compose the colour using the UV coordinate
    // and modulate it with the noise like ambient occlusion
	float colorRange = clamp(vUv * ( 1. - 2. * noise ), 0, 1);
	
	//float colorRed = clamp(colorRange*3, 0, 1);
	//float colorGreen = clamp(colorRange*3, 1, 2)-1;
	//float colorBlue = clamp(colorRange*3, 2, 3)-2;
	 
    gl_FragColor = mix(GlobalColor1, GlobalColor2, colorRange); // vec4( colorRed, colorGreen, colorBlue, 1.0 );
}
