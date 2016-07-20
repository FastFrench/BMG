
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
//using System.Drawing.Imaging;
using System.Text;
using System.Threading;
using OpenGLNoise.Properties;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SharpNoise;
using SharpNoise.Builders;
using SharpNoise.Modules;

namespace OpenGLNoise
{
  public class RenderTeaPot : RenderWindowBase
  {
    public RenderTeaPot(RenderWindowSettings settings) : base(settings)
    {

    }    

    override protected void CreateObjects()
    {
      base.CreateObjects();
      var teaPot = OpenGLObject.CreateTeapot(0f, 0f, 0f, 1.0f, this);
      Objects.Add(teaPot);      
    }
  }
}
