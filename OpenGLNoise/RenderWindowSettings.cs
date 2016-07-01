using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenGLNoise.Lights;
using OpenGLNoise.Materials;

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
      Lights = new LightDataCollection();
      Material = new MaterialData();
      Lights.CollectionChanged += Lights_CollectionChanged;
      DataBindingSource = new BindingSource() { DataSource = this };
    }

    private void Lights_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      Notify("Lights");
    }

    public LightDataCollection Lights
    {
      get;
      set;
    }

    public MaterialData Material
    {
      get;
      set;
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

    public void Dispose()
    {
      if ( DataBindingSource != null)
      {
        DataBindingSource.Dispose();
        DataBindingSource = null;
      }
    }
  }
}
