﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenGLNoise.Components.Lights
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = sizeof(float)*4, Size = sizeof(float) * 4 * 4)]
  public struct LightStruct
  {
    public Vector4 AmbientColor;
    public Vector4 DiffuseColor;
    public Vector4 SpecularColor;
    public Vector3 Position;
    public bool Visible;        
  }
}
