using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenGLNoise.Lights;
using OpenGLNoise.Materials;
using OpenGLNoise.Properties;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SharpNoise;
using SharpNoise.Builders;
using SharpNoise.Modules;

namespace OpenGLNoise
{
  public abstract class OpenGLObject : IDisposable
  {
    RenderWindowBase _parent;
    public RenderWindowBase Parent
    {
      get { return _parent; }
      set
      {
        _parent = value;
        if (value != null) Parent.RenderSettings.PropertyChanged += RenderSettings_PropertyChanged;
      }
    }

    protected bool WithNoise { get; set; }
    protected bool WithLightsArray { get; set; }
    protected OpenGLObject(float size = 1.0f, Color? color1 = null, Color? color2 = null, bool withNoise = false, bool withLightsArray = true)
    {
      WithLightsArray = withLightsArray;
      WithNoise = withNoise;
      Size = size;
      Color1 = color1 ?? Color.Red;
      Color2 = color2 ?? Color.Black;
    }
    public Vector3[] Positions { get; set; }
    public Vector3[] Normals { get; set; }
    public int[] Indices { get; set; }

    public int ProgramHandle { get; set; }

    public int ElementCount
    {
      get
      {
        if (Indices == null) return 0;
        return Indices.Length;
      }
    }

    static protected Random rnd = new Random();

    /// <summary>
    /// Initial building of the object (usually called in OnLoad or chen the object appears)
    /// Base class method should be called last
    /// </summary>
    virtual public void BuildObject()
    {
      SetupBuffers();
      if (WithNoise)
        SetupNoiseMapBuilder();
    }

    /// <summary>
    /// Called if something to clean when the object is destroyed
    /// </summary>
    virtual public void Dispose()
    {

    }

    #region Noise
    TranslatePoint TimeTranslator;
    NoiseMap NoiseMap;
    PlaneNoiseMapBuilder NoiseMapBuilder;
    protected int MvpUniformLocation { get; set; }
    protected int ViewUniformLocation { get; set; }

    protected int SizeUniformLocation { get; set; }
    protected int Color1UniformLocation { get; set; }
    protected int Color2UniformLocation { get; set; }
    protected int GammaUniformLocation { get; set; }

    protected int ObjectDataUniform { get; set; }

    protected int LightsUniformBlockLocation { get; set; }

    void GenerateElevationNoise(double timeDelta)
    {
      if (!WithNoise) return;

      // Shift time and generate noise
      TimeTranslator.YTranslation += timeDelta;
      NoiseMapBuilder.Build();
    }

    protected virtual int lowerBoundX { get { return 0; } set { } }
    protected virtual int upperBoundX { get; set; }
    protected virtual int lowerBoundZ { get { return 0; } set { } }
    protected virtual int upperBoundZ { get; set; }
    void SetupNoiseMapBuilder()
    {
      // Set up noise module tree
      // TranslatePoint is used to shift the generated noise over time
      TimeTranslator = new TranslatePoint
      {
        // Scales the generated noise values down to 80%
        Source0 = new ScaleBias
        {
          Scale = 0.8,
          Bias = 0,
          // Scale coordinates down to get some rougher structures
          Source0 = new ScalePoint
          {
            // Scale down xz-plane
            XScale = 0.0375,
            ZScale = 0.0375,
            // Scale down "time"
            YScale = 0.625,
            Source0 = new Billow(),
          },
        },
      };

      // Set up target noise map and noise map builder            
      NoiseMap = new NoiseMap();
      NoiseMapBuilder = new PlaneNoiseMapBuilder
      {
        DestNoiseMap = NoiseMap,
        SourceModule = TimeTranslator,
      };
      NoiseMapBuilder.SetBounds(lowerBoundX, upperBoundX, lowerBoundZ, upperBoundZ);
      NoiseMapBuilder.SetDestSize(upperBoundX - lowerBoundX, upperBoundZ - lowerBoundZ);
    }

    #endregion Noise

    Stopwatch sw = Stopwatch.StartNew();
    /// <summary>
    /// Called on OnUpdateFrame
    /// </summary>
    virtual public void OnUpdateObject(FrameEventArgs e)
    {
      double ratio = (sw.ElapsedMilliseconds % 2000) / 1000.0;
      if (ratio > 1.0) ratio = 2 - ratio;
      AjustedDeformationSize = (float)(Size * ratio);
      var error = GL.GetError();
      if (error != ErrorCode.NoError)
        Debug.Print("OpenGL error(0): " + error.ToString());

      // Update elevation data
      if (WithNoise)
      {
        GenerateElevationNoise(e.Time);
        GL.BindBuffer(BufferTarget.ArrayBuffer, ElevationBuffer);
        GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr)(NoiseMap.Data.Length * sizeof(float)), NoiseMap.Data);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      }
      error = GL.GetError();
      if (error != ErrorCode.NoError)
        Debug.Print("OpenGL error (1): " + error.ToString());
      // Update normals
      if (Normals != null)
      {
        GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBuffer);
        GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr)(Normals.Length * Vector3.SizeInBytes), Normals);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        Normals = null;
      }
      error = GL.GetError();
      if (error != ErrorCode.NoError)
        Debug.Print("OpenGL error (2): " + error.ToString());

    }

    #region Shaders and Programs management
    #region Shader Loading

    public void LoadShaders(byte[] pixelShaderData, byte[] vertexShaderData, byte[] geometryShaderData = null)
    {
      int? pixelShader = null;
      int? vertexShader = null;
      int? geometryShader = null;
      if (pixelShaderData != null)
        pixelShader = LoadShaderFromResource(ShaderType.FragmentShader, pixelShaderData, "Fragment Shader");
      if (vertexShaderData != null)
        vertexShader = LoadShaderFromResource(ShaderType.VertexShader, vertexShaderData, "Vertex Shader");
      if (geometryShaderData != null)
        geometryShader = LoadShaderFromResource(ShaderType.GeometryShader, geometryShaderData, "Geometry Shader");
      CreateAndLinkProgram(pixelShader, vertexShader, geometryShader);
      MvpUniformLocation = GL.GetUniformLocation(ProgramHandle, "MVP");
      ViewUniformLocation = GL.GetUniformLocation(ProgramHandle, "View");
      SizeUniformLocation = GL.GetUniformLocation(ProgramHandle, "Size");
      Color1UniformLocation = GL.GetUniformLocation(ProgramHandle, "GlobalColor1");
      Color2UniformLocation = GL.GetUniformLocation(ProgramHandle, "GlobalColor2");
      GammaUniformLocation = GL.GetUniformLocation(ProgramHandle, "Gamma");
      ObjectDataUniform = GL.GetUniformLocation(ProgramHandle, "Object.Ka");
      if (WithLightsArray)
      {
        LightsUniformBlockLocation = GL.GetUniformBlockIndex(ProgramHandle, "Lights"); // LightInfo and LightInfo[0] are both valid and equivalent

        if (LightsUniformBlockLocation >= 0)
          GL.UniformBlockBinding(ProgramHandle, LightsUniformBlockLocation, RenderWindowBase.LIGHTS_BUFFER_INDEX);
        else
          WithLightsArray = false; // Autoset to false
      }
      var error = GL.GetError();
      if (error != ErrorCode.NoError)
        Debug.Print("OpenGL error (LoadShaders): " + error.ToString());

    }


    public void LoadShaders(string pixelShaderName, string vertexShaderName, string geometryShaderName = null)
    {
      LoadShaders((byte[])Resources.ResourceManager.GetObject(pixelShaderName), (byte[])Resources.ResourceManager.GetObject(vertexShaderName), (byte[])Resources.ResourceManager.GetObject(geometryShaderName));
    }

    int LoadShaderFromResource(ShaderType shaderType, string resourceName)
    {
      return LoadShaderFromResource(shaderType, (byte[])Resources.ResourceManager.GetObject(resourceName), resourceName);
    }

    int LoadShaderFromResource(ShaderType shaderType, byte[] data, string name)
    {
      // Load shader from WinForms resource manager thing
      var shaderSource = Encoding.UTF8.GetString(data);

      // Create and compile shader
      var shaderHandle = GL.CreateShader(shaderType);
      GL.ShaderSource(shaderHandle, shaderSource);
      GL.CompileShader(shaderHandle);

      // Check for compilation errors
      int status;
      GL.GetShader(shaderHandle, ShaderParameter.CompileStatus, out status);
      if (status == 0)
      {
        var infoLog = GL.GetShaderInfoLog(shaderHandle);
        Debug.Print("Compile failed for shader {0}: {1}", name, infoLog);
        Debug.Assert(false, infoLog);
      }
      return shaderHandle;
    }


    #endregion


    int CreateAndLinkProgram(params int?[] shaders)
    {
      // Create program, attach shaders, link
      var programHandle = GL.CreateProgram();
      foreach (var shader in shaders)
        if (shader != null)
          GL.AttachShader(programHandle, shader.Value);
      GL.LinkProgram(programHandle);

      // Check for link errors
      int status;
      GL.GetProgram(programHandle, GetProgramParameterName.LinkStatus, out status);
      if (status == 0)
      {
        var infoLog = GL.GetProgramInfoLog(programHandle);
        Debug.Print("Link for shader program {0} failed: {1}", programHandle, infoLog);
        Debug.Assert(false);
      }

      // Delete shaders
      foreach (var shader in shaders)
        if (shader != null)
          GL.DeleteShader(shader.Value);
      this.ProgramHandle = programHandle;
      UpdateMaterialFromSettings();
      var error = GL.GetError();
      if (error != ErrorCode.NoError)
        Debug.Print("OpenGL error (CreateAndLinkProgram): " + error.ToString());

      return programHandle;
    }
    #endregion Shaders and Programs management

    public Color Color1 { get; set; }
    public Color Color2 { get; set; }

    public float Size { get; set; }
    public float AjustedDeformationSize { get; set; }

    public MaterialStruct Material;

    public void UpdateMaterialFromSettings()
    {
      if (Parent == null) return;
      Material = Parent.RenderSettings.ConvertIntoGLMaterialStruct();
      Material.Size = Size;
      Material.Visible = true;
    }

    private void RenderSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      UpdateMaterialFromSettings();
    }

    /// <summary>
    /// Called on OnRenderFrame
    /// </summary>
    virtual public void OnRenderObject(Matrix4 mvpMatrix, Matrix4 viewMatrix)
    {
      GL.UseProgram(ProgramHandle);
      GL.BindVertexArray(VertexArrayObject);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);
      var error = GL.GetError();
      if (error != ErrorCode.NoError)
        Debug.Print("OpenGL error (OnRenderObject 1): " + error.ToString());

      GL.UniformMatrix4(MvpUniformLocation, false, ref mvpMatrix);
      GL.UniformMatrix4(ViewUniformLocation, false, ref viewMatrix);
      GL.Uniform4(Color1UniformLocation, new Color4(Color1.R, Color1.G, Color1.B, Color1.A));
      GL.Uniform4(Color2UniformLocation, new Color4(Color2.R, Color2.G, Color2.B, Color2.A));
      GL.Uniform1(SizeUniformLocation, AjustedDeformationSize);
      error = GL.GetError();
      if (error != ErrorCode.NoError)
        Debug.Print("OpenGL error (OnRenderObject 2): " + error.ToString());
      if (Parent != null)
      {
        GL.Uniform1(GammaUniformLocation, Parent.RenderSettings.Gamma);
        error = GL.GetError();
        if (error != ErrorCode.NoError)
          Debug.Print("OpenGL error (OnRenderObject 2-1): " + error.ToString() + " (location:"+GammaUniformLocation+")");
        if (ObjectDataUniform != -1)
          Material.SetUniforms(ObjectDataUniform);
        error = GL.GetError();
        if (error != ErrorCode.NoError)
          Debug.Print("OpenGL error (OnRenderObject 2-2): " + error.ToString() + " (location:" + ObjectDataUniform + ")");
        //Material.SetUniforms(ObjectDataUniform);
      }
      error = GL.GetError();
      if (error != ErrorCode.NoError)
        Debug.Print("OpenGL error (OnRenderObject 3): " + error.ToString());

      GL.DrawElements(PrimitiveType.Triangles, ElementCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
      error = GL.GetError();
      if (error != ErrorCode.NoError)
        Debug.Print("OpenGL error (OnRenderObject 4): " + error.ToString());

      GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
      GL.BindVertexArray(0);
      GL.UseProgram(0);
      error = GL.GetError();
      if (error != ErrorCode.NoError)
        Debug.Print("OpenGL error (OnRenderObject 5): " + error.ToString());

      //if (DisplayNormals)
      //{
      //  GL.UseProgram(NormalProgramHandle);
      //  GL.BindVertexArray(VertexArrayObject);
      //  GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);

      //  GL.Uniform1(NormalUniformLocationNormalLength, NormalLength);
      //  GL.UniformMatrix4(NormalUniformLocationMvpMatrix, false, ref MvpMatrix);
      //  GL.DrawElements(PrimitiveType.Triangles, ElementCount, DrawElementsType.UnsignedInt, IntPtr.Zero);

      //  GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
      //  GL.BindVertexArray(0);
      //  GL.UseProgram(0);
      //}
    }

    /// <summary>
    /// 
    /// </summary>
    virtual public void OnKeyPressed()
    {

    }

    int VertexArrayObject;
    int VertexBuffer, NormalBuffer, ElevationBuffer, IndexBuffer;
    static byte[] GetRandomFragmentShader()
    {
      return Resources.Lighting_frag;
      //int randomFrag = rnd.Next(3);
      //switch (randomFrag)
      //{
      //	case 0: return "Explosion_Frag";
      //	case 1: return "Explosion2_frag";
      //	case 2: return "Explosion3_frag";
      //}
      //return "";
    }

    static byte[] GetRandomVertexShader()
    {
      return Resources.Lighting_vert;
      //int randomVert = rnd.Next(2);
      //switch (randomVert)
      //{
      //	case 0: return "Explosion_Vert";
      //	case 1: return "Explosion2_vert";
      //}
      //return "";
    }

    static Color GetRandomColor()
    {
      switch (rnd.Next(10))
      {
        case 0: return Color.Red;
        case 1: return Color.Blue;
        case 2: return Color.Black;
        case 3: return Color.Yellow;
        case 4: return Color.Green;
        case 5: return Color.Cyan;
        case 6: return Color.Indigo;
        case 7: return Color.White;
        case 8: return Color.Tomato;
        case 9: return Color.LawnGreen;
        default: return Color.Gray;
      }
    }

    //Random rnd = new Random();
    public static OpenGLObject CreateObject(float px, float py, float pz, float radius, RenderWindowBase parent)
    {
      OpenGLObject openGLObject = null;
      if (rnd.Next(2) == 1)
        openGLObject = new SphereObject(new Vector3(px, py, pz), radius * 2, true);
      else
        openGLObject = new CubeObject(new Vector3(px, py, pz), radius * 2, true);
      openGLObject.Parent = parent;
      openGLObject.Color1 = GetRandomColor();
      do
      {
        openGLObject.Color2 = GetRandomColor();
      } while (openGLObject.Color2 == openGLObject.Color1);
      openGLObject.LoadShaders(GetRandomFragmentShader(), GetRandomVertexShader(), null);
      return openGLObject;
    }

    public static OpenGLObject CreateTeapot(float px, float py, float pz, float radius, RenderWindowBase parent)
    {
      OpenGLObject openGLObject = null;
      openGLObject = new TeaPotObject();
      openGLObject.Parent = parent;
      openGLObject.Color1 = GetRandomColor();
      do
      {
        openGLObject.Color2 = GetRandomColor();
      } while (openGLObject.Color2 == openGLObject.Color1);
      openGLObject.LoadShaders(GetRandomFragmentShader(), GetRandomVertexShader(), null);
      return openGLObject;
    }

    void SetupBuffers()
    {
      // Generate VBOs
      VertexBuffer = GL.GenBuffer();
      NormalBuffer = GL.GenBuffer();
      if (WithNoise)
        ElevationBuffer = GL.GenBuffer();
      IndexBuffer = GL.GenBuffer();

      // positions
      GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBuffer);
      GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Positions.Length * Vector3.SizeInBytes), Positions, BufferUsageHint.StaticDraw);
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

      // normals, no data for now
      GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBuffer);
      GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Normals.Length * Vector3.SizeInBytes), IntPtr.Zero, BufferUsageHint.StreamDraw);
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

      // elevation, no data for now
      if (WithNoise)
      {
        GL.BindBuffer(BufferTarget.ArrayBuffer, ElevationBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Positions.Length * sizeof(float)), IntPtr.Zero, BufferUsageHint.StreamDraw);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      }

      // indices
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);
      GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(Indices.Length * sizeof(int)), Indices, BufferUsageHint.StaticDraw);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

      // Create and set up VAO
      VertexArrayObject = GL.GenVertexArray();
      GL.BindVertexArray(VertexArrayObject);
      {
        // positions, located at attribute index 0
        GL.EnableVertexAttribArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBuffer);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        // normals, located at atteibute location 1
        GL.EnableVertexAttribArray(1);
        GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBuffer);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);

        // elevation, located at attribute index 2
        if (WithNoise)
        {
          GL.EnableVertexAttribArray(2);
          GL.BindBuffer(BufferTarget.ArrayBuffer, ElevationBuffer);
          GL.VertexAttribPointer(2, 1, VertexAttribPointerType.Float, false, 0, 0);
        }
      }
      GL.BindVertexArray(0);
    }
  }
}
