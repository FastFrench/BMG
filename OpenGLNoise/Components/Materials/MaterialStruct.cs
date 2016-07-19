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

    // Not really material... but still specific to this object
    public bool Visible { get; set; }
    public bool UsingNoise { get; set; }
    public float Size { get; set; }
    const string uniformName = "Object.";
    readonly static string[] uniformFields = {
      "Ka",
      "Kd",
      "Ks",
      "Shininess",
      "Visible",
      "UsingNoise",
      "Size" };
    enum uniformNamesEnum
    {
      Ka, Kd, Ks, Shininess, Visible, UsingNoise, Size
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

    public void SetUniforms(int programHandle)
    {
      int fieldLocation = location(programHandle, uniformNamesEnum.Ka);
      if (fieldLocation >= 0)
      {
        GL.Uniform3(fieldLocation, AmbientReflectivity);
        var error = GL.GetError();
        if (error != ErrorCode.NoError)
        {
          Debug.Print("OpenGL error (SetUniforms AmbientReflectivity): " + error.ToString());
        }
      }

      fieldLocation = location(programHandle, uniformNamesEnum.Kd);
      if (fieldLocation >= 0)
      {
        GL.Uniform3(fieldLocation, DiffuseReflectivity);
        var error = GL.GetError();
        if (error != ErrorCode.NoError)
          Debug.Print("OpenGL error (SetUniforms DiffuseReflectivity): " + error.ToString());
      }

      fieldLocation = location(programHandle, uniformNamesEnum.Ks);
      if (fieldLocation >= 0)
      {
        GL.Uniform3(fieldLocation, SpecularReflectivity);
        var error = GL.GetError();
        if (error != ErrorCode.NoError)
          Debug.Print("OpenGL error (SetUniforms SpecularReflectivity): " + error.ToString());
      }

      fieldLocation = location(programHandle, uniformNamesEnum.Shininess);
      if (fieldLocation >= 0)
      {
        GL.Uniform1(fieldLocation, Shininess);
        var error = GL.GetError();
        if (error != ErrorCode.NoError)
          Debug.Print("OpenGL error (SetUniforms Shininess): " + error.ToString());
      }

      fieldLocation = location(programHandle, uniformNamesEnum.Visible);
      if (fieldLocation >= 0)
      {
        GL.Uniform1(fieldLocation, Visible ? 1 : 0);
        var error = GL.GetError();
        if (error != ErrorCode.NoError)
          Debug.Print("OpenGL error (SetUniforms Visible): " + error.ToString());
      }

      fieldLocation = location(programHandle, uniformNamesEnum.UsingNoise);
      if (fieldLocation >= 0)
      {
        GL.Uniform1(fieldLocation, UsingNoise ? 1 : 0);
        var error = GL.GetError();
        if (error != ErrorCode.NoError)
          Debug.Print("OpenGL error (SetUniforms UsingNoise): " + error.ToString());
      }

      fieldLocation = location(programHandle, uniformNamesEnum.Size);
      if (fieldLocation >= 0)
      {
        GL.Uniform1(fieldLocation, Size);
        var error = GL.GetError();
        if (error != ErrorCode.NoError)
          Debug.Print("OpenGL error (SetUniforms Size): " + error.ToString());
      }
    }
  }
}
