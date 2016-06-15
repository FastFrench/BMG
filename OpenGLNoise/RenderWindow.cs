
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
	public class RenderWindow : GameWindow
	{
		TextRenderer renderer;
		Font serif = new Font(FontFamily.GenericSerif, 24);
		Font sans = new Font(FontFamily.GenericSansSerif, 24);
		Font mono = new Font(FontFamily.GenericMonospace, 24);


		const int LatitudeBands = 50;
		const int LongitudeBands = 100;
		const float SphereRadius = 0.40f;//1.6f;

		bool DisplayNormals = false;

		//int SphereProgramHandle;
		Matrix4 ModelMatrix, ViewMatrix, ProjectionMatrix, MvpMatrix;
		
		//int NormalProgramHandle;
		//float NormalLength = 0.5f;
		//int NormalUniformLocationNormalLength, NormalUniformLocationMvpMatrix;

		//int VertexArrayObject;
		//int VertexBuffer, NormalBuffer, ElevationBuffer, IndexBuffer;
		//int ElementCount;

		//Vector3[] Positions, Normals;
		//int[] Indices;


		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if (e.KeyChar == ' ')
				DisplayNormals = !DisplayNormals;

			base.OnKeyPress(e);
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
				renderer.Clear(Color.Black);
				renderer.DrawString(titlestring, serif, Brushes.White, new PointF(0.0f, 0.0f));
				chrono.Restart();
				//t0 = t;
				frames = 0;
			}
			frames++;
			return fps;
		}

    void BuildObjects()
    {
      // Create sphere data and set up buffers
      foreach (var obj in Objects)
        obj.BuildObject();
      //CreateVertexData();
    }

    List<OpenGLObject> Objects { get; set; }
		protected override void OnLoad(EventArgs e)
		{
      Objects = new List<OpenGLObject>();
      var sphere1 = new SphereObject(0, 0, SphereRadius);
      sphere1.LoadShaders(/*"Explosion_Frag"*/"sphere_frag", /*"Explosion_Vert"*/"sphere_vert", null);
      Objects.Add(new SphereObject(0, 0, SphereRadius));

      frames = 0;

			renderer = new TextRenderer(Width, Height);
			PointF position = PointF.Empty;

      GL.ClearColor(Color4.Gray);

      GL.Enable(EnableCap.DepthTest);

   //   renderer.Clear(Color.MidnightBlue);
			//renderer.DrawString("The quick brown fox jumps over the lazy dog", serif, Brushes.White, position);
			//position.Y += serif.Height;
			//renderer.DrawString("The quick brown fox jumps over the lazy dog", sans, Brushes.White, position);
			//position.Y += sans.Height;
			//renderer.DrawString("The quick brown fox jumps over the lazy dog", mono, Brushes.White, position);
			//position.Y += mono.Height;


			 
      
			// Load sphere shader
			//var vertexHandle = LoadShaderFromResource(ShaderType.VertexShader, "Explosion_Vert"/*"sphere_vert"*/);
			//var fragmentHandle = LoadShaderFromResource(ShaderType.FragmentShader, "Explosion_Frag"/*"sphere_frag"*/);
			//SphereProgramHandle = CreateAndLinkProgram(vertexHandle, fragmentHandle);
			//MvpUniformLocation = GL.GetUniformLocation(SphereProgramHandle, "MVP");

      // Load normal shader
      //var normalVertexHandle = LoadShaderFromResource(ShaderType.VertexShader, "normals_vert");
      //var normalGeometryHandle = LoadShaderFromResource(ShaderType.GeometryShader, "normals_geom");
      //var normalFragmentHandle = LoadShaderFromResource(ShaderType.FragmentShader, "normals_frag");
      //NormalProgramHandle = CreateAndLinkProgram(normalVertexHandle, normalGeometryHandle, normalFragmentHandle);
      //NormalUniformLocationMvpMatrix = GL.GetUniformLocation(NormalProgramHandle, "MVP");
      //NormalUniformLocationNormalLength = GL.GetUniformLocation(NormalProgramHandle, "NormalLength");

      BuildObjects();
      
			TargetRenderFrequency = 50;
			TargetUpdateFrequency = 50;
			// Initialize model and view matrices once
			ViewMatrix = Matrix4.LookAt(new Vector3(7, 0, 0), Vector3.Zero, Vector3.UnitZ);
			ModelMatrix = Matrix4.CreateScale(1.0f);

		}

		protected override void OnUnload(EventArgs e)
		{
			renderer.Dispose();
			base.OnUnload(e);
		}

		static float alt = 4;
		bool sign = true;
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			computeFPS(e);
			if (sign)
				alt += 0.03f;
			else
				alt -= 0.03f;
			if (alt > 15 || alt < 3) sign = !sign;
			ViewMatrix = Matrix4.LookAt(new Vector3(alt, 0, 0), Vector3.Zero, Vector3.UnitZ);

      foreach (var obj in Objects)
        obj.OnUpdateObject(e);

			// Rotate the plane
			var modelRotation = Matrix4.CreateRotationZ((float)(e.Time * 0.5));
			Matrix4.Mult(ref ModelMatrix, ref modelRotation, out ModelMatrix);

			// Update MVP matrix
			Matrix4 mvMatrix; // MVP: Model * View * Projection
			Matrix4.Mult(ref ModelMatrix, ref ViewMatrix, out mvMatrix);
			Matrix4.Mult(ref mvMatrix, ref ProjectionMatrix, out MvpMatrix);



			//GL.Enable(EnableCap.Texture2D);
			//GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDst.OneMinusSourceAlpha);

			//GL.Enable(EnableCap.Texture2D);
			//GL.BindTexture(TextureTarget.Texture2D, renderer.Texture);
			//GL.Begin(BeginMode.Quads);

			//GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-1f, -1f);
			//GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1f, -1f);
			//GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1f, 1f);
			//GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-1f, 1f);

			//GL.End();

			//SwapBuffers();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			// Update your text
			renderer.Clear(Color.Black);
			//renderer.DrawString("Hello, world", serif, Brushes.White, new PointF(0.0f, 0.0f));

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


      foreach (var obj in Objects)
        obj.OnRenderObject(MvpMatrix);
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

		public RenderWindow()
			: base(800, 600, GraphicsMode.Default, "SharpNoise OpenGL Example",
			GameWindowFlags.Default, DisplayDevice.Default, 3, 3, GraphicsContextFlags.ForwardCompatible)
		{
			VSync = VSyncMode.Off;
		}
	}
}
