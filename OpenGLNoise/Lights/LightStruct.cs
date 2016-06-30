using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenGLNoise.Lights
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public struct LightStruct
  {
    public Vector4 AmbientColor;
    public Vector4 DiffuseColor;
    public Vector4 SpecularColor;
    public Vector3 Position;
    public bool Visible; 
  }
}
