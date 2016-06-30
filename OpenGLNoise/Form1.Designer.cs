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
      this.buttonRandom = new System.Windows.Forms.Button();
      this.buttonTeapot = new System.Windows.Forms.Button();
      this.buttonTerrain = new System.Windows.Forms.Button();
      this.colorDialog1 = new System.Windows.Forms.ColorDialog();
      this.userControlLightData1 = new OpenGLNoise.UserControlLightData();
      this.userControlLightData2 = new OpenGLNoise.UserControlLightData();
      this.userControlLightData3 = new OpenGLNoise.UserControlLightData();
      this.checkBoxPaused = new System.Windows.Forms.CheckBox();
      this.checkBoxBouncing = new System.Windows.Forms.CheckBox();
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
      this.userControlLightData1.Size = new System.Drawing.Size(418, 63);
      this.userControlLightData1.TabIndex = 3;
      // 
      // userControlLightData2
      // 
      this.userControlLightData2.ColorName = "Light 2";
      this.userControlLightData2.Location = new System.Drawing.Point(240, 68);
      this.userControlLightData2.Name = "userControlLightData2";
      this.userControlLightData2.Size = new System.Drawing.Size(418, 63);
      this.userControlLightData2.TabIndex = 3;
      // 
      // userControlLightData3
      // 
      this.userControlLightData3.ColorName = "Light 3";
      this.userControlLightData3.Location = new System.Drawing.Point(240, 131);
      this.userControlLightData3.Name = "userControlLightData3";
      this.userControlLightData3.Size = new System.Drawing.Size(418, 63);
      this.userControlLightData3.TabIndex = 3;
      // 
      // checkBoxPaused
      // 
      this.checkBoxPaused.AutoSize = true;
      this.checkBoxPaused.Location = new System.Drawing.Point(89, 221);
      this.checkBoxPaused.Name = "checkBoxPaused";
      this.checkBoxPaused.Size = new System.Drawing.Size(62, 17);
      this.checkBoxPaused.TabIndex = 4;
      this.checkBoxPaused.Text = "Paused";
      this.checkBoxPaused.UseVisualStyleBackColor = true;
      // 
      // checkBoxBouncing
      // 
      this.checkBoxBouncing.AutoSize = true;
      this.checkBoxBouncing.Location = new System.Drawing.Point(240, 221);
      this.checkBoxBouncing.Name = "checkBoxBouncing";
      this.checkBoxBouncing.Size = new System.Drawing.Size(71, 17);
      this.checkBoxBouncing.TabIndex = 4;
      this.checkBoxBouncing.Text = "Bouncing";
      this.checkBoxBouncing.UseVisualStyleBackColor = true;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(696, 263);
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
  }
}

