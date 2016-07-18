using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLNoise.Materials
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = sizeof(float))]
  public struct MaterialStruct
  {
    public Vector3 AmbientReflectivity { get; set; }
    public Vector3 DiffuseReflectivity { get; set; }
    public Vector3 SpecularReflectivity { get; set; }
    public float Shininess { get; set; }

    // Not really material... global
    public float Gamma { get; set; }
    public int NbLight { get; set; }

    // Not really material... specific to this object
    public bool Visible { get; set; }
    public bool UsingNoise { get; set; }
    public float Size { get; set; }

    public void SetUniforms(int baseUniformLocation)
    {
      GL.Uniform3(baseUniformLocation++, AmbientReflectivity);
      var error = GL.GetError();
      if (error != ErrorCode.NoError)
      {
        Debug.Print("OpenGL error (SetUniforms AmbientReflectivity): " + error.ToString());
        baseUniformLocation--;
      }

      GL.Uniform3(baseUniformLocation++, DiffuseReflectivity);
      error = GL.GetError();
      if (error != ErrorCode.NoError)
      {
        Debug.Print("OpenGL error (SetUniforms DiffuseReflectivity): " + error.ToString());
        baseUniformLocation--;
      }
      GL.Uniform3(baseUniformLocation++, SpecularReflectivity);
      error = GL.GetError();
      if (error != ErrorCode.NoError)
      {
        Debug.Print("OpenGL error (SetUniforms SpecularReflectivity): " + error.ToString());
        baseUniformLocation--;
      }
      GL.Uniform1(baseUniformLocation++, Shininess);
      error = GL.GetError();
      if (error != ErrorCode.NoError)
      {
        Debug.Print("OpenGL error (SetUniforms Shininess): " + error.ToString());
        baseUniformLocation--;
      }//GL.Uniform1(baseUniformLocation++, Gamma);
      //GL.Uniform1(baseUniformLocation++, NbLight);
      GL.Uniform1(baseUniformLocation++, Visible ? 1 : 0);
      error = GL.GetError();
      if (error != ErrorCode.NoError)
      {
        Debug.Print("OpenGL error (SetUniforms Visible): " + error.ToString());
        baseUniformLocation--;
      }
      //GL.Uniform1(baseUniformLocation++, UsingNoise ? 1 : 0);
      //error = GL.GetError();
      //if (error != ErrorCode.NoError)
      //{
      //  Debug.Print("OpenGL error (SetUniforms UsingNoise): " + error.ToString());
      //  baseUniformLocation--;
      //}
      //GL.Uniform1(baseUniformLocation++, Size);
      //error = GL.GetError();
      //if (error != ErrorCode.NoError)
      //{
      //  Debug.Print("OpenGL error (SetUniforms Size): " + error.ToString());
      //  baseUniformLocation--;
      //}
    }
  }
}
