﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenGLNoise
{
  public class SphereObject : OpenGLObject
  {
    public SphereObject(Vector3 center, float radius)
    {
            this.radius = radius;
            this.center = center;
    }
    public override void BuildObject()
    {
      CreateVertexData();
      base.BuildObject();
    }

    public override void OnRenderObject(Matrix4 mvpMatrix)
    {
      base.OnRenderObject(mvpMatrix);
    }

    public override void OnUpdateObject(FrameEventArgs e)
    {
      base.OnUpdateObject(e);

    }
        const int _latitudeBands = 50;
        const int _longitudeBands = 100;
    const float _sphereRadius = 0.40f;//1.6f;

    int latitudeBands;
    int longitudeBands;

    protected override int upperBoundX { get { return longitudeBands; } }
    protected override int upperBoundZ { get { return latitudeBands; } }
    
    public float radius { get; private set; }
    public Vector3 center { get; private set; }
    
        List<Vector3> positions;
    List<Vector3> normals;
    List<int> indices;

    void CreateSphereData()
    {
      this.latitudeBands = (int)(_latitudeBands * radius);
      this.longitudeBands = (int)(_longitudeBands * radius);
      
      int i_basis = indices.Count;
      for (double latitudeNum = 0; latitudeNum <= _latitudeBands; latitudeNum++)
      {
        var theta = latitudeNum * MathHelper.Pi / _latitudeBands;
        var sinTheta = Math.Sin(theta);
        var cosTheta = Math.Cos(theta);
        for (double longitudeNum = 0; longitudeNum <= _longitudeBands; longitudeNum++)
        {
          var phi = longitudeNum * MathHelper.TwoPi / _longitudeBands;
          var sinPhi = Math.Sin(phi);
          var cosPhi = Math.Cos(phi);

          var px = cosPhi * sinTheta;
          var py = sinPhi * sinTheta;
          var pz = cosTheta;

          var normal = new Vector3((float)px, (float)py, (float)pz);
          var position = (normal + center) * radius;
          normals.Add(normal);
          positions.Add(position);
        }
      }

      for (int latitudeNum = 0; latitudeNum < _latitudeBands; latitudeNum++)
      {
        for (int longitudeNum = 0; longitudeNum <= _longitudeBands; longitudeNum++)
        {
          var i0 = i_basis + (latitudeNum * (_longitudeBands + 1)) + longitudeNum;
          var i1 = i_basis + i0 + _longitudeBands + 1;

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

      CreateSphereData();

      //CreateSphereData(0.5f, 0f, 0.5f, 0.5f+SphereRadius);

      Positions = positions.ToArray();
      Normals = normals.ToArray();
      Indices = indices.ToArray();      
    }
  }
}
