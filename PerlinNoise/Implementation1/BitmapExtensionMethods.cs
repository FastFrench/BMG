using System;
using System.Drawing;

namespace PerlinNoise.Implementation1
{
    public static class BitmapExtensionMethods
    {
        public static void ExecuteForEachPixel(this Bitmap bitmap, Action<Point, Bitmap> action)
        {
            Point point = new Point(0, 0);
            for (int x = 0; x < bitmap.Width; x++)
            {
                point.X = x;
                for (int y = 0; y < bitmap.Height; y++)
                {
                    point.Y = y;
                    action(point, bitmap);
                }
            }
        }

        public static void ExecuteForEachPixel(this Bitmap bitmap, Action<Point> action)
        {
            Point point = new Point(0, 0);
            for (int x = 0; x < bitmap.Width; x++)
            {
                point.X = x;
                for (int y = 0; y < bitmap.Height; y++)
                {
                    point.Y = y;
                    action(point);
                }
            }
        }

        public static void SetEachPixelColour(this Bitmap bitmap, Func<Point, Color> colourFunc)
        {
            Point point = new Point(0, 0);
            for (int x = 0; x < bitmap.Width; x++)
            {
                point.X = x;
                for (int y = 0; y < bitmap.Height; y++)
                {
                    point.Y = y;
                    bitmap.SetPixel(x, y, colourFunc(point));
                }
            }
        }

        public static void SetEachPixelColour(this Bitmap bitmap, Func<Point, Color, Color> colourFunc)
        {
            Point point = new Point(0, 0);
            for (int x = 0; x < bitmap.Width; x++)
            {
                point.X = x;
                for (int y = 0; y < bitmap.Height; y++)
                {
                    point.Y = y;
                    bitmap.SetPixel(x, y, colourFunc(point, bitmap.GetPixel(x, y)));
                }
            }
        }
    }

}

/* sample usage:
 private void renderDistanceFromCenterButton_Click(object sender, EventArgs e)
{
    // Build the bitmap
    Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
    Point imageCenter = new Point(pictureBox.Width / 2, pictureBox.Height / 2);
 
    // Ok, this is terrible, you _really_ shouldn't use lambda like this... But you can...
    Func<double, double, double> distance = (x, y) => Math.Sqrt(x * x + y * y);
 
    double maxDistance = distance(imageCenter.X, imageCenter.Y);
    bitmap.SetEachPixelColour(
        point =>
        {
            // Pythagoras
            double dist = distance((point.X - imageCenter.X), (point.Y - imageCenter.Y));
            byte b = Convert.ToByte(255 * dist / maxDistance);
            return Color.FromArgb(b, b, b);
        });
    pictureBox.Image = bitmap;
}
*/