using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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


    Color _color;
    Vector3 _position;
    bool _visible;

    public LightData(Color color, Vector3 position, bool visible)
    {
      _color = color;
      _position = position;
      _visible = visible;
    }

    public Color Color
    {
      get
      {
        return _color;
      }
      set
      {
        if (_color != value)
        {
          _color = value;
          Notify("Color");
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
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs("Position"));
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
  }
}
