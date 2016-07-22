using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGLNoise.Properties;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLNoise
{
	public static class OpenGLHelper
	{
		public static void SetUniformValue(int location, object data)
		{
			if (location >= 0)
			{
				if (data is float)
					GL.Uniform1(location, (float)data);
				else
				if (data is int)
					GL.Uniform1(location, (int)data);
				else
				if (data is uint)
					GL.Uniform1(location, (uint)data);
				else
				if (data is bool)
					GL.Uniform1(location, (int)((bool)data ? 1 : 0));
				else
				  if (data is Vector3)
					GL.Uniform3(location, (Vector3)data);
				else
				  if (data is Vector4)
					GL.Uniform4(location, (Vector4)data);
				else
				  if (data is Color4)
					GL.Uniform4(location, (Color4)data);
				else
					throw new NotImplementedException(string.Format("The type {0} is not supported", data.GetType().Name));
			}
		}

		/// <summary>
		/// Check if there is an OpenGL Error. Returns true if OK, false on error. 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		static public bool CheckError(string context)
		{
			var error = GL.GetError();
			if (error != ErrorCode.NoError)
			{
				Debug.Print("OpenGL error ({0}): {1}", context, error);
				return false;
			}
			return true;
		}

		static public byte[] GetRandomFragmentShader()
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

		static public byte[] GetRandomVertexShader()
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

		static public Random Rnd = new Random();
		static public Color GetRandomColor()
		{
			switch (Rnd.Next(10))
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

		static public Color4 TransformColor(Color color)
		{
			return new Color4(color.R, color.G, color.B, color.A);
		}
		static public Color TransformColor(Color4 color)
		{
			return Color.FromArgb(color.ToArgb());
		}

	}
}
