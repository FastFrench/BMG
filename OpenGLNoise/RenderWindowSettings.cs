using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenGLNoise.Lights;
using OpenGLNoise.Materials;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLNoise
{
  public class RenderWindowSettings : INotifyPropertyChanged, IDisposable
  {
    public BindingSource DataBindingSource { get; private set; } 

    public event PropertyChangedEventHandler PropertyChanged;
    private void Notify(string memberName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(memberName));
    }

    public RenderWindowSettings()
    {
      _gamma = 2.2f;
      Lights = new LightDataCollection();
      Material = new MaterialData();
      DataBindingSource = new BindingSource() { DataSource = this };
    }

    private void Lights_ListChanged(object sender, ListChangedEventArgs e)
    {
      Notify("Lights");
    }

    LightDataCollection _lights = null;

    public LightDataCollection Lights
    {
      get { return _lights; }
      set
      {
        if (_lights != null)
          _lights.ListChanged -= Lights_ListChanged;
        _lights = value;
        if (value != null)
          _lights.ListChanged += Lights_ListChanged;
        Lights_ListChanged(Lights, null);
      }
    }

    MaterialData _material = null;
    public MaterialData Material
    {
      get { return _material; }
      set
      {
        if (_material != null)
          _material.PropertyChanged -= Material_PropertyChanged;
        _material = value;
        if (value != null)
          Material.PropertyChanged += Material_PropertyChanged;
        Material_PropertyChanged(Material, null);
      }
    }

    private void Material_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      Notify("Material");      
    }

    bool _bouncing { get; set; }
    public bool Bouncing
    {
      get { return _bouncing; }
      set
      {
        if (_bouncing != value)
        {
          _bouncing = value;
          Notify("Bouncing");
        }
      }
    }

    bool _paused { get; set; }
    public bool Paused
    {
      get { return _paused; }
      set
      {
        if (_paused != value)
        {
          _paused = value;
          Notify("Paused");
        }
      }
    }

    float _gamma { get; set; }
    public float Gamma
    {
      get { return _gamma; }
      set
      {
        if (_gamma != value)
        {
          _gamma = value;
          Notify("Gamma");
        }
      }
    }


    bool _usingNoise { get; set; }
    public bool UsingNoise
    {
      get { return _usingNoise; }
      set
      {
        if (_usingNoise != value)
        {
          _usingNoise = value;
          Notify("UsingNoise");
        }
      }
    }

    bool _visible { get; set; }
    public bool Visible
    {
      get { return _visible; }
      set
      {
        if (_visible != value)
        {
          _visible = value;
          Notify("Visible");
        }
      }
    }

    public void Dispose()
    {
      if ( DataBindingSource != null)
      {
        DataBindingSource.Dispose();
        DataBindingSource = null;
      }
    }

    public MaterialStruct ConvertIntoGLMaterialStruct()
    {
      MaterialStruct material = Material.ConvertIntoGLStruct();
      material.Gamma = Gamma;
      material.NbLight = Lights.Count(l => l.Visible);
      material.UsingNoise = UsingNoise;
      material.Visible = Visible;
      material.Size = 0;
      return material;
    }
  }
}
