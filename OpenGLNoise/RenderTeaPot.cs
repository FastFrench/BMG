
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

    protected override OpenGLObject AddObject(Vector3 center, float radius)
    {
      var olObject = OpenGLObject.CreateTeapot(center, radius, this);
      Objects.Add(olObject);
	  return olObject;
	}

    override protected void CreateObjects()
    {
      base.CreateObjects();

      var teaPot = OpenGLObject.CreateTeapot(Vector3.Zero, 1.0f, this);
      Objects.Add(teaPot);      
    }
  }
}
