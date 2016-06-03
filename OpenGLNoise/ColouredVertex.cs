using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenGLNoise
{
  public struct ColouredVertex
  {
    public const int Size = sizeof(float) * (3 + 4);// (3 + 4) * 4; // size of struct in bytes

    private readonly Vector3 position;
    private readonly Color4 color;

    public ColouredVertex(Vector3 position, Color4 color)
    {
      this.position = position;
      this.color = color;
    }

    /// <summary> 
    /// Uploads the vertex buffer to the GPU. 
    /// </summary> 
    /// <param name="target">The target.</param> 
    /// <param name="usageHint">The usage hint.</param> 
    static public void BufferData(List<ColouredVertex> list)
    {
      
      GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Size * list.Count), list.ToArray(), BufferUsageHint.StreamDraw);
    }

  }


}
