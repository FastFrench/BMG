using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGLNoise.Properties;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SharpNoise;
using SharpNoise.Builders;
using SharpNoise.Modules;

namespace OpenGLNoise
{
	public abstract class OpenGLObject : IDisposable
	{
		protected OpenGLObject(float size = 1.0f, Color? color1 = null, Color? color2= null)
		{
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

		protected Random rnd = new Random();
		
		/// <summary>
		/// Initial building of the object (usually called in OnLoad or chen the object appears)
		/// Base class method should be called last
		/// </summary>
		virtual public void BuildObject()
		{
			SetupBuffers();
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

		protected int SizeUniformLocation { get; set; }
		protected int Color1UniformLocation { get; set; }
		protected int Color2UniformLocation { get; set; }

		void GenerateElevationNoise(double timeDelta)
		{
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
		/// <summary>
		/// Called on OnUpdateFrame
		/// </summary>
		virtual public void OnUpdateObject(FrameEventArgs e)
		{
			// Update elevation data
			GenerateElevationNoise(e.Time);
			GL.BindBuffer(BufferTarget.ArrayBuffer, ElevationBuffer);
			GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr)(NoiseMap.Data.Length * sizeof(float)), NoiseMap.Data);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			// Update normals
			GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBuffer);
			GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr)(Normals.Length * Vector3.SizeInBytes), Normals);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			var error = GL.GetError();
			if (error != ErrorCode.NoError)
				Debug.Print("OpenGL error: " + error.ToString());

		}

		#region Shaders and Programs management
		#region Shader Loading

		public void LoadShaders(string pixelShaderName, string vertexShaderName, string geometryShaderName = null)
		{
			int? pixelShader = null;
			int? vertexShader = null;
			int? geometryShader = null;
			if (pixelShaderName != null)
				pixelShader = LoadShaderFromResource(ShaderType.FragmentShader, pixelShaderName);
			if (vertexShaderName != null)
				vertexShader = LoadShaderFromResource(ShaderType.VertexShader, vertexShaderName);
			if (geometryShaderName != null)
				geometryShader = LoadShaderFromResource(ShaderType.GeometryShader, geometryShaderName);
			CreateAndLinkProgram(pixelShader, vertexShader, geometryShader);
			MvpUniformLocation = GL.GetUniformLocation(ProgramHandle, "MVP");
			SizeUniformLocation = GL.GetUniformLocation(ProgramHandle, "Size");
			Color1UniformLocation = GL.GetUniformLocation(ProgramHandle, "GlobalColor1");
			Color2UniformLocation = GL.GetUniformLocation(ProgramHandle, "GlobalColor2");

			Debug.Assert(MvpUniformLocation != -1);
		}

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
			}

			// Delete shaders
			foreach (var shader in shaders)
				if (shader != null)
					GL.DeleteShader(shader.Value);
			this.ProgramHandle = programHandle;
			return programHandle;
		}
		#endregion Shaders and Programs management

		public Color Color1 { get; set; }
		public Color Color2 { get; set; }

		public float Size { get; set; }

		/// <summary>
		/// Called on OnRenderFrame
		/// </summary>
		virtual public void OnRenderObject(Matrix4 mvpMatrix)
		{
			GL.UseProgram(ProgramHandle);
			GL.BindVertexArray(VertexArrayObject);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);

			GL.UniformMatrix4(MvpUniformLocation, false, ref mvpMatrix);
			GL.Uniform4(Color1UniformLocation, new Vector4(Color1.R / 255.0f, Color1.G / 255.0f, Color1.B / 255.0f, Color1.A / 255.0f));
			GL.Uniform4(Color2UniformLocation, new Vector4(Color2.R / 255.0f, Color2.G / 255.0f, Color2.B / 255.0f, Color2.A / 255.0f));
			GL.Uniform1(SizeUniformLocation, Size);
			GL.DrawElements(PrimitiveType.Triangles, ElementCount, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.UseProgram(0);

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
	}
}
