using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGLNoise.Properties;
using OpenTK;

namespace OpenGLNoise
{
  public class TeaPotObject : ObjectFromRessourceData
  {

    public TeaPotObject(Vector3 center, float size, bool withDeformations, Color? color1 = null, Color? color2 = null)
        : base(center, size, withDeformations, color1, color2)
    {
    }

    static Vector3[] vertices = null;
    static Vector3[] normals = null;
    static int[] indices = null;

    override protected bool GetStaticData(out Vector3[] _vertices, out int[] _indices, out Vector3[] _normals)
    {
      try
      {
        if (vertices == null && !base.ReadData(Resources.teapot, out vertices, out indices, out normals)) return false;
        return true;
      }
      finally
      {
        _vertices = vertices;
        _indices = indices;
        _normals = normals;
      }
    }   
  }
}
