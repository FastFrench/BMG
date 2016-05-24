using System;
using System.Drawing;
using System.Windows.Forms;
using PerlinNoise;
namespace PerlinNoise.Implementation2
{
    public class Usage
    {
        
        static public void noiseButton_Click(PictureBox pictureBox)
        {
			int width = pictureBox.Width;
			int height = pictureBox.Height;
			int octaveCount = 8;

			Color gradientStart = Color.FromArgb(255, 0, 0);
			Color gradientEnd = Color.FromArgb(255, 0, 255);

			float[][] perlinNoise = PerlinNoise.GeneratePerlinNoise(width, height, octaveCount);
			perlinNoise.Normalize();
			Color[][] perlinImage = PerlinNoise.MapGradient(gradientStart, gradientEnd, perlinNoise);
			
			Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            double widthDivisor = 1 / (double)pictureBox.Width;
            double heightDivisor = 1 / (double)pictureBox.Height;
            bitmap.SetEachPixelColour(
                (point, color) =>
                {
					return perlinImage[point.X][point.Y];
                });
            pictureBox.Image = bitmap;
        }

    }
}
