
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
  public abstract class RenderWindowBase : GameWindow
  {
    const float SphereRadius = 0.40f;//1.6f;

    public bool Bouncing { get { return RenderSettings.Bouncing; } }

    protected Matrix4 ModelMatrix, ViewMatrix, ProjectionMatrix, MvpMatrix;

    protected List<OpenGLObject> Objects { get; set; }

    protected Random rnd = new Random();

    protected void RemoveASphere()
    {
      if (Objects.Count == 0) return;
      var sphereToRemove = Objects[rnd.Next(Objects.Count)];
      sphereToRemove.Dispose();
      Objects.Remove(sphereToRemove);

    }

    protected override void OnResize(EventArgs e)
    {
      GL.Viewport(ClientRectangle);
      ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
        MathHelper.PiOver4, ClientSize.Width / (float)ClientSize.Height, 0.1f, 1000);

      // Ensure Bitmap and texture match window size
      //GL.MatrixMode(MatrixMode.Projection);
      //GL.LoadIdentity();
      //GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);


    }

    /*
         * computeFPS() - Calculate, display and return samples per second.
         * Stats are recomputed only once per second.
         */
    int frames = 0;
    Stopwatch chrono = Stopwatch.StartNew();
    double computeFPS(FrameEventArgs e)
    {

      double fps = 0.0;
      string titlestring;


      if (chrono.ElapsedMilliseconds > 1000)
      // Get current time
      //t += e.Time;  // Gets number of seconds since glfwInit()
      // If one second has passed, or if this is the very first frame
      //if ((t - t0) > 4.0 || frames == 0)
      {
        fps = ((double)frames * 1000) / chrono.ElapsedMilliseconds;
        titlestring = string.Format("GLSL noise demo ({0:0.0} FPS)", fps);
        Title = titlestring;
        // Update your text
        //renderer.Clear(Color.Black);
        //renderer.DrawString(titlestring, serif, Brushes.White, new PointF(0.0f, 0.0f));
        chrono.Restart();
        //t0 = t;
        frames = 0;
      }
      frames++;
      return fps;
    }

    virtual protected void BuildObjects()
    {
      // Create sphere data and set up buffers
      foreach (var obj in Objects)
        obj.BuildObject();
      //CreateVertexData();
    }
    

    protected void AddObject(float px, float py, float pz, float radius)
    {
      var olObject = OpenGLObject.CreateObject(px, py, pz, radius);
      Objects.Add(olObject);
    }

    virtual protected void AddARandomObject()
    {
      AddObject((float)(rnd.NextDouble() * 12.0 - 6.0), (float)(rnd.NextDouble() * 12.0 - 6.0), (float)(rnd.NextDouble() * 12.0 - 6.0), (float)(SphereRadius * (0.1 + 0.9 * rnd.NextDouble())));
    }

    protected abstract void CreateObjects();



    protected override void OnLoad(EventArgs e)
    {
      CreateObjects();
      BuildObjects();
      frames = 0;

      //renderer = new TextRenderer(Width, Height);
      PointF position = PointF.Empty;

      GL.ClearColor(Color4.Gray);

      //TargetRenderFrequency = 100;
      //TargetUpdateFrequency = 100;
      // Initialize model and view matrices once
      ViewMatrix = Matrix4.LookAt(new Vector3(7, 0, 0), Vector3.Zero, Vector3.UnitZ);
      ModelMatrix = Matrix4.CreateScale(1.0f);

    }

    protected override void OnUnload(EventArgs e)
    {
      //renderer.Dispose();
      base.OnUnload(e);
    }

    static float alt = 4;
    bool sign = true;
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      computeFPS(e);
      if (!RenderSettings.Paused && Bouncing)
      {
        if (sign)
          alt += (float)e.Time;
        else
          alt -= (float)e.Time;
        if (alt > 15 || alt < 3) sign = !sign;
      }
      ViewMatrix = Matrix4.LookAt(new Vector3(alt, 0, 0), Vector3.Zero, Vector3.UnitZ);

      foreach (var obj in Objects)
        obj.OnUpdateObject(e);

      if (!RenderSettings.Paused)
      {
        // Rotate the plane
        var modelRotation = Matrix4.CreateRotationZ((float)(e.Time * 0.5));
        Matrix4.Mult(ref ModelMatrix, ref modelRotation, out ModelMatrix);

        // Update MVP matrix
        Matrix4 mvMatrix; // MVP: Model * View * Projection
        Matrix4.Mult(ref ModelMatrix, ref ViewMatrix, out mvMatrix);
        Matrix4.Mult(ref mvMatrix, ref ProjectionMatrix, out MvpMatrix);
      }
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      // Update your text
      //renderer.Clear(Color.Black);
      //renderer.DrawString("Hello, world", serif, Brushes.White, new PointF(0.0f, 0.0f));

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      GL.Disable(EnableCap.CullFace);
      GL.Enable(EnableCap.DepthTest);
      //SwapBuffers();

      foreach (var obj in Objects)
        obj.OnRenderObject(MvpMatrix, ViewMatrix);
      //GL.MatrixMode(MatrixMode.Modelview);
      //GL.LoadIdentity();

      //GL.Enable(EnableCap.Texture2D);
      //GL.BindTexture(TextureTarget.Texture2D, renderer.Texture);
      //GL.Begin(BeginMode.Quads);

      //GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-1f, -1f);
      //GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1f, -1f);
      //GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1f, 1f);
      //GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-1f, 1f);

      //GL.End();

      SwapBuffers();
    }

    protected RenderWindowSettings RenderSettings { get; set; }

    public RenderWindowBase(RenderWindowSettings settings)
          : base(800, 600, GraphicsMode.Default, "SharpNoise OpenGL Example",
          GameWindowFlags.Default, DisplayDevice.Default, 3, 3, GraphicsContextFlags.ForwardCompatible)
    {
      RenderSettings = settings;
      VSync = VSyncMode.Off;
    }
  }
}
