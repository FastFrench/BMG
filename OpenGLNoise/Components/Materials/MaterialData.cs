﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenGLNoise.Lights;
using OpenTK;

namespace OpenGLNoise.Materials
{
  public class MaterialData : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    private void Notify(string memberName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(memberName));
    }

    Color _ambientReflectivity;     //Ambient reflectivity
    Color _diffuseReflectivity;     //Diffuse reflectivity
    Color _specularReflectivity;    //Specular reflectivity
    float _shininess;	              //Specular shininess factor


    public MaterialData()
      : this(Color.Maroon, 5.1f)
    {
    }

    public MaterialData(Color color, float shininess)
    {
      GlobalColor = color;
      _shininess = shininess;
    }

    byte avg(byte b1, byte b2, byte b3)
    {
      return (byte)(((int)b1 + b2 + b3) / 3);
    }

    public Color GlobalColor
    {
      get
      {
        return Color.FromArgb(avg(_ambientReflectivity.R, _diffuseReflectivity.R, _specularReflectivity.R), avg(_ambientReflectivity.G, _diffuseReflectivity.G, _specularReflectivity.G), avg(_ambientReflectivity.B, _diffuseReflectivity.B, _specularReflectivity.B));
      } 
      set
      {
        AmbientReflectivity = value;
        DiffuseReflectivity = value;
        SpecularReflectivity = value;
      }     
    }

    public Color AmbientReflectivity
    {
      get
      {
        return _ambientReflectivity;
      }
      set
      {
        if (_ambientReflectivity != value)
        {
          _ambientReflectivity = value;
          Notify("AmbientReflectivity");
          Notify("GlobalColor");
        }
      }
    }

    public Color DiffuseReflectivity
    {
      get
      {
        return _diffuseReflectivity;
      }
      set
      {
        if (_diffuseReflectivity != value)
        {
          _diffuseReflectivity = value;
          Notify("DiffuseReflectivity");
          Notify("GlobalColor");
        }
      }
    }
    public Color SpecularReflectivity
    {
      get
      {
        return _specularReflectivity;
      }
      set
      {
        if (_specularReflectivity != value)
        {
          _specularReflectivity = value;
          Notify("SpecularReflectivity");
          Notify("GlobalColor");
        }
      }
    }

    public float Shininess
    {
      get
      {
        return _shininess;
      }
      set
      {
        if (_shininess != value)
        {
          _shininess = value;
          Notify("Shininess");
        }
      }
    }

    public void ConvertIntoGLStruct(ref MaterialStruct materialStruct)
    {
      materialStruct.AmbientReflectivity = LightDataCollection.Color2Vector3(this.AmbientReflectivity);
      materialStruct.DiffuseReflectivity = LightDataCollection.Color2Vector3(this.DiffuseReflectivity);
      materialStruct.SpecularReflectivity = LightDataCollection.Color2Vector3(this.SpecularReflectivity);
      materialStruct.Shininess = this.Shininess;      
    }

  }
}
