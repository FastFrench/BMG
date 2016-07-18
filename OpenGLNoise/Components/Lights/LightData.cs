using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenGLNoise.Components.Lights;
using OpenTK;

namespace OpenGLNoise.Lights
{
  public class LightData : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    private void Notify(string memberName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(memberName));
    }


    Color _ambientColor;
    Color _diffuseColor;
    Color _specularColor;
    Vector3 _position;
    bool _visible;

    public LightData(Color color, Vector3 position, bool visible)
    {
      GlobalColor = color;
      _position = position;
      _visible = visible;
    }

    byte avg(byte b1, byte b2, byte b3)
    {
      return (byte)(((int)b1 + b2 + b3) / 3);
    }

    public Color GlobalColor
    {
      get
      {
        return Color.FromArgb(avg(_ambientColor.R, _diffuseColor.R, _specularColor.R), avg(_ambientColor.G, _diffuseColor.G, _specularColor.G), avg(_ambientColor.B, _diffuseColor.B, _specularColor.B));
      } 
      set
      {
        AmbientColor = value;
        DiffuseColor = value;
        SpecularColor = value;
      }     
    }

    public Color AmbientColor
    {
      get
      {
        return _ambientColor;
      }
      set
      {
        if (_ambientColor != value)
        {
          _ambientColor = value;
          Notify("AmbientColor");
          Notify("GlobalColor");
        }
      }
    }

    public Color DiffuseColor
    {
      get
      {
        return _diffuseColor;
      }
      set
      {
        if (_diffuseColor != value)
        {
          _diffuseColor = value;
          Notify("DiffuseColor");
          Notify("GlobalColor");
        }
      }
    }
    public Color SpecularColor
    {
      get
      {
        return _specularColor;
      }
      set
      {
        if (_specularColor != value)
        {
          _specularColor = value;
          Notify("SpecularColor");
          Notify("GlobalColor");
        }
      }
    }

    public Vector3 Position
    {
      get
      {
        return _position;
      }
      set
      {
        if (_position != value)
        {
          _position = value;
          Notify("Position");
        }
      }
    }

    public bool Visible
    {
      get
      {
        return _visible;
      }
      set
      {
        if (_visible != value)
        {
          _visible = value;
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs("Visible"));
        }
      }
    }

    public LightStruct FillLightStructStruct(ref LightStruct light)
    {
      light.AmbientColor = LightDataCollection.Color2Vector4(AmbientColor);
      light.DiffuseColor = LightDataCollection.Color2Vector4(DiffuseColor);
      light.SpecularColor = LightDataCollection.Color2Vector4(SpecularColor);
      light.Position = new Vector3(Position);
      light.Visible = Visible;
      return light;
    }
  }
}
