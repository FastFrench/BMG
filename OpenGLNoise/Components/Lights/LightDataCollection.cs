using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGLNoise.Components.Lights;
using OpenGLNoise.Lights;
using OpenTK;
using OpenTK.Graphics;

namespace OpenGLNoise.Lights
{

  [Serializable]
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

    public LightCollectionStruct ConvertIntoGLStruct(float gamma)
    {
      LightCollectionStruct data = new LightCollectionStruct();
      data.Init(this, gamma);
      return data;
      //return this.Where(dat => dat.Visible).Select(data => { var light = new LightStruct(); return data.FillLightStructStruct(ref light);}).ToArray();      
    }   
  }
}
