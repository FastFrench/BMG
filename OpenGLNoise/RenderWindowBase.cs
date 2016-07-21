
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
using System.Windows.Forms;
using OpenGLNoise.Components;
using OpenGLNoise.Components.Lights;
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
		private float Gamma { get { if (RenderSettings != null) return RenderSettings.Gamma; return 2.2f; } }
		#region Array of Lights data
		int LightsBufferUBO; // Lights: Location for the UBO given by OpenGL
		int SettingsBufferUBO; // Global Settings: Location for the UBO given by OpenGL
		public const int LIGHTS_BUFFER_INDEX = 0; // Lights : Index to use for the buffer binding (All good things start at 0 )
		public const int SETTINGS_BUFFER_INDEX = 1; // Global Settings : Index to use for the buffer binding (All good things start at 0 )
		LightCollectionStruct LightsUBOData;
    public SettingsStruct GlobalSettingsStruct;
		void InitLightAndSettingsBuffer()
		{
			LightsUBOData = RenderSettings.Lights.ConvertIntoGLStruct(Gamma); // Create actual data
			GL.GenBuffers(1, out LightsBufferUBO); // Generate the buffer
			GL.BindBuffer(BufferTarget.UniformBuffer, LightsBufferUBO); // Bind the buffer for writing
			GL.BufferData(BufferTarget.UniformBuffer, (IntPtr)(LightsUBOData.Size(true)), (IntPtr)(null), BufferUsageHint.DynamicDraw); // Request the memory to be allocated
			GL.BindBufferRange(BufferRangeTarget.UniformBuffer, LIGHTS_BUFFER_INDEX, LightsBufferUBO, (IntPtr)0, (IntPtr)(LightsUBOData.Size(true))); // Bind the created Uniform Buffer to the Buffer Index			
			GL.GenBuffers(1, out SettingsBufferUBO); // Generate the buffer
			GL.BindBuffer(BufferTarget.UniformBuffer, SettingsBufferUBO); // Bind the buffer for writing
			GL.BufferData(BufferTarget.UniformBuffer, (IntPtr)(Marshal.SizeOf<SettingsStruct>()), (IntPtr)(null), BufferUsageHint.DynamicDraw); // Request the memory to be allocated
																																				//GL.BindBuffer(BufferTarget.UniformBuffer, SettingsBufferUBO); // Bind the created Uniform Buffer to the Buffer Index
			GL.BindBufferRange(BufferRangeTarget.UniformBuffer, SETTINGS_BUFFER_INDEX, SettingsBufferUBO, (IntPtr)0, (IntPtr)(Marshal.SizeOf<SettingsStruct>())); // Bind the created Uniform Buffer to the Buffer Index			
		}

		byte[] ObjectToByteArray(object obj)
		{
			byte[] arr = null;
			IntPtr ptr = IntPtr.Zero;
			try
			{
				int size = Marshal.SizeOf(obj);
				arr = new byte[size];
				ptr = Marshal.AllocHGlobal(size);
				Marshal.StructureToPtr(obj, ptr, true);
				Marshal.Copy(ptr, arr, 0, size);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
			finally
			{
				Marshal.FreeHGlobal(ptr);
			}

			return arr;
		}

		void FillLightsUniformBuffer()
		{
			LightsUBOData = RenderSettings.Lights.ConvertIntoGLStruct(Gamma); // Create actual data
			GL.BindBuffer(BufferTarget.UniformBuffer, LightsBufferUBO);
			var firstPart = ObjectToByteArray(LightsUBOData.GlobalData);
			GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)0, firstPart.Length, firstPart);
			GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)Marshal.SizeOf<Vector4>(), (IntPtr)(LightsUBOData.GlobalData.NbLights * Marshal.SizeOf<LightStruct>()), LightsUBOData.Lights);
			GL.BindBuffer(BufferTarget.UniformBuffer, 0);
		}
		#endregion Array of Lights data
		void UpdateAndFillGlobalSettingsUniformBuffer(FrameEventArgs e)
		{
			// Rotate the plane
			if (!RenderSettings.Paused)
			{
				var modelRotation = Matrix4.CreateRotationZ((float)(e.Time * 0.5));
				Matrix4.Mult(ref ModelMatrix, ref modelRotation, out ModelMatrix);
			}
			// Update MVP matrix
			Matrix4 mvMatrix; // MVP: Model * View * Projection
			Matrix4.Mult(ref ModelMatrix, ref GlobalSettingsStruct.View, out mvMatrix);
			Matrix4.Mult(ref mvMatrix, ref ProjectionMatrix, out GlobalSettingsStruct.MVP);
			if (!RenderSettings.Paused)
				GlobalSettingsStruct.Time += (float)e.Time;

			var data = ObjectToByteArray(GlobalSettingsStruct);

			GL.BindBuffer(BufferTarget.UniformBuffer, SettingsBufferUBO);
			GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)0, (IntPtr)(Marshal.SizeOf<SettingsStruct>()), data);
			GL.BindBuffer(BufferTarget.UniformBuffer, 0);
		}

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

		//protected Matrix4 ModelMatrix, ProjectionMatrix;
		protected Matrix4 ModelMatrix, ViewMatrix, ProjectionMatrix, MvpMatrix;

		protected List<OpenGLObject> Objects { get; set; }

		protected Random rnd = new Random();

		protected void RemoveAnObject()
		{
			if (Objects.Count == 0) return;
			var removed = Objects.Last(obj => !(obj is LightObject));
			if (removed == null) return;
			removed.Dispose();
			Objects.Remove(removed);

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

		//virtual protected void BuildObjects()
		//{
		//	// Create sphere data and set up buffers
		//	foreach (var obj in Objects)
		//		obj.BuildObject();
		//	//CreateVertexData();
		//}


		protected virtual void AddObject(float px, float py, float pz, float radius)
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

			InitLightAndSettingsBuffer();
			// Add lights
			UpdateLights(false);
		}

		/// <summary>
		/// We remove all light objects and recreate them
		/// </summary>
		/// <param name="buildThem"></param>
		private void UpdateLights(bool buildThem = true)
		{
			if (!needToUpdateLights) return;
			needToUpdateLights = false;
			foreach (var light in Objects.OfType<LightObject>().ToArray())
			{
				Objects.Remove(light);
				light.Dispose();
			}

			FillLightsUniformBuffer();
			foreach (var light in RenderSettings.Lights)
				if (light.Visible)
					Objects.Add(OpenGLObject.CreateLight(light.Position, light.GlobalColor));
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			#region DEBUG
			/// DEBUG
			MakeCurrent();
			GL.DebugMessageCallback(DebugCallbackInstance, IntPtr.Zero);
			#endregion DEBUG

			CreateObjects();
			//BuildObjects();
			frames = 0;

			//renderer = new TextRenderer(Width, Height);
			PointF position = PointF.Empty;

			GL.ClearColor(Color4.Gray);

			//TargetRenderFrequency = 100;
			//TargetUpdateFrequency = 100;
			// Initialize model and view matrices once
			ViewMatrix = GlobalSettingsStruct.View = Matrix4.LookAt(new Vector3(7, 0, 0), Vector3.Zero, Vector3.UnitZ);
			ModelMatrix = Matrix4.CreateScale(1.0f);

		}
		protected override void OnKeyPress(OpenTK.KeyPressEventArgs e)
		{
			//if (e.KeyChar == ' ')
			//	DisplayNormals = !DisplayNormals;
			//Thread.Sleep(2000);
			switch (e.KeyChar)
			{
				case '+':
					AddARandomObject();
					//BuildObjects();
					break;
				case '-':
					RemoveAnObject();
					//BuildObjects();
					break;
				case ' ':
					RenderSettings.Bouncing = !RenderSettings.Bouncing;
					break;
				default:
					break;
			}
			foreach (var obj in Objects)
				obj.OnKeyPressed(e);
			base.OnKeyPress(e);
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
			UpdateLights();
			computeFPS(e);
			FillLightsUniformBuffer();
			if (!RenderSettings.Paused && Bouncing)
			{
				if (sign)
					alt += (float)e.Time;
				else
					alt -= (float)e.Time;
				if (alt > 15 || alt < 3) sign = !sign;
			}
			ViewMatrix = GlobalSettingsStruct.View = Matrix4.LookAt(new Vector3(0, alt, 0), new Vector3(0, 0, 0), Vector3.UnitZ);

			foreach (var obj in Objects)
				obj.OnUpdateObject(e);

			UpdateAndFillGlobalSettingsUniformBuffer(e);

			if (!RenderSettings.Paused)
			{
				// New way
				// Old obsolete way
				// Rotate the plane
				//var modelRotation = Matrix4.CreateRotationZ((float)(e.Time * 0.5));
				//Matrix4.Mult(ref ModelMatrix, ref modelRotation, out ModelMatrix);

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
			//obj.OnRenderObject(GlobalSettingsStruct.MVP, GlobalSettingsStruct.View);


			SwapBuffers();
		}

		public RenderWindowSettings RenderSettings { get; set; }

		public RenderWindowBase(RenderWindowSettings settings)
			  : base(800, 600, GraphicsMode.Default, "SharpNoise OpenGL Example",
			  GameWindowFlags.Default, DisplayDevice.Default, 3, 3, GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug)
		{
			RenderSettings = settings;
			RenderSettings.Lights.ListChanged += Lights_ListChanged;
			VSync = VSyncMode.Off;
		}


		#region DEBUG
		// The callback delegate must be stored to avoid GC
		DebugProc DebugCallbackInstance = DebugCallback;

		static void DebugCallback(DebugSource source, DebugType type, int id,
		  DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
		{
			string msg = Marshal.PtrToStringAnsi(message);
			Console.WriteLine("[GL] {0}; {1}; {2}; {3}; {4}",
			  source, type, id, severity, msg);
		}
		#endregion DEBUG

		bool needToUpdateLights = true;
		private void Lights_ListChanged(object sender, ListChangedEventArgs e)
		{
			needToUpdateLights = true;

		}
	}
}
