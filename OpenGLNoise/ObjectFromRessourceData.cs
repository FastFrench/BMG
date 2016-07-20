using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGLNoise.Properties;
using OpenTK;

namespace OpenGLNoise
{
  public abstract class ObjectFromRessourceData : OpenGLObject
  {
    
    public ObjectFromRessourceData(Vector3 center, float size, bool withDeformations, Color? color1, Color? color2)
        : base(withDeformations ? size : 0, color1, color2, withDeformations, true)
    {
      Center = center;
      Radius = size;
      DeformationAmplitude = 0;
    }
    protected Vector3[] MoveAndResizeData(Vector3[] normalizedVertices)
    {
      Vector3[] newVertices = new Vector3[normalizedVertices.Length];
      for (int i = 0; i < normalizedVertices.Count(); i++)
        newVertices[i] = normalizedVertices[i] * Radius + Center; // Move the Vertex to have a center position according to the Center vector and the proper radius
      return newVertices;
    }

    abstract protected bool GetStaticData(out Vector3[] vertices, out int[] indices, out Vector3[] normals);

    protected bool ReadData(string data, out Vector3[] vertices, out int[] indices, out Vector3[] normals)
    {
      vertices = null;
      indices = null;
      normals = null;
      if (string.IsNullOrEmpty(data)) return false;
      string[] lines = data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
      List<Vector3> vertList = new List<Vector3>();
      List<int> indexList = new List<int>();
      List<Vector3> normList = new List<Vector3>();
      Vector3 verticeSumVector = new Vector3(0, 0, 0);
      // Step 1: analyse the data and fill verList & indexList 
      foreach (string line in lines)
      {
        string[] items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //Debug.Assert(items.Length == 4 && (items[0] == "v" || items[0] == "f"));
        if (items.Length != 4) continue;
        if (items[0] == "v")
        {
          float fx, fy, fz;
          if (!float.TryParse(items[1], NumberStyles.Float, CultureInfo.In­variantCulture, out fy))
          {
            Debug.Assert(false, items[1] + " is not a valid float");
            continue;
          }
          if (!float.TryParse(items[2], NumberStyles.Float, CultureInfo.In­variantCulture, out fz))
          {
            Debug.Assert(false, items[2] + " is not a valid float");
            continue;
          }
          if (!float.TryParse(items[3], NumberStyles.Float, CultureInfo.In­variantCulture, out fx))
          {
            Debug.Assert(false, items[3] + " is not a valid float");
            continue;
          }
          Vector3 vertice = new Vector3(fx, fy, fz);
          verticeSumVector += vertice;
          vertList.Add(vertice);
        }
        else if (items[0] == "f")
        {
          int i1, i2, i3;
          if (!int.TryParse(items[1], out i1))
          {
            Debug.Assert(false, items[1] + " is not a valid int");
            continue;
          }
          if (!int.TryParse(items[2], out i2))
          {
            Debug.Assert(false, items[2] + " is not a valid int");
            continue;
          }
          if (!int.TryParse(items[3], out i3))
          {
            Debug.Assert(false, items[3] + " is not a valid int");
            continue;
          }
          Debug.Assert(i1 >= 1 && i2 >= 1 && i3 >= 1);
          indexList.Add(i1 - 1);
          indexList.Add(i2 - 1);
          indexList.Add(i3 - 1);
        }
      }

      // Step 1bis: normalize the item, then grow at the right size and move where expected
      if (vertList.Count() > 0)
      {
        float distSum = 0;

        // Center the item (so that the center is as wanted)
        Vector3 currentCenter = verticeSumVector / vertList.Count();
        for (int i = 0; i < vertList.Count(); i++)
        {
          vertList[i] -= currentCenter; // Move the Vertex to have a center position at 0,0,0
          distSum += vertList[i].Length; // Check the distance from the center
        }
        distSum /= vertList.Count();

        // Normalize the item (so that the average distance to the center is 1), then apply the wanted size (Radius)
        for (int i = 0; i < vertList.Count(); i++)
        {
          vertList[i] /= distSum;
          //vertList[i] *= radius;
        }

        //for (int i = 0; i < vertList.Count(); i++)
        //{
        //  vertList[i] += center; // Move the Vertex to have a center position according to the Center vector
        //}
      }

      // Step 2: convert lists into arrays for more efficient further processing (and needed anyway for OpenGL)
      vertices = vertList.ToArray();
      indices = indexList.ToArray();

      // Step 3: for each vertex, compute the list of all normals of adjacent triangle, weighted based on the area of the triangle
      List<Vector3>[] normalComponents = new List<Vector3>[vertices.Length];
      for (int i = 0; i < vertices.Length; i++)
        normalComponents[i] = new List<Vector3>();

      for (int i = 0; i < indices.Length; i += 3)
      {
        int i1 = indices[i];
        int i2 = indices[i + 1];
        int i3 = indices[i + 2];
        Debug.Assert(i1 >= 0 && i1 < vertices.Length);
        Debug.Assert(i2 >= 0 && i2 < vertices.Length);
        Debug.Assert(i3 >= 0 && i3 < vertices.Length);
        Vector3 v1 = vertices[i3] - vertices[i1];
        Vector3 v2 = vertices[i2] - vertices[i1];
        Vector3 normal = Vector3.Cross(v2, v1);
        normalComponents[i1].Add(normal);
        normalComponents[i2].Add(normal);
        normalComponents[i3].Add(normal);
      }

      // Step 4: compute the resulting (average) normal for each vertex, and normalize it
      for (int i = 0; i < vertices.Length; i++)
      {
        Vector3 result = new Vector3(0f, 0f, 0f);
        foreach (Vector3 subNormal in normalComponents[i])
          result = Vector3.Add(result, subNormal);
        result.Normalize();
        normList.Add(result);
      }

      // Step5: store the resulting array
      normals = normList.ToArray();
      Debug.Assert(normals.Length == vertices.Length);
      Debug.Assert(indices.Length > 2 && (indices.Length % 3) == 0); // Not even enough for a single triangle
      Debug.Assert(vertices.Length > 3);
      return (normals.Length == vertices.Length) && (indices.Length > 2 && (indices.Length % 3) == 0) && (vertices.Length > 3);
    }

    protected override int lowerBoundX { get { return -2; } set { } }
    protected override int upperBoundX { get { return 2; } set { } }
    protected override int lowerBoundZ { get { return -2; } set { } }
    protected override int upperBoundZ { get { return 2; } set { } }

    void CreateVertexData()
    {
      Vector3[] vertices;
      int[] indices;
      Vector3[] normals;
      if (!GetStaticData(out vertices, out indices, out normals)) return;
      Indices = indices;
      Normals = normals;
      Positions = MoveAndResizeData(vertices);
      //Positions = vertices;
      //Normals = normals;
      //Indices = indices;
      DeformationAmplitude = 0;
    }

    protected override void InternalBuildObject()
    {
      CreateVertexData();
    }

  }
}
