using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenGLNoise.Lights;

namespace OpenGLNoise
{
  public partial class UserControlLightData : UserControl
  {
    public UserControlLightData() :
      this(new LightData(Color.Bisque, new OpenTK.Vector3(), true), "noname")
    {
    }

    BindingSource dataBindingSource { get; set; }

    public UserControlLightData(LightData data, string name)
    {
      InitializeComponent();
      SetData(data, name);
    }

    LightData _data { get; set; }
    public LightData Data { get { return _data; } set { SetData(value, ColorName); } }

    private void SetData(LightData data, string name)
    {
      ColorName = name;
      _data = data;
      if (dataBindingSource != null)
        dataBindingSource.Dispose();
      dataBindingSource = new BindingSource() { DataSource = Data };
      checkBoxVisible.DataBindings.Clear();
      buttonSetColor.DataBindings.Clear();
      buttonSetAmbientColor.DataBindings.Clear();
      buttonSetDiffuseColor.DataBindings.Clear();
      buttonSetSpecularColor.DataBindings.Clear();
      checkBoxVisible.DataBindings.Add("Checked", dataBindingSource,
          "Visible", false, DataSourceUpdateMode.OnPropertyChanged);
      buttonSetColor.DataBindings.Add("BackColor", dataBindingSource,
          "GlobalColor", false, DataSourceUpdateMode.OnPropertyChanged);
      buttonSetAmbientColor.DataBindings.Add("BackColor", dataBindingSource,
          "AmbientColor", false, DataSourceUpdateMode.OnPropertyChanged);
      buttonSetDiffuseColor.DataBindings.Add("BackColor", dataBindingSource,
          "DiffuseColor", false, DataSourceUpdateMode.OnPropertyChanged);
      buttonSetSpecularColor.DataBindings.Add("BackColor", dataBindingSource,
          "SpecularColor", false, DataSourceUpdateMode.OnPropertyChanged);
      textBoxX.Text = Data.Position.X.ToString();
      textBoxY.Text = Data.Position.Y.ToString();
      textBoxZ.Text = Data.Position.Z.ToString();
      groupBox1.Text = name;
    }

    public string ColorName { get; set; }

    private void buttonSetColor_Click(object sender, EventArgs e)
    {
      if (Data == null) return;
      colorDialog.Color = Data.GlobalColor;
      if (colorDialog.ShowDialog() == DialogResult.OK)
        Data.GlobalColor = colorDialog.Color;
    }

    private void textBoxX_TextChanged(object sender, EventArgs e)
    {
      if (Data == null) return;
      float newValue = 0.0f;
      if (float.TryParse(textBoxX.Text, out newValue))
      {
        Vector3 newPosition = new Vector3(newValue, Data.Position.Y, Data.Position.Z);
        Data.Position = newPosition;
      }
    }

    private void textBox2_TextChanged(object sender, EventArgs e)
    {
      if (Data == null) return;
      float newValue = 0.0f;
      if (float.TryParse(textBoxY.Text, out newValue))
      {
        Vector3 newPosition = new Vector3(Data.Position.X, newValue, Data.Position.Z);
        Data.Position = newPosition;
      }
    }

    private void textBox3_TextChanged(object sender, EventArgs e)
    {
      if (Data == null) return;
      float newValue = 0.0f;
      if (float.TryParse(textBoxZ.Text, out newValue))
      {
        Vector3 newPosition = new Vector3(Data.Position.X, Data.Position.Y, newValue);
        Data.Position = newPosition;
      }
    }

    private void buttonSetAmbiantColor_Click(object sender, EventArgs e)
    {
      if (Data == null) return;
      colorDialog.Color = Data.AmbientColor;
      if (colorDialog.ShowDialog() == DialogResult.OK)
        Data.AmbientColor = colorDialog.Color;
    }

    private void buttonSetDiffuseColor_Click(object sender, EventArgs e)
    {
      if (Data == null) return;
      colorDialog.Color = Data.DiffuseColor;
      if (colorDialog.ShowDialog() == DialogResult.OK)
        Data.DiffuseColor = colorDialog.Color;
    }

    private void buttonSetSpecularColor_Click(object sender, EventArgs e)
    {
      if (Data == null) return;
      colorDialog.Color = Data.SpecularColor;
      if (colorDialog.ShowDialog() == DialogResult.OK)
        Data.SpecularColor = colorDialog.Color;
    }
  }
}
