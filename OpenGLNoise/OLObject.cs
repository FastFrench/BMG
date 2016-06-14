using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenGLNoise
{
	public abstract class OLObject
	{
		public Vector3[] Positions { get; set; }
		public Vector3[] Normals { get; set; }
		public int[] indices { get; set; }


		abstract public void BuildObject();
		abstract public void RenderObject();
	}
}
