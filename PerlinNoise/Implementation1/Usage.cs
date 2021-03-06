﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace PerlinNoise.Implementation1
{
    public class Usage
    {
        
        static public void noiseButton_Click(PictureBox pictureBox)
        {
            PerlinNoise3D perlinNoise = new PerlinNoise3D(new Random().Next());
            Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            double widthDivisor = 1 / (double)pictureBox.Width;
            double heightDivisor = 1 / (double)pictureBox.Height;
            bitmap.SetEachPixelColour(
                (point, color) =>
                {
            // Note that the result from the noise function is in the range -1 to 1, but I want it in the range of 0 to 1
            // that's the reason of the strange code
            double v =
                        // First octave
                        perlinNoise.Noise(2 * point.X * widthDivisor, 2 * point.Y * heightDivisor, -0.5) *7 +
                        // Second octave
                        perlinNoise.Noise(4 * point.X * widthDivisor, 4 * point.Y * heightDivisor, 0) *2 +
                        // Third octave
                        perlinNoise.Noise(8 * point.X * widthDivisor, 8 * point.Y * heightDivisor, +0.5) *1;
                    v = (v + 2) / 14;
                    v = Math.Min(1, Math.Max(0, v));
                    byte b = (byte)(v * 255);
                    return Color.FromArgb(b, b, b);
                });
            pictureBox.Image = bitmap;
        }

    }
}
