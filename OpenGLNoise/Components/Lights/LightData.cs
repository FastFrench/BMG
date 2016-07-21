using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using OpenGLNoise.Components.Lights;
using OpenTK;

namespace OpenGLNoise.Lights
{
  [Serializable]
  public class LightData : INotifyPropertyChanged
  {
    public LightData()
    {      
    }

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

    const float AMBIENT_RATIO = 0.33f;
    const float DIFFUSE_RATIO = 0.67f;
    const float SPECULAR_RATIO = 1.00f;

    private Color ApplyRatio(Color color, float ratio)
    {
      return Color.FromArgb(color.A, (byte)(color.R * ratio), (byte)(color.G * ratio), (byte)(color.B * ratio));
    }
    public LightData(Color color, Vector3 position, bool visible)
    {

      _ambientColor = ApplyRatio(color, AMBIENT_RATIO);
      _diffuseColor = ApplyRatio(color, DIFFUSE_RATIO);
      _specularColor = ApplyRatio(color, SPECULAR_RATIO);
      _position = position;
      _visible = visible;
    }

    byte avg(byte b1, byte b2, byte b3)
    {
      return (byte)(((int)b1 + b2 + b3) / 3);
    }

    [XmlIgnore]
    public Color GlobalColor
    {
      get
      {
        return Color.FromArgb(avg(_ambientColor.R, _diffuseColor.R, _specularColor.R), avg(_ambientColor.G, _diffuseColor.G, _specularColor.G), avg(_ambientColor.B, _diffuseColor.B, _specularColor.B));
      } 
      set
      {
        _ambientColor = ApplyRatio(value, AMBIENT_RATIO);
        _diffuseColor = ApplyRatio(value, DIFFUSE_RATIO);
        _specularColor = ApplyRatio(value, SPECULAR_RATIO);
        Notify("AmbientColor");
        Notify("DiffuseColor");
        Notify("SpecularColor");
        Notify("GlobalColor");
      }     
    }

    [XmlIgnore]
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

    [XmlIgnore]
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

    [XmlIgnore]
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

    #region Color serialization
    [XmlElement("SpecularColor")]
    public string SpecularColorStr
    {
      get { return ColorTranslator.ToHtml(_specularColor); }
      set { _specularColor = ColorTranslator.FromHtml(value); }
    }
    [XmlElement("DiffuseColor")]
    public string DiffuseColorStr
    {
      get { return ColorTranslator.ToHtml(_diffuseColor); }
      set { _diffuseColor = ColorTranslator.FromHtml(value); }
    }
    [XmlElement("AmbientColor")]
    public string AmbientColorStr
    {
      get { return ColorTranslator.ToHtml(_ambientColor); }
      set { _ambientColor = ColorTranslator.FromHtml(value); }
    }
    #endregion
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
      light.MaxDistance = 5.0f;
      return light;
    }
  }
}
