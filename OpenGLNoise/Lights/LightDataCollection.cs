using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGLNoise.Lights;
using OpenTK;
using OpenTK.Graphics;

namespace OpenGLNoise.Lights
{
  public class LightDataCollection : ObservableCollection<LightData>
  {     
    float b2f(byte value)
    {
      return value / (float)Byte.MaxValue;
    }

    Vector4 Color2Vector(System.Drawing.Color color)
    {
      return new Vector4(b2f(color.R), b2f(color.G), b2f(color.B), b2f(color.A));
    }

    public LightStruct[] ConvertIntoGLStruct()
    {
      return this.Select(data => new LightStruct() { Color = Color2Vector(data.Color), Position = data.Position, Visible = data.Visible }).ToArray();      
    }   
  }
}
