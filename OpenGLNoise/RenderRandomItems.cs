
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
  public class RenderRandomItems : RenderWindowBase
  {
    public RenderRandomItems(RenderWindowSettings settings)
    : base(settings)
    {

    }

    override protected void CreateObjects()
    {
      base.CreateObjects();
      int nbSpheres = rnd.Next(4, 20);
      for (int i = 0; i < nbSpheres; i++)
        AddARandomObject();
    }
  }
}
