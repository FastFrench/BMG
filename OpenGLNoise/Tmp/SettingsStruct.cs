using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenGLNoise.Components
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = sizeof(float))]
	public struct SettingsStruct
	{
		public Matrix4 MVP, View;
		public float Time;
		public float Gamma;
		public int NbLights;
	}
}
