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

namespace OpenGLNoise.Materials
{
  public partial class UserControlMaterialData : UserControl
  {
    public UserControlMaterialData() :
      this(new MaterialData(Color.Bisque, 5.0f), "noname")
    {
    }

    BindingSource dataBindingSource { get; set; }

    public UserControlMaterialData(MaterialData data, string name)
    {
      InitializeComponent();
      SetData(data, name);
    }

    MaterialData _data { get; set; }
    public MaterialData Data { get { return _data; } set { SetData(value, ColorName); } }

    private void SetData(MaterialData data, string name)
    {
      ColorName = name;
      _data = data;
      if (dataBindingSource != null)
        dataBindingSource.Dispose();
      dataBindingSource = new BindingSource() { DataSource = Data };
      buttonSetColor.DataBindings.Clear();
      buttonSetAmbientColor.DataBindings.Clear();
      buttonSetDiffuseColor.DataBindings.Clear();
      buttonSetSpecularColor.DataBindings.Clear();
      buttonSetColor.DataBindings.Add("BackColor", dataBindingSource,
          "GlobalColor", false, DataSourceUpdateMode.OnPropertyChanged);
      buttonSetAmbientColor.DataBindings.Add("BackColor", dataBindingSource,
          "AmbientReflectivity", false, DataSourceUpdateMode.OnPropertyChanged);
      buttonSetDiffuseColor.DataBindings.Add("BackColor", dataBindingSource,
          "DiffuseReflectivity", false, DataSourceUpdateMode.OnPropertyChanged);
      buttonSetSpecularColor.DataBindings.Add("BackColor", dataBindingSource,
          "SpecularReflectivity", false, DataSourceUpdateMode.OnPropertyChanged);
      textBoxX.Value = (decimal)Data.Shininess;
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
      Data.Shininess = (float)textBoxX.Value;      
    }
    
    private void buttonSetAmbiantColor_Click(object sender, EventArgs e)
    {
      if (Data == null) return;
      colorDialog.Color = Data.AmbientReflectivity;
      if (colorDialog.ShowDialog() == DialogResult.OK)
        Data.AmbientReflectivity = colorDialog.Color;
    }

    private void buttonSetDiffuseColor_Click(object sender, EventArgs e)
    {
      if (Data == null) return;
      colorDialog.Color = Data.DiffuseReflectivity;
      if (colorDialog.ShowDialog() == DialogResult.OK)
        Data.DiffuseReflectivity = colorDialog.Color;
    }

    private void buttonSetSpecularColor_Click(object sender, EventArgs e)
    {
      if (Data == null) return;
      colorDialog.Color = Data.SpecularReflectivity;
      if (colorDialog.ShowDialog() == DialogResult.OK)
        Data.SpecularReflectivity = colorDialog.Color;
    }
  }
}
