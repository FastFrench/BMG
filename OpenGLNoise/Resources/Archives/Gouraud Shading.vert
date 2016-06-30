#version 430
 
in vec3 VertexPosition;
in vec3 VertexNormal;
in vec2 VertexTex;
 
out Data
{
	vec3 FrontColor;
	vec3 BackColor;
	vec2 TexCoord;
} data;
 
struct LightInfo
{
	vec3 Position;	//Light Position in eye-coords
	vec3 La;		//Ambient light intensity
	vec3 Ld;		//Diffuse light intensity
	vec3 Ls;		//Specular light intensity
};
 
struct MaterialInfo
{
	vec3 Ka;			//Ambient reflectivity
	vec3 Kd;			//Diffuse reflectivity
	vec3 Ks;			//Specular reflectivity
	float Shininess;	//Specular shininess factor
};
 
uniform LightInfo Light[LIGHTCOUNT];
uniform MaterialInfo Material;
 
uniform mat4 ModelViewMatrix;
uniform mat3 NormalMatrix;
uniform mat4 MVP;
 
 
void getEyeSpace( out vec3 norm, out vec3 position )
{
	norm = normalize( NormalMatrix * VertexNormal );
	position = vec3( ModelViewMatrix * vec4( VertexPosition, 1 ) );
}
 
vec3 light( int lightIndex, vec3 position, vec3 norm )
{
	vec3 s = normalize( vec3( Light[lightIndex].Position - position ) );
	vec3 v = normalize( -position.xyz );
	vec3 r = reflect( -s, norm );
 
	vec3 ambient = Light[lightIndex].La * Material.Ka;
 
	float sDotN = max( dot( s, norm ), 0.0 );
	vec3 diffuse = Light[lightIndex].Ld * Material.Kd * sDotN;
 
	vec3 spec = vec3( 0.0 );
	if ( sDotN > 0.0 )
		spec = Light[lightIndex].Ls * Material.Ks * pow( max( dot(r,v) , 0.0 ), Material.Shininess );
 
	return ambient + diffuse + spec;
}
 
void main()
{
	vec3 eyeNorm;
	vec3 eyePosition;
	getEyeSpace( eyeNorm, eyePosition );
 
	data.FrontColor = vec3(0);
	data.BackColor = vec3(0); 
 
	for( int i=0; i<LIGHTCOUNT; ++i )
	{
		data.FrontColor += light( i, eyePosition, eyeNorm );
		data.BackColor +=  light( i, eyePosition, -eyeNorm );
	}
 
	data.TexCoord = VertexTex;
	gl_Position = MVP * vec4( VertexPosition, 1 );
}
