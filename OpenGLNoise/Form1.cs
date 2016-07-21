using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using OpenGLNoise.Lights;
using OpenGLNoise.Materials;
using OpenTK;

namespace OpenGLNoise
{
  public partial class Form1 : Form
  {
    const string SettingsFileName = "Settings.xml";
    public SynchronizationContext context = SynchronizationContext.Current;

    RenderWindowSettings settings { get; set; }
    
    public Form1()
    {

      settings = RenderWindowSettings.Load(SettingsFileName);
      if (settings == null)
      {
        settings = new RenderWindowSettings();
        settings.Lights.Add(new LightData(Color.White, new Vector3(3, 0.5f, 0.0f), true));
        settings.Lights.Add(new LightData(Color.Green, new Vector3(-3, -5.5f, -4.0f), true));
        settings.Lights.Add(new LightData(Color.Red, new Vector3(0, 3.5f, -12.0f), true));
        settings.Material = new MaterialData(Color.White, 20.0f);
      }
      InitializeComponent();
      userControlMaterialData1.Data = settings.Material;
      userControlLightData1.Data = settings.Lights[0];
      userControlLightData2.Data = settings.Lights[1];
      userControlLightData3.Data = settings.Lights[2];

      checkBoxBouncing.DataBindings.Add("Checked", settings.DataBindingSource,
          "Bouncing", false, DataSourceUpdateMode.OnPropertyChanged);
      checkBoxPaused.DataBindings.Add("Checked", settings.DataBindingSource,
          "Paused", false, DataSourceUpdateMode.OnPropertyChanged);
      numericUpDownGamma.DataBindings.Add("Value", settings.DataBindingSource,
          "Gamma", false, DataSourceUpdateMode.OnPropertyChanged);

    }

    protected override void OnClosed(EventArgs e)
    {
      RenderWindowSettings.Save(settings, SettingsFileName);

      base.OnClosed(e);
      CloseRunningWindows();
    }

    List<RenderWindowBase> windows = new List<RenderWindowBase>();
    void CloseRunningWindows()
    {
      foreach (var window in windows)
      {
        //window.Visible = false;
        window.Exit();
        //window.ProcessEvents();
        //window.Dispose();
      }
      windows.Clear();
    }
    private void buttonRandom_Click(object sender, EventArgs e)
    {
      CloseRunningWindows();
      using (var window = new RenderRandomItems(settings))//HelloGL3())// RenderWindow())
      {
        windows.Add(window);
        window.Run();
        windows.Remove(window);
      }
    }

    private void buttonTeapot_Click(object sender, EventArgs e)
    {
      CloseRunningWindows();
      using (var window = new RenderTeaPot(settings))//HelloGL3())// RenderWindow())
      {
        windows.Add(window);
        window.Run();
        windows.Remove(window);
      }

    }

    private void buttonTerrain_Click(object sender, EventArgs e)
    {
      CloseRunningWindows();

    }
  }
}
