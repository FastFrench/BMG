using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGLNoise.Lights;
using OpenTK;
using OpenTK.Graphics;

namespace OpenGLNoise.Lights
{
  public class LightDataCollection : BindingList<LightData>
  {
    
    public static float b2f(byte value)
    {
      return value / (float)Byte.MaxValue;
    }

    public static Vector4 Color2Vector4(System.Drawing.Color color)
    {
      return new Vector4(b2f(color.R), b2f(color.G), b2f(color.B), b2f(color.A));
    }

    public static Vector3 Color2Vector3(System.Drawing.Color color)
    {
      return new Vector3(b2f(color.R), b2f(color.G), b2f(color.B));
    }

    public LightStruct[] ConvertIntoGLStruct()
    {
      return this.Where(dat => dat.Visible).Select(data => new LightStruct() { AmbientColor = Color2Vector4(data.AmbientColor), DiffuseColor = Color2Vector4(data.DiffuseColor), SpecularColor = Color2Vector4(data.SpecularColor), Position = data.Position, Visible = data.Visible ? true : false }).ToArray();      
    }   
  }
}
