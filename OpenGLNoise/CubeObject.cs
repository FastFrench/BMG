using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLNoise
{
  public class CubeObject : OpenGLObject
  {
    public CubeObject(Vector3 center, float radius, bool noDeformation = false, bool withLight = true, bool withNoise = false)
    : base(noDeformation ? 0 : radius, null, null, withNoise, withLight)
    {
      this.radius = radius;
      this.center = center;
    }
    public override void BuildObject()
    {
      CreateVertexData();
      base.BuildObject();
    }


    const float _quality = 200.0f;
    int latitudeBands;
    int longitudeBands;

    protected override int upperBoundX { get { return longitudeBands; } }
    protected override int upperBoundZ { get { return latitudeBands; } }

    public float radius { get; private set; }
    public Vector3 center { get; private set; }

    List<Vector3> positions;
    List<Vector3> normals;
    List<int> indices;

    void CreateCubeData(float quality = 1/*_quality*/)
    {
      this.latitudeBands = Math.Max(1, (int)(quality * radius));
      this.longitudeBands = Math.Max(1, (int)(quality * radius));

      for (int face = 0; face < 6; face++)
        for (double latitudeNum = 0; latitudeNum <= latitudeBands; latitudeNum++)
          for (double longitudeNum = 0; longitudeNum <= longitudeBands; longitudeNum++)
          {
            float v1 = (float)(-radius + (2 * radius * latitudeNum) / latitudeBands);
            float v2 = (float)(-radius + (2 * radius * longitudeNum) / longitudeBands);
            float px = 0.0f, py = 0.0f, pz = 0.0f;
            Vector3 normal = new Vector3();
            switch (face)
            {
              case 0: // top
                py = radius;
                px = v1;
                pz = v2;
                normal = new Vector3(0, 1, 0);
                break;
              case 1: // bottom
                py = -radius;
                px = v1;
                pz = v2;
                normal = new Vector3(0, -1, 0);
                break;
              case 2:
                px = -radius;
                py = v1;
                pz = v2;
                normal = new Vector3(-1, 0, 0);
                break;
              case 3:
                px = radius;
                py = v1;
                pz = v2;
                normal = new Vector3(1, 0, 0);
                break;
              case 4:
                pz = -radius;
                px = v1;
                py = v2;
                normal = new Vector3(0, 0, -1);
                break;
              case 5:
                pz = radius;
                px = v1;
                py = v2;
                normal = new Vector3(0, 0, 1);
                break;
            }
            var position = new Vector3(px, py, pz) + center;
            normals.Add(normal);
            positions.Add(position);
          }

      for (int face = 0; face < 6; face++)
        for (int latitudeNum = 0; latitudeNum < latitudeBands; latitudeNum++)
        {
          int basis = (face * (latitudeBands + 1) + latitudeNum) * (longitudeBands + 1);
          for (int longitudeNum = 0; longitudeNum < longitudeBands; longitudeNum++)
          {
            var i0 = basis + longitudeNum;
            var i1 = i0 + longitudeBands + 1;

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

      CreateCubeData();

      //CreateSphereData(0.5f, 0f, 0.5f, 0.5f+SphereRadius);

      Positions = positions.ToArray();
      Normals = normals.ToArray();
      Indices = indices.ToArray();
    }
  }
}

