
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
//using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using OpenGLNoise.Lights;
using OpenGLNoise.Properties;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using SharpNoise;
using SharpNoise.Builders;
using SharpNoise.Modules;

namespace OpenGLNoise
{
  public abstract class RenderWindowBase : GameWindow
  {
    #region Array of Lights data
    int LightsBufferUBO; // Lights: Location for the UBO given by OpenGL
    public const int LIGHTS_BUFFER_INDEX = 7; // Lights : Index to use for the buffer binding (All good things start at 0 )
    LightStruct[] LightsUBOData = null;
    void InitLightBuffer()
    {
      LightsUBOData = RenderSettings.Lights.ConvertIntoGLStruct(); // Create actual data
      GL.GenBuffers(1, out LightsBufferUBO); // Generate the buffer
      GL.BindBuffer(BufferTarget.UniformBuffer, LightsBufferUBO); // Bind the buffer for writing
      GL.BufferData(BufferTarget.UniformBuffer, (IntPtr)(Marshal.SizeOf<LightStruct>() * LightsUBOData.Length), (IntPtr)(null), BufferUsageHint.DynamicDraw); // Request the memory to be allocated

      GL.BindBufferRange(BufferRangeTarget.UniformBuffer, LIGHTS_BUFFER_INDEX, LightsBufferUBO, (IntPtr)0, (IntPtr)(Marshal.SizeOf<LightStruct>() * LightsUBOData.Length)); // Bind the created Uniform Buffer to the Buffer Index
    }
 
    void FillLightUniformBuffer()
    {
      LightsUBOData = RenderSettings.Lights.ConvertIntoGLStruct(); // Create actual data
      GL.BindBuffer(BufferTarget.UniformBuffer, LightsBufferUBO);
      GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)0, (IntPtr)(Marshal.SizeOf<LightStruct>() * LightsUBOData.Length), LightsUBOData);
      GL.BindBuffer(BufferTarget.UniformBuffer, 0);
    }
    #endregion Array of Lights data

    #region Zooming
    float zoom = 1.0f;
    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
      base.OnMouseWheel(e);
      float newZoom = e.Delta > 0 ? 1.05f : 0.95f;
      zoom *= newZoom;
      ModelMatrix = ModelMatrix * Matrix4.CreateScale(newZoom);
    }
    #endregion Zooming

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
      var olObject = OpenGLObject.CreateObject(px, py, pz, radius, this);
      Objects.Add(olObject);
    }

    virtual protected void AddARandomObject()
    {
      AddObject((float)(rnd.NextDouble() * 12.0 - 6.0), (float)(rnd.NextDouble() * 12.0 - 6.0), (float)(rnd.NextDouble() * 12.0 - 6.0), (float)(SphereRadius * (0.1 + 0.9 * rnd.NextDouble())));
    }

    protected virtual void CreateObjects()
    {
      // Initialize List of objects
      if (Objects != null)
        foreach (var obj in Objects)
          obj.Dispose();
      Objects = new List<OpenGLObject>();

      InitLightBuffer();
      // Add lights
      UpdateLights(false);
    }

    private void UpdateLights(bool buildThem = true)
    {
      foreach (var light in Objects.OfType<LightObject>().ToArray())
      {
        Objects.Remove(light);
        light.Dispose();
      }

      FillLightUniformBuffer();
      foreach (var light in RenderSettings.Lights)
      {
        var lightObj = new LightObject(light.Position, light.GlobalColor);
        lightObj.LoadShaders(Resources.Simple_frag, Resources.Simple_vert, null);
        lightObj.BuildObject();
        Objects.Add(lightObj);
      }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      base.OnClosing(e);
    }

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
      FillLightUniformBuffer();
      if (!RenderSettings.Paused && Bouncing)
      {
        if (sign)
          alt += (float)e.Time;
        else
          alt -= (float)e.Time;
        if (alt > 15 || alt < 3) sign = !sign;
      }
      ViewMatrix = Matrix4.LookAt(new Vector3(0, alt, 0), new Vector3(0, 0, 0), Vector3.UnitZ);

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
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      GL.Disable(EnableCap.CullFace);
      GL.Enable(EnableCap.DepthTest);

      foreach (var obj in Objects)
        obj.OnRenderObject(MvpMatrix, ViewMatrix);


      SwapBuffers();
    }

    public RenderWindowSettings RenderSettings { get; set; }

    public RenderWindowBase(RenderWindowSettings settings)
          : base(800, 600, GraphicsMode.Default, "SharpNoise OpenGL Example",
          GameWindowFlags.Default, DisplayDevice.Default, 3, 3, GraphicsContextFlags.ForwardCompatible)
    {
      RenderSettings = settings;
      RenderSettings.Lights.ListChanged += Lights_ListChanged;
      VSync = VSyncMode.Off;
    }

    private void Lights_ListChanged(object sender, ListChangedEventArgs e)
    {
      UpdateLights();
    }
  }
}
