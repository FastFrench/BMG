using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenGLNoise.Components.Lights
{
  [Serializable]
  [StructLayout(LayoutKind.Explicit, Size = sizeof(float) * 4 * 5)]
  public struct LightStruct
  {
    [FieldOffset(0)]
    public Vector4 AmbientColor;
    [FieldOffset(16)]
    public Vector4 DiffuseColor;
    [FieldOffset(32)]
    public Vector4 SpecularColor;
    [FieldOffset(48)]
    public Vector3 Position;
    [FieldOffset(60)]
    public bool Visible;
    [FieldOffset(64)]
    public float MaxDistance;
  }
}
