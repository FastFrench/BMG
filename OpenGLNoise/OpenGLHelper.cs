using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLNoise
{
  public static class OpenGLHelper
  {
    public static void SetUniformValue(int location, object data)
    {
      if (location >= 0)
      {
        if (data is float)
          GL.Uniform1(location, (float)data);
        else
        if (data is int)
          GL.Uniform1(location, (int)data);
        else
        if (data is bool)
          GL.Uniform1(location, (int)((bool)data ? 1 : 0));
        else
          if (data is Vector3)
          GL.Uniform3(location, (Vector3)data);
        else
          if (data is Vector4)
          GL.Uniform4(location, (Vector4)data);
        else
          throw new NotImplementedException(string.Format("The type {0} is not supported", data.GetType().Name));
      }
    }

    /// <summary>
    /// Check if there is an OpenGL Error. Returns true if OK, false on error. 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    static public bool CheckError(string context)
    {
      var error = GL.GetError();
      if (error != ErrorCode.NoError)
      {
        Debug.Print("OpenGL error ({0}): {1}", context, error);
        return false;
      }
      return true;
    }
  }
}
