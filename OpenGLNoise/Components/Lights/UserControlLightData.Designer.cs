namespace OpenGLNoise
{
  partial class UserControlLightData
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.checkBoxVisible = new System.Windows.Forms.CheckBox();
      this.textBoxX = new System.Windows.Forms.NumericUpDown();
      this.textBoxY = new System.Windows.Forms.NumericUpDown();
      this.textBoxZ = new System.Windows.Forms.NumericUpDown();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.buttonSetColor = new System.Windows.Forms.Button();
      this.colorDialog = new System.Windows.Forms.ColorDialog();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.buttonSetSpecularColor = new System.Windows.Forms.Button();
      this.buttonSetDiffuseColor = new System.Windows.Forms.Button();
      this.buttonSetAmbientColor = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.textBoxX)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.textBoxY)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.textBoxZ)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // checkBoxVisible
      // 
      this.checkBoxVisible.AutoSize = true;
      this.checkBoxVisible.Location = new System.Drawing.Point(249, 26);
      this.checkBoxVisible.Name = "checkBoxVisible";
      this.checkBoxVisible.Size = new System.Drawing.Size(56, 17);
      this.checkBoxVisible.TabIndex = 0;
      this.checkBoxVisible.Text = "Visible";
      this.checkBoxVisible.UseVisualStyleBackColor = true;
      // 
      // textBoxX
      // 
      this.textBoxX.DecimalPlaces = 2;
      this.textBoxX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
      this.textBoxX.Location = new System.Drawing.Point(23, 24);
      this.textBoxX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
      this.textBoxX.Name = "textBoxX";
      this.textBoxX.Size = new System.Drawing.Size(55, 20);
      this.textBoxX.TabIndex = 1;
      this.textBoxX.TextChanged += new System.EventHandler(this.textBoxX_TextChanged);
      // 
      // textBoxY
      // 
      this.textBoxY.DecimalPlaces = 2;
      this.textBoxY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
      this.textBoxY.Location = new System.Drawing.Point(102, 24);
      this.textBoxY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
      this.textBoxY.Name = "textBoxY";
      this.textBoxY.Size = new System.Drawing.Size(55, 20);
      this.textBoxY.TabIndex = 2;
      this.textBoxY.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
      // 
      // textBoxZ
      // 
      this.textBoxZ.DecimalPlaces = 2;
      this.textBoxZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
      this.textBoxZ.Location = new System.Drawing.Point(179, 24);
      this.textBoxZ.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
      this.textBoxZ.Name = "textBoxZ";
      this.textBoxZ.Size = new System.Drawing.Size(55, 20);
      this.textBoxZ.TabIndex = 3;
      this.textBoxZ.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(9, 28);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(12, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "x";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(87, 28);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(12, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "x";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(84, 28);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(12, 13);
      this.label6.TabIndex = 4;
      this.label6.Text = "y";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(164, 28);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(12, 13);
      this.label8.TabIndex = 4;
      this.label8.Text = "z";
      // 
      // buttonSetColor
      // 
      this.buttonSetColor.Location = new System.Drawing.Point(311, 21);
      this.buttonSetColor.Name = "buttonSetColor";
      this.buttonSetColor.Size = new System.Drawing.Size(92, 27);
      this.buttonSetColor.TabIndex = 5;
      this.buttonSetColor.Text = "Color";
      this.buttonSetColor.UseVisualStyleBackColor = true;
      this.buttonSetColor.Click += new System.EventHandler(this.buttonSetColor_Click);
      // 
      // colorDialog
      // 
      this.colorDialog.AnyColor = true;
      this.colorDialog.FullOpen = true;
      this.colorDialog.SolidColorOnly = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.textBoxY);
      this.groupBox1.Controls.Add(this.buttonSetSpecularColor);
      this.groupBox1.Controls.Add(this.buttonSetDiffuseColor);
      this.groupBox1.Controls.Add(this.buttonSetAmbientColor);
      this.groupBox1.Controls.Add(this.buttonSetColor);
      this.groupBox1.Controls.Add(this.checkBoxVisible);
      this.groupBox1.Controls.Add(this.label8);
      this.groupBox1.Controls.Add(this.textBoxX);
      this.groupBox1.Controls.Add(this.textBoxZ);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.label6);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Location = new System.Drawing.Point(3, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(605, 56);
      this.groupBox1.TabIndex = 6;
      this.groupBox1.TabStop = false;
      // 
      // buttonSetSpecularColor
      // 
      this.buttonSetSpecularColor.Location = new System.Drawing.Point(533, 25);
      this.buttonSetSpecularColor.Name = "buttonSetSpecularColor";
      this.buttonSetSpecularColor.Size = new System.Drawing.Size(64, 19);
      this.buttonSetSpecularColor.TabIndex = 5;
      this.buttonSetSpecularColor.Text = "Specular";
      this.buttonSetSpecularColor.UseVisualStyleBackColor = true;
      this.buttonSetSpecularColor.Click += new System.EventHandler(this.buttonSetSpecularColor_Click);
      // 
      // buttonSetDiffuseColor
      // 
      this.buttonSetDiffuseColor.Location = new System.Drawing.Point(471, 25);
      this.buttonSetDiffuseColor.Name = "buttonSetDiffuseColor";
      this.buttonSetDiffuseColor.Size = new System.Drawing.Size(56, 19);
      this.buttonSetDiffuseColor.TabIndex = 5;
      this.buttonSetDiffuseColor.Text = "Diffuse";
      this.buttonSetDiffuseColor.UseVisualStyleBackColor = true;
      this.buttonSetDiffuseColor.Click += new System.EventHandler(this.buttonSetDiffuseColor_Click);
      // 
      // buttonSetAmbientColor
      // 
      this.buttonSetAmbientColor.Location = new System.Drawing.Point(409, 25);
      this.buttonSetAmbientColor.Name = "buttonSetAmbientColor";
      this.buttonSetAmbientColor.Size = new System.Drawing.Size(56, 19);
      this.buttonSetAmbientColor.TabIndex = 5;
      this.buttonSetAmbientColor.Text = "Ambiant";
      this.buttonSetAmbientColor.UseVisualStyleBackColor = true;
      this.buttonSetAmbientColor.Click += new System.EventHandler(this.buttonSetAmbiantColor_Click);
      // 
      // UserControlLightData
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "UserControlLightData";
      this.Size = new System.Drawing.Size(668, 66);
      ((System.ComponentModel.ISupportInitialize)(this.textBoxX)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.textBoxY)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.textBoxZ)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.CheckBox checkBoxVisible;
    private System.Windows.Forms.NumericUpDown textBoxX;
    private System.Windows.Forms.NumericUpDown textBoxY;
    private System.Windows.Forms.NumericUpDown textBoxZ;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Button buttonSetColor;
    private System.Windows.Forms.ColorDialog colorDialog;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button buttonSetAmbientColor;
    private System.Windows.Forms.Button buttonSetSpecularColor;
    private System.Windows.Forms.Button buttonSetDiffuseColor;
  }
}
