namespace OpenGLNoise.Materials
{
  partial class UserControlMaterialData
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
      this.textBoxX = new System.Windows.Forms.NumericUpDown();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.buttonSetColor = new System.Windows.Forms.Button();
      this.colorDialog = new System.Windows.Forms.ColorDialog();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.buttonSetSpecularColor = new System.Windows.Forms.Button();
      this.buttonSetDiffuseColor = new System.Windows.Forms.Button();
      this.buttonSetAmbientColor = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.textBoxX)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // textBoxX
      // 
      this.textBoxX.DecimalPlaces = 2;
      this.textBoxX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
      this.textBoxX.Location = new System.Drawing.Point(67, 22);
      this.textBoxX.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
      this.textBoxX.Name = "textBoxX";
      this.textBoxX.Size = new System.Drawing.Size(55, 20);
      this.textBoxX.TabIndex = 1;
      this.textBoxX.TextChanged += new System.EventHandler(this.textBoxX_TextChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(9, 26);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(52, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Shininess";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(87, 26);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(12, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "x";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(84, 26);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(12, 13);
      this.label6.TabIndex = 4;
      this.label6.Text = "y";
      // 
      // buttonSetColor
      // 
      this.buttonSetColor.Location = new System.Drawing.Point(143, 19);
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
      this.groupBox1.Controls.Add(this.buttonSetSpecularColor);
      this.groupBox1.Controls.Add(this.buttonSetDiffuseColor);
      this.groupBox1.Controls.Add(this.buttonSetAmbientColor);
      this.groupBox1.Controls.Add(this.buttonSetColor);
      this.groupBox1.Controls.Add(this.textBoxX);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.label6);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Location = new System.Drawing.Point(3, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(439, 60);
      this.groupBox1.TabIndex = 6;
      this.groupBox1.TabStop = false;
      // 
      // buttonSetSpecularColor
      // 
      this.buttonSetSpecularColor.Location = new System.Drawing.Point(365, 23);
      this.buttonSetSpecularColor.Name = "buttonSetSpecularColor";
      this.buttonSetSpecularColor.Size = new System.Drawing.Size(64, 19);
      this.buttonSetSpecularColor.TabIndex = 5;
      this.buttonSetSpecularColor.Text = "Specular";
      this.buttonSetSpecularColor.UseVisualStyleBackColor = true;
      this.buttonSetSpecularColor.Click += new System.EventHandler(this.buttonSetSpecularColor_Click);
      // 
      // buttonSetDiffuseColor
      // 
      this.buttonSetDiffuseColor.Location = new System.Drawing.Point(303, 23);
      this.buttonSetDiffuseColor.Name = "buttonSetDiffuseColor";
      this.buttonSetDiffuseColor.Size = new System.Drawing.Size(56, 19);
      this.buttonSetDiffuseColor.TabIndex = 5;
      this.buttonSetDiffuseColor.Text = "Diffuse";
      this.buttonSetDiffuseColor.UseVisualStyleBackColor = true;
      this.buttonSetDiffuseColor.Click += new System.EventHandler(this.buttonSetDiffuseColor_Click);
      // 
      // buttonSetAmbientColor
      // 
      this.buttonSetAmbientColor.Location = new System.Drawing.Point(241, 23);
      this.buttonSetAmbientColor.Name = "buttonSetAmbientColor";
      this.buttonSetAmbientColor.Size = new System.Drawing.Size(56, 19);
      this.buttonSetAmbientColor.TabIndex = 5;
      this.buttonSetAmbientColor.Text = "Ambiant";
      this.buttonSetAmbientColor.UseVisualStyleBackColor = true;
      this.buttonSetAmbientColor.Click += new System.EventHandler(this.buttonSetAmbiantColor_Click);
      // 
      // UserControlMaterialData
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "UserControlMaterialData";
      this.Size = new System.Drawing.Size(448, 66);
      ((System.ComponentModel.ISupportInitialize)(this.textBoxX)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.NumericUpDown textBoxX;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Button buttonSetColor;
    private System.Windows.Forms.ColorDialog colorDialog;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button buttonSetAmbientColor;
    private System.Windows.Forms.Button buttonSetSpecularColor;
    private System.Windows.Forms.Button buttonSetDiffuseColor;
  }
}
