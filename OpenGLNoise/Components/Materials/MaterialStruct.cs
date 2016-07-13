using System;
using System.Collections.Generic;
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
      GL.Uniform3(baseUniformLocation++, DiffuseReflectivity);
      GL.Uniform3(baseUniformLocation++, SpecularReflectivity);
      GL.Uniform1(baseUniformLocation++, Shininess);
      GL.Uniform1(baseUniformLocation++, Gamma);
      GL.Uniform1(baseUniformLocation++, NbLight);
      GL.Uniform1(baseUniformLocation++, Visible ? 1 : 0);
      GL.Uniform1(baseUniformLocation++, UsingNoise ? 1 : 0);
      GL.Uniform1(baseUniformLocation++, Size);
    }
  }
}
