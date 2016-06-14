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

		int SphereProgramHandle;
		Matrix4 ModelMatrix, ViewMatrix, ProjectionMatrix, MvpMatrix;
		int MvpUniformLocation;

		int NormalProgramHandle;
		float NormalLength = 0.5f;
		int NormalUniformLocationNormalLength, NormalUniformLocationMvpMatrix;

		int VertexArrayObject;
		int VertexBuffer, NormalBuffer, ElevationBuffer, IndexBuffer;
		int ElementCount;

		Vector3[] Positions, Normals;
		int[] Indices;

		TranslatePoint TimeTranslator;
		NoiseMap NoiseMap;
		PlaneNoiseMapBuilder NoiseMapBuilder;

		#region Shader Loading

		int LoadShaderFromResource(ShaderType shaderType, string resourceName)
		{
			// Load shader from WinForms resource manager thing
			var shaderBytes = (byte[])Resources.ResourceManager.GetObject(resourceName);
			var shaderSource = Encoding.UTF8.GetString(shaderBytes);

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
				Debug.Print("Compile failed for shader {0}: {1}", resourceName, infoLog);
			}
			return shaderHandle;
		}

		int CreateAndLinkProgram(params int[] shaders)
		{
			// Create program, attach shaders, link
			var programHandle = GL.CreateProgram();
			foreach (var shader in shaders)
				GL.AttachShader(programHandle, shader);
			GL.LinkProgram(programHandle);

			// Check for link errors
			int status;
			GL.GetProgram(programHandle, GetProgramParameterName.LinkStatus, out status);
			if (status == 0)
			{
				var infoLog = GL.GetProgramInfoLog(programHandle);
				Debug.Print("Link for shader program {0} failed: {1}", programHandle, infoLog);
			}

			// Delete shaders
			foreach (var shader in shaders)
				GL.DeleteShader(shader);
			return programHandle;
		}

		#endregion

		#region Buffer setup

		List<Vector3> positions;
		List<Vector3> normals;
		List<int> indices;

		void CreateSphereData(float x, float y, float z, float radius)
		{
			int i_basis = indices.Count;
			for (double latitudeNum = 0; latitudeNum <= LatitudeBands; latitudeNum++)
			{
				var theta = latitudeNum * MathHelper.Pi / LatitudeBands;
				var sinTheta = Math.Sin(theta);
				var cosTheta = Math.Cos(theta);
				Vector3 p0 = new Vector3(x, y, z);
				for (double longitudeNum = 0; longitudeNum <= LongitudeBands; longitudeNum++)
				{
					var phi = longitudeNum * MathHelper.TwoPi / LongitudeBands;
					var sinPhi = Math.Sin(phi);
					var cosPhi = Math.Cos(phi);

					var px = cosPhi * sinTheta;
					var py = sinPhi * sinTheta;
					var pz = cosTheta;

					var normal = new Vector3((float)px, (float)py, (float)pz);
					var position = (normal + p0) * radius;
					normals.Add(normal);
					positions.Add(position);
				}
			}

			for (int latitudeNum = 0; latitudeNum < LatitudeBands; latitudeNum++)
			{
				for (int longitudeNum = 0; longitudeNum <= LongitudeBands; longitudeNum++)
				{
					var i0 = i_basis + (latitudeNum * (LongitudeBands + 1)) + longitudeNum;
					var i1 = i_basis + i0 + LongitudeBands + 1;

					indices.Add(i0);
					indices.Add(i1);
					indices.Add(i0 + 1);

					indices.Add(i1);
					indices.Add(i1 + 1);
					indices.Add(i0 + 1);
				}
			}
		}

		void CreateVertexData()
		{
			positions = new List<Vector3>();
			normals = new List<Vector3>();
			indices = new List<int>();

			CreateSphereData(6f, 0f, 0, SphereRadius);

			//CreateSphereData(0.5f, 0f, 0.5f, 0.5f+SphereRadius);

			Positions = positions.ToArray();
			Normals = normals.ToArray();
			Indices = indices.ToArray();
			ElementCount = Indices.Length;
		}

		void SetupBuffers()
		{
			// Generate VBOs
			VertexBuffer = GL.GenBuffer();
			NormalBuffer = GL.GenBuffer();
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
			GL.BindBuffer(BufferTarget.ArrayBuffer, ElevationBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Positions.Length * sizeof(float)), IntPtr.Zero, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

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
				GL.EnableVertexAttribArray(2);
				GL.BindBuffer(BufferTarget.ArrayBuffer, ElevationBuffer);
				GL.VertexAttribPointer(2, 1, VertexAttribPointerType.Float, false, 0, 0);
			}
			GL.BindVertexArray(0);
		}

		#endregion

		void UpdateTriangleNormals(Vector3[] positions, int[] indices, float[] elevation)
		{
			// Do nothing
		}

		void GenerateElevationNoise(double timeDelta)
		{
			// Shift time and generate noise
			TimeTranslator.YTranslation += timeDelta;
			NoiseMapBuilder.Build();
		}

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
			NoiseMapBuilder.SetBounds(0, LongitudeBands, 0, LatitudeBands);
			NoiseMapBuilder.SetDestSize(LongitudeBands, LatitudeBands);
		}

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
		double t0 = 0.0;
		int frames = 0;
		double t = 0.0;
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

		protected override void OnLoad(EventArgs e)
		{
			t0 = 0.0;
			frames = 0;

			renderer = new TextRenderer(Width, Height);
			PointF position = PointF.Empty;

			renderer.Clear(Color.MidnightBlue);
			renderer.DrawString("The quick brown fox jumps over the lazy dog", serif, Brushes.White, position);
			position.Y += serif.Height;
			renderer.DrawString("The quick brown fox jumps over the lazy dog", sans, Brushes.White, position);
			position.Y += sans.Height;
			renderer.DrawString("The quick brown fox jumps over the lazy dog", mono, Brushes.White, position);
			position.Y += mono.Height;


			GL.ClearColor(Color4.Gray);

			GL.Enable(EnableCap.DepthTest);

			// Load sphere shader
			var vertexHandle = LoadShaderFromResource(ShaderType.VertexShader, "Explosion_Vert"/*"sphere_vert"*/);
			var fragmentHandle = LoadShaderFromResource(ShaderType.FragmentShader, "Explosion_Frag"/*"sphere_frag"*/);
			SphereProgramHandle = CreateAndLinkProgram(vertexHandle, fragmentHandle);
			MvpUniformLocation = GL.GetUniformLocation(SphereProgramHandle, "MVP");

			// Load normal shader
			var normalVertexHandle = LoadShaderFromResource(ShaderType.VertexShader, "normals_vert");
			var normalGeometryHandle = LoadShaderFromResource(ShaderType.GeometryShader, "normals_geom");
			var normalFragmentHandle = LoadShaderFromResource(ShaderType.FragmentShader, "normals_frag");
			NormalProgramHandle = CreateAndLinkProgram(normalVertexHandle, normalGeometryHandle, normalFragmentHandle);
			NormalUniformLocationMvpMatrix = GL.GetUniformLocation(NormalProgramHandle, "MVP");
			NormalUniformLocationNormalLength = GL.GetUniformLocation(NormalProgramHandle, "NormalLength");

			// Create sphere data and set up buffers
			CreateVertexData();
			SetupBuffers();

			TargetRenderFrequency = 50;
			TargetUpdateFrequency = 50;
			// Initialize model and view matrices once
			ViewMatrix = Matrix4.LookAt(new Vector3(7, 0, 0), Vector3.Zero, Vector3.UnitZ);
			ModelMatrix = Matrix4.CreateScale(1.0f);

			// Set up noise module
			SetupNoiseMapBuilder();
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

			// Update elevation data
			GenerateElevationNoise(e.Time);
			GL.BindBuffer(BufferTarget.ArrayBuffer, ElevationBuffer);
			GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr)(NoiseMap.Data.Length * sizeof(float)), NoiseMap.Data);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			// Update normals
			UpdateTriangleNormals(Positions, Indices, NoiseMap.Data);
			GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBuffer);
			GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr)(Normals.Length * Vector3.SizeInBytes), Normals);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			var error = GL.GetError();
			if (error != ErrorCode.NoError)
				Debug.Print("OpenGL error: " + error.ToString());

			// Rotate the plane
			var modelRotation = Matrix4.CreateRotationZ((float)(e.Time * 0.5));
			Matrix4.Mult(ref ModelMatrix, ref modelRotation, out ModelMatrix);

			// Update MVP matrix
			Matrix4 mvMatrix;
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

			GL.UseProgram(SphereProgramHandle);
			GL.BindVertexArray(VertexArrayObject);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);

			GL.UniformMatrix4(MvpUniformLocation, false, ref MvpMatrix);
			GL.DrawElements(PrimitiveType.Triangles, ElementCount, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.UseProgram(0);

			if (DisplayNormals)
			{
				GL.UseProgram(NormalProgramHandle);
				GL.BindVertexArray(VertexArrayObject);
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);

				GL.Uniform1(NormalUniformLocationNormalLength, NormalLength);
				GL.UniformMatrix4(NormalUniformLocationMvpMatrix, false, ref MvpMatrix);
				GL.DrawElements(PrimitiveType.Triangles, ElementCount, DrawElementsType.UnsignedInt, IntPtr.Zero);

				GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
				GL.BindVertexArray(0);
				GL.UseProgram(0);
			}

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
