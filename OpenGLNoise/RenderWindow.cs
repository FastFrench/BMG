
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
        //TextRenderer renderer;
        //Font serif = new Font(FontFamily.GenericSerif, 24);
        //Font sans = new Font(FontFamily.GenericSansSerif, 24);
        //Font mono = new Font(FontFamily.GenericMonospace, 24);


        const int LatitudeBands = 50;
        const int LongitudeBands = 100;
        const float SphereRadius = 0.40f;//1.6f;

        bool Bouncing = true;

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
            //if (e.KeyChar == ' ')
            //	DisplayNormals = !DisplayNormals;
            //Thread.Sleep(2000);
            switch (e.KeyChar)
            {
                case '+':
                    AddARandomSphere();
                    BuildObjects();
                    break;
                case '-':
                    RemoveASphere();
                    BuildObjects();
                    break;
                case ' ':
                    Bouncing = !Bouncing;
                    break;
                default:
                    CreateObjects();
                    break;
            }
            base.OnKeyPress(e);
        }

        private void RemoveASphere()
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

        void BuildObjects()
        {
            // Create sphere data and set up buffers
            foreach (var obj in Objects)
                obj.BuildObject();
            //CreateVertexData();
        }

        Random rnd = new Random();

        byte[] GetRandomFragmentShader()
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

        byte[] GetRandomVertexShader()
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

        Color GetRandomColor()
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

        void AddSphere(float px, float py, float pz, float radius)
        {
            var sphere = new SphereObject(new Vector3(px, py, pz), radius * 2, true /*rnd.Next(2) == 0*/);
            sphere.Color1 = GetRandomColor();
            do
            {
                sphere.Color2 = GetRandomColor();
            } while (sphere.Color2 == sphere.Color1);
            sphere.LoadShaders(GetRandomFragmentShader(), GetRandomVertexShader(), null);

            //sphere.LoadShaders(null, null, null);// GetRandomFragmentShader(), GetRandomVertexShader(), null);

            Objects.Add(sphere);

        }

        void AddARandomSphere()
        {
            AddSphere((float)(rnd.NextDouble() * 12.0 - 6.0), (float)(rnd.NextDouble() * 12.0 - 6.0), (float)(rnd.NextDouble() * 12.0 - 6.0), (float)(SphereRadius * (0.1 + 0.9 * rnd.NextDouble())));
        }

        void CreateObjects()
        {
            if (Objects != null)
                foreach (var obj in Objects)
                    obj.Dispose();
            Objects = new List<OpenGLObject>();
            int nbSpheres = rnd.Next(4, 20);
            for (int i = 0; i < nbSpheres; i++)
                AddARandomSphere();
            BuildObjects();
        }

        List<OpenGLObject> Objects { get; set; }

        //void LightOn()
        //{
        //  GL.Light(LightName.Light0, LightParameter.Position, new float[] { 1.0f, 1.0f, -0.5f });
        //  GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.3f, 0.3f, 0.3f, 1.0f });
        //  GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
        //  GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
        //  GL.Light(LightName.Light0, LightParameter.SpotExponent, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
        //  GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 0.2f, 0.2f, 0.2f, 1.0f });
        //  GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
        //  GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);
        //  GL.Enable(EnableCap.Lighting);
        //  GL.Enable(EnableCap.Light0);

        //  //Use GL.Material to set your object's material parameters.
        //  GL.Material(MaterialFace.Front, MaterialParameter.Ambient, new float[] { 0.3f, 0.3f, 0.3f, 1.0f });
        //  GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
        //  GL.Material(MaterialFace.Front, MaterialParameter.Specular, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
        //  GL.Material(MaterialFace.Front, MaterialParameter.Emission, new float[] { 0.0f, 0.0f, 0.0f, 1.0f });
        //  GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.Specular);
        //}
        protected override void OnLoad(EventArgs e)
        {
            CreateObjects();
            frames = 0;

            //renderer = new TextRenderer(Width, Height);
            PointF position = PointF.Empty;

            GL.ClearColor(Color4.Gray);

            //GL.Enable(EnableCap.DepthTest);
            //LightOn();
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
            if (Bouncing)
            {
                if (sign)
                    alt += 0.03f;
                else
                    alt -= 0.03f;
                if (alt > 15 || alt < 3) sign = !sign;
            }
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
            //renderer.Clear(Color.Black);
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
