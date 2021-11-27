using System;
using System.Drawing;

namespace CervusDama.Utility.ImageHelper
{
	public class ImageHelper
	{
		public static void ImageCrop(string sourcePath, string targetPath, int width, int height)
		{
			Size newImageSize = new Size(width, height);

			System.Drawing.Image image = Image.FromFile(sourcePath);

			float ratio = 0, ratioWidth = (float)newImageSize.Width / image.Width, ratioHeight = (float)newImageSize.Height / image.Height;

			if (ratioHeight < ratioWidth)
				ratio = ratioHeight;
			else
				ratio = ratioWidth;

			Bitmap newImage = new Bitmap((int)(image.Width * ratio), (int)(image.Height * ratio));

			Graphics g = Graphics.FromImage((System.Drawing.Image)newImage);
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			g.DrawImage(image, 0, 0, (int)(image.Width * ratio), (int)(image.Height * ratio));

			newImage.Save(targetPath);
		}

		public static bool ImageDelete(string fileName)
		{
			if (System.IO.File.Exists(fileName))
			{
				try
				{
					System.IO.File.Delete(fileName);
					return true;
				}
				catch (Exception e)
				{
					return false;
				}
			}

			return false;
		}
	}
}
