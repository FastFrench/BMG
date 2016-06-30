using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using FreeType;

public class FreeTypeLineWiseTest : GameWindow
{

	private FtFont freetypeFont11;
	private FtFont freetypeFont12;
	private FtFont freetypeFont21;
	private FtFont freetypeFont22;
	private FtFont freetypeFont31;
	private FtFont freetypeFont32;
	private FtFont freetypeFont41;
	private FtFont freetypeFont42;
	private FtFont freetypeFont51;
	private FtFont freetypeFont52;
	private FtFont freetypeFont61;
	private FtFont freetypeFont62;
	private FtFont freetypeFont71;
	private FtFont freetypeFont72;

	float avgFPS;
	float avgCNT = 0.0F;
	string poem = "Hello out there!!!... Are you feeling fine?\n0123456789 $£€& ;W,w:@.@";

	public FreeTypeLineWiseTest ()
		: base(1200, 600, GraphicsMode.Default, "OpenTK Quick Start Sample")
	{
		VSync = VSyncMode.On;
		this.WindowBorder = WindowBorder.Resizable;
	}

	/// <summary>Load resources here.</summary>
	/// <param name="e">Not used.</param>
	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

		int size1 = 14; // 9
		int size2 = 30; // 23
		string fontRoot = "/usr/share/fonts/truetype/";

		//Create the fonts we want to use
		freetypeFont11 = new FtFont(fontRoot + "DroidSerif-Regular.ttf", size1); // "FreeSans.ttf"
		freetypeFont12 = new FtFont(fontRoot + "DroidSerif-Regular.ttf", size2);
		freetypeFont21 = new FtFont(fontRoot + "DejaVuSerif.ttf", size1);
		freetypeFont22 = new FtFont(fontRoot + "DejaVuSerif.ttf", size2);
		freetypeFont31 = new FtFont(fontRoot + "OpenSans-Regular.ttf", size1);
		freetypeFont32 = new FtFont(fontRoot + "OpenSans-Regular.ttf", size2);
		freetypeFont41 = new FtFont(fontRoot + "Tinos-Regular.ttf", size1);
		freetypeFont42 = new FtFont(fontRoot + "Tinos-Regular.ttf", size2);
		freetypeFont51 = new FtFont(fontRoot + "AnonymousPro-Regular.ttf", size1);
		freetypeFont52 = new FtFont(fontRoot + "AnonymousPro-Regular.ttf", size2);
		freetypeFont61 = new FtFont(fontRoot + "luxisr.ttf", size1);
		freetypeFont62 = new FtFont(fontRoot + "luxisr.ttf", size2);
		freetypeFont71 = new FtFont(fontRoot + "UbuntuMono-R.ttf", size1);
		freetypeFont72 = new FtFont(fontRoot + "UbuntuMono-R.ttf", size2);

		GL.ClearColor(1.0f, 1.0f, 1.0f, 0.0f);
		GL.Disable(EnableCap.DepthTest);
	}


	protected override void OnUnload(EventArgs e)
	{
		if (freetypeFont72 != null)
			freetypeFont72.Dispose ();
		if (freetypeFont71 != null)
			freetypeFont71.Dispose ();
		if (freetypeFont62 != null)
			freetypeFont62.Dispose ();
		if (freetypeFont61 != null)
			freetypeFont61.Dispose ();
		if (freetypeFont52 != null)
			freetypeFont52.Dispose ();
		if (freetypeFont51 != null)
			freetypeFont51.Dispose ();
		if (freetypeFont42 != null)
			freetypeFont42.Dispose ();
		if (freetypeFont41 != null)
			freetypeFont41.Dispose ();
		if (freetypeFont32 != null)
			freetypeFont32.Dispose ();
		if (freetypeFont31 != null)
			freetypeFont31.Dispose ();
		if (freetypeFont22 != null)
			freetypeFont22.Dispose ();
		if (freetypeFont21 != null)
			freetypeFont21.Dispose ();
		if (freetypeFont12 != null)
			freetypeFont12.Dispose ();
		if (freetypeFont11 != null)
			freetypeFont11.Dispose ();
	}

	/// <summary>Called if window is resized. Set the viewport here. It is also
	/// a good place to set up the projection matrix (which probably changes
	/// along when the aspect ratio of your window).</summary>
	/// <param name="e">Not used.</param>
	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);

		GL.MatrixMode(MatrixMode.Projection);
		GL.LoadIdentity();

		// Create aGDI-text-mode alike coordinate system for x and y coordinate (origin at left top corner) with positive
		// x-axis from left to right and positive y-axis from top to bottom.
		GL.MatrixMode(MatrixMode.Modelview);
		GL.LoadIdentity();
		GL.Ortho(0, Width, Height, 0, -1, 1);
		GL.Viewport(0, 0, Width, Height);
	}


	/// <summary>Called when it is time to render the next frame. Add your rendering code here.</summary>
	/// <param name="e">Contains timing information.</param>
	protected override void OnRenderFrame(FrameEventArgs e)
	{
		base.OnRenderFrame(e);

		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		// Rectangle gradient gray background.
		GL.Begin(PrimitiveType.Quads);
		GL.Color3(1.0f, 1.0f, 1.0f); GL.Vertex2(0, 0);
		GL.Color3(0.9f, 0.9f, 0.9f); GL.Vertex2(0, Height);
		GL.Color3(0.8f, 0.8f, 0.8f); GL.Vertex2(Width, Height);
		GL.Color3(0.9f, 0.9f, 0.9f); GL.Vertex2(Width, 0);
		GL.End();

		// Triangle gradient three-color background.
		GL.Begin(PrimitiveType.Triangles);
		GL.Color3(Color.FromArgb (255, 255, 128, 128));
		GL.Vertex3(Width/2, 10, 0);
		GL.Color3(Color.FromArgb (255, 128, 255, 128));
		GL.Vertex3(Width - 10, Height - 10, 0);
		GL.Color3(Color.FromArgb (255, 128, 128, 255));
		GL.Vertex3(10, Height - 10, 0);
		GL.End();

		float curFPS = (float)(1.0 / e.Time);
		if (avgCNT <= 10.0F)
			avgFPS = curFPS;
		else
			avgFPS += (curFPS - avgFPS) / avgCNT;
		avgCNT++;
		freetypeFont72.DrawString ("FPS average: " + avgFPS.ToString("F2") + " - FPS current: " + curFPS.ToString("F2"), 400, 0);

		GL.Color3 (26/255.0, 26/255.0, 26/255.0);
		freetypeFont11.DrawString (poem + " (Droid Serif)", 10, 40);
		freetypeFont12.DrawString (poem + " (Droid Serif)", 400, 40);
		GL.Color3 (26/255.0, 26/255.0, 128/255.0);
		freetypeFont21.DrawString (poem + " (DejaVu Serif)", 10, 120);
		freetypeFont22.DrawString (poem + " (DejaVu Serif)", 400, 120);
		GL.Color3 (128/255.0, 26/255.0, 26/255.0);
		freetypeFont31.DrawString (poem + " (Open Sans)", 10, 190);
		freetypeFont32.DrawString (poem + " (Open Sans)", 400, 190);
		GL.Color3 (26/255.0, 128/255.0, 26/255.0);
		freetypeFont41.DrawString (poem + " (Tinos)", 10, 270);
		freetypeFont42.DrawString (poem + " (Tinos)", 400, 270);
		GL.Color3 (77/255.0, 77/255.0, 26/255.0);
		freetypeFont51.DrawString (poem + " (Anonymous Pro)", 10, 340);
		freetypeFont52.DrawString (poem + " (Anonymous Pro)", 400, 340);
		GL.Color3 (77/255.0, 26/255.0, 77/255.0);
		freetypeFont61.DrawString (poem + " (Luxi Sans)", 10, 410);
		freetypeFont62.DrawString (poem + " (Luxi Sans)", 400, 410);
		GL.Color3 (26/255.0, 77/255.0, 77/255.0);
		freetypeFont71.DrawString (poem + " (Ubuntu Mono)", 10, 480);
		freetypeFont72.DrawString (poem + " (Ubuntu Mono)", 400, 480);

		SwapBuffers();
	}

	/// <summary>The main entry point of the application.</summary>
	[STAThread]
	static void Main()
	{
		// The 'using' idiom guarantees proper resource cleanup.
		// We request 30 UpdateFrame events per second, and unlimited
		// RenderFrame events (as fast as the computer can handle).
		using (FreeTypeLineWiseTest example = new FreeTypeLineWiseTest())
		{
			// Get the title and category  of this example using reflection.
			//ExampleAttribute info = ((ExampleAttribute)example.GetType().GetCustomAttributes(false)[0]);
			//example.Title = String.Format("OpenTK | {0} {1}: {2}", info.Category, info.Difficulty, info.Title);

			example.Title = String.Format("OpenTK | {0}: {1} {2}", "Font rendering (FreeType - line wise)", 14, "Fonts");
			example.Run(30.0, 0.0);
		}
	}

/*
	static Font3D smallFont;
	static Font3D largeFont;
	static float cnt1;
	static float cnt2;
	

	///The main render method
	public static void render()
	{
	
		//Clear display 
	    GL.Clear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);


		//Rotate something about
	    GL.Color3(0xff,0,0);
	    GL.PushMatrix();
	    GL.LoadIdentity();
	    GL.Rotate(cnt1,0,0,1);    
	    GL.Scale(1f,(float).8f+.3f* (float)Math.Cos((double)cnt1/5),1);
	    GL.Translate(-180,0,0);
	    largeFont.print( 320, 240, "FREE-TYPE / 2" );
	    GL.PopMatrix();
	    cnt1+=0.051f;   
	    cnt2+=0.005f;  
	    
		//Center a message
		string s = "You can press ESCAPE to exit the sample.";
		GL.Color3(0,0xff,0);		
		smallFont.print( 320-(smallFont.extent(s) >> 1), 10, s );

				
	}
*/
}