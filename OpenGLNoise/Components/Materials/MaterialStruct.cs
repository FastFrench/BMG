﻿using System;
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

    // Not really material... but still specific to this object
    public bool Visible { get; set; }
    public bool UsingNoise { get; set; }
    public float Size { get; set; }

    public Vector3 Speed { get; set; }
    public float StartingTime { get; set; }

    const string uniformName = "Object.";
    readonly static string[] uniformFields = {
      "Ka",
      "Kd",
      "Ks",
      "Shininess",
      "Visible",
      "UsingNoise",
      "Size",
      "Speed",
      "StartingTime"};
    enum uniformNamesEnum
    {
      Ka, Kd, Ks, Shininess, Visible, UsingNoise, Size, Speed, StartingTime
    };
    Dictionary<uniformNamesEnum, int> uniformLocation;
    static int? baseUniformLocation = null;

    private int location(int programHandle, uniformNamesEnum nameEnum)
    {
      if (uniformLocation == null)
        uniformLocation = new Dictionary<uniformNamesEnum, int>();
      if (!uniformLocation.ContainsKey(nameEnum))
      {
        uniformLocation[nameEnum] = GL.GetUniformLocation(programHandle, uniformName+uniformFields[(int)nameEnum]);
        if (uniformLocation[nameEnum] < 0)
          Debug.Print("Uniform {0} not found", uniformName+uniformFields[(int)nameEnum]);
      }
      return uniformLocation[nameEnum];
    }

    void SetUniformValue(int programHandle, uniformNamesEnum field, object data)
    {
      int fieldLocation = location(programHandle, field);
      if (fieldLocation >= 0)
      {
        OpenGLHelper.SetUniformValue(fieldLocation, data);
        OpenGLHelper.CheckError("SetUniforms " + field);
      }
    }

    public void SetUniforms(int programHandle)
    {
      SetUniformValue(programHandle, uniformNamesEnum.Ka, AmbientReflectivity);
      SetUniformValue(programHandle, uniformNamesEnum.Kd, DiffuseReflectivity);
      SetUniformValue(programHandle, uniformNamesEnum.Ks, SpecularReflectivity);
      SetUniformValue(programHandle, uniformNamesEnum.Shininess, Shininess);
      SetUniformValue(programHandle, uniformNamesEnum.Visible, Visible);
      SetUniformValue(programHandle, uniformNamesEnum.UsingNoise, UsingNoise);
      SetUniformValue(programHandle, uniformNamesEnum.Size, Size);
      SetUniformValue(programHandle, uniformNamesEnum.Speed, Speed);
      SetUniformValue(programHandle, uniformNamesEnum.StartingTime, StartingTime);
    }
  }
}
