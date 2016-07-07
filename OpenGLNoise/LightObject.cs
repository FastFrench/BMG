using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenGLNoise
{
  public class LightObject : SphereObject
  {
    public LightObject(Vector3 center, Color color) : base(center, 0.1f, true, false, false)
    {
      Color1 = Color2 = color;
      
    }

    
  }
}
