using PerlinNoise.Implementation1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerlinNoise
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonTest1_Click(object sender, EventArgs e)
        {
			Implementation1.Usage.noiseButton_Click(pictureBox1);
        }

		private void buttonTest2_Click(object sender, EventArgs e)
		{
			Implementation2.Usage.noiseButton_Click(pictureBox1);
		}
	}
}
