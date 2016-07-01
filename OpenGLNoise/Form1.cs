using System;
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

    public SynchronizationContext context = SynchronizationContext.Current;

    RenderWindowSettings settings { get; set; }
    
    public Form1()
    {
      settings = new RenderWindowSettings();
      settings.Lights.Add(new LightData(Color.White, new Vector3(3, 0.5f, 0.0f), true));
      settings.Lights.Add(new LightData(Color.Green, new Vector3(-3, -5.5f, -4.0f), true));
      settings.Lights.Add(new LightData(Color.Red, new Vector3(0, 3.5f, -12.0f), true));
      settings.Material = new MaterialData(Color.SteelBlue, 12.0f);
      InitializeComponent();
      userControlMaterialData1.Data = settings.Material;
      userControlLightData1.Data = settings.Lights[0];
      userControlLightData2.Data = settings.Lights[1];
      userControlLightData3.Data = settings.Lights[2];

      checkBoxBouncing.DataBindings.Add("Checked", settings.DataBindingSource,
          "Bouncing", false, DataSourceUpdateMode.OnPropertyChanged);
      checkBoxPaused.DataBindings.Add("Checked", settings.DataBindingSource,
          "Paused", false, DataSourceUpdateMode.OnPropertyChanged);

    }

    private void buttonRandom_Click(object sender, EventArgs e)
    {
      using (var window = new RenderRandomItems(settings))//HelloGL3())// RenderWindow())
      {
        window.Run();
      }
    }

    private void buttonTeapot_Click(object sender, EventArgs e)
    {
      using (var window = new RenderTeaPot(settings))//HelloGL3())// RenderWindow())
      {
        window.Run();
      }

    }

    private void buttonTerrain_Click(object sender, EventArgs e)
    {

    }

    public Color ColorLight1 { get; set; }
    public Color ColorLight2 { get; set; }
    public Color ColorLight3 { get; set; }

    public bool HasLight1 { get; set; }
    public bool HasLight2 { get; set; }
    public bool HasLight3 { get; set; }

  }
}
