namespace OpenGLNoise
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      OpenGLNoise.Materials.MaterialData materialData1 = new OpenGLNoise.Materials.MaterialData();
      this.buttonRandom = new System.Windows.Forms.Button();
      this.buttonTeapot = new System.Windows.Forms.Button();
      this.buttonTerrain = new System.Windows.Forms.Button();
      this.colorDialog1 = new System.Windows.Forms.ColorDialog();
      this.userControlLightData1 = new OpenGLNoise.UserControlLightData();
      this.userControlLightData2 = new OpenGLNoise.UserControlLightData();
      this.userControlLightData3 = new OpenGLNoise.UserControlLightData();
      this.checkBoxPaused = new System.Windows.Forms.CheckBox();
      this.checkBoxBouncing = new System.Windows.Forms.CheckBox();
      this.userControlMaterialData1 = new OpenGLNoise.Materials.UserControlMaterialData();
      this.numericUpDownGamma = new System.Windows.Forms.NumericUpDown();
      this.LabelGamma = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGamma)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonRandom
      // 
      this.buttonRandom.Location = new System.Drawing.Point(60, 37);
      this.buttonRandom.Name = "buttonRandom";
      this.buttonRandom.Size = new System.Drawing.Size(99, 23);
      this.buttonRandom.TabIndex = 0;
      this.buttonRandom.Text = "Random items";
      this.buttonRandom.UseVisualStyleBackColor = true;
      this.buttonRandom.Click += new System.EventHandler(this.buttonRandom_Click);
      // 
      // buttonTeapot
      // 
      this.buttonTeapot.Location = new System.Drawing.Point(60, 92);
      this.buttonTeapot.Name = "buttonTeapot";
      this.buttonTeapot.Size = new System.Drawing.Size(99, 23);
      this.buttonTeapot.TabIndex = 0;
      this.buttonTeapot.Text = "Tea pot only";
      this.buttonTeapot.UseVisualStyleBackColor = true;
      this.buttonTeapot.Click += new System.EventHandler(this.buttonTeapot_Click);
      // 
      // buttonTerrain
      // 
      this.buttonTerrain.Location = new System.Drawing.Point(60, 147);
      this.buttonTerrain.Name = "buttonTerrain";
      this.buttonTerrain.Size = new System.Drawing.Size(99, 23);
      this.buttonTerrain.TabIndex = 0;
      this.buttonTerrain.Text = "Terrain";
      this.buttonTerrain.UseVisualStyleBackColor = true;
      this.buttonTerrain.Click += new System.EventHandler(this.buttonTerrain_Click);
      // 
      // colorDialog1
      // 
      this.colorDialog1.AnyColor = true;
      this.colorDialog1.FullOpen = true;
      this.colorDialog1.SolidColorOnly = true;
      // 
      // userControlLightData1
      // 
      this.userControlLightData1.ColorName = "Light 1";
      this.userControlLightData1.Location = new System.Drawing.Point(240, 5);
      this.userControlLightData1.Name = "userControlLightData1";
      this.userControlLightData1.Size = new System.Drawing.Size(620, 63);
      this.userControlLightData1.TabIndex = 3;
      // 
      // userControlLightData2
      // 
      this.userControlLightData2.ColorName = "Light 2";
      this.userControlLightData2.Location = new System.Drawing.Point(240, 68);
      this.userControlLightData2.Name = "userControlLightData2";
      this.userControlLightData2.Size = new System.Drawing.Size(620, 63);
      this.userControlLightData2.TabIndex = 3;
      // 
      // userControlLightData3
      // 
      this.userControlLightData3.ColorName = "Light 3";
      this.userControlLightData3.Location = new System.Drawing.Point(240, 131);
      this.userControlLightData3.Name = "userControlLightData3";
      this.userControlLightData3.Size = new System.Drawing.Size(620, 63);
      this.userControlLightData3.TabIndex = 3;
      // 
      // checkBoxPaused
      // 
      this.checkBoxPaused.AutoSize = true;
      this.checkBoxPaused.Location = new System.Drawing.Point(40, 200);
      this.checkBoxPaused.Name = "checkBoxPaused";
      this.checkBoxPaused.Size = new System.Drawing.Size(62, 17);
      this.checkBoxPaused.TabIndex = 4;
      this.checkBoxPaused.Text = "Paused";
      this.checkBoxPaused.UseVisualStyleBackColor = true;
      // 
      // checkBoxBouncing
      // 
      this.checkBoxBouncing.AutoSize = true;
      this.checkBoxBouncing.Location = new System.Drawing.Point(140, 200);
      this.checkBoxBouncing.Name = "checkBoxBouncing";
      this.checkBoxBouncing.Size = new System.Drawing.Size(71, 17);
      this.checkBoxBouncing.TabIndex = 4;
      this.checkBoxBouncing.Text = "Bouncing";
      this.checkBoxBouncing.UseVisualStyleBackColor = true;
      // 
      // userControlMaterialData1
      // 
      this.userControlMaterialData1.ColorName = "noname";
      materialData1.AmbientReflectivity = System.Drawing.Color.Bisque;
      materialData1.DiffuseReflectivity = System.Drawing.Color.Bisque;
      materialData1.GlobalColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(228)))), ((int)(((byte)(196)))));
      materialData1.Shininess = 5F;
      materialData1.SpecularReflectivity = System.Drawing.Color.Bisque;
      this.userControlMaterialData1.Data = materialData1;
      this.userControlMaterialData1.Location = new System.Drawing.Point(339, 200);
      this.userControlMaterialData1.Name = "userControlMaterialData1";
      this.userControlMaterialData1.Size = new System.Drawing.Size(448, 66);
      this.userControlMaterialData1.TabIndex = 5;
      // 
      // numericUpDownGamma
      // 
      this.numericUpDownGamma.DecimalPlaces = 2;
      this.numericUpDownGamma.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
      this.numericUpDownGamma.Location = new System.Drawing.Point(102, 246);
      this.numericUpDownGamma.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
      this.numericUpDownGamma.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            65536});
      this.numericUpDownGamma.Name = "numericUpDownGamma";
      this.numericUpDownGamma.Size = new System.Drawing.Size(120, 20);
      this.numericUpDownGamma.TabIndex = 6;
      this.numericUpDownGamma.Value = new decimal(new int[] {
            22,
            0,
            0,
            65536});
      // 
      // LabelGamma
      // 
      this.LabelGamma.AutoSize = true;
      this.LabelGamma.Location = new System.Drawing.Point(52, 248);
      this.LabelGamma.Name = "LabelGamma";
      this.LabelGamma.Size = new System.Drawing.Size(43, 13);
      this.LabelGamma.TabIndex = 7;
      this.LabelGamma.Text = "Gamma";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(868, 290);
      this.Controls.Add(this.LabelGamma);
      this.Controls.Add(this.numericUpDownGamma);
      this.Controls.Add(this.userControlMaterialData1);
      this.Controls.Add(this.checkBoxBouncing);
      this.Controls.Add(this.checkBoxPaused);
      this.Controls.Add(this.userControlLightData3);
      this.Controls.Add(this.userControlLightData2);
      this.Controls.Add(this.userControlLightData1);
      this.Controls.Add(this.buttonTerrain);
      this.Controls.Add(this.buttonTeapot);
      this.Controls.Add(this.buttonRandom);
      this.Name = "Form1";
      this.Text = "Form1";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGamma)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonRandom;
    private System.Windows.Forms.Button buttonTeapot;
    private System.Windows.Forms.Button buttonTerrain;
    private System.Windows.Forms.ColorDialog colorDialog1;
    private UserControlLightData userControlLightData1;
    private UserControlLightData userControlLightData2;
    private UserControlLightData userControlLightData3;
    private System.Windows.Forms.CheckBox checkBoxPaused;
    private System.Windows.Forms.CheckBox checkBoxBouncing;
    private Materials.UserControlMaterialData userControlMaterialData1;
    private System.Windows.Forms.NumericUpDown numericUpDownGamma;
    private System.Windows.Forms.Label LabelGamma;
  }
}

