using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenGLNoise.Materials
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public struct MaterialStruct
  {
    public Vector4 AmbientReflectivity;
    public Vector4 DiffuseReflectivity;
    public Vector4 SpecularReflectivity;
    public float Shininess;    
  }
}
