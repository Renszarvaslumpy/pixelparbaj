using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace PixelParbaj_CORE.Models
{
    public partial class Movie
    {
		[Key]
        public long Id { get; set; }
        public string? Imdb { get; set; }
        public string? TitleH { get; set; }
        public string? TitleE { get; set; }
        public int? Year { get; set; }
        public string? Votes { get; set; }
		public int? VotesInt { get; set; }
        public byte[]? Poster { get; set; }

		private static int PHASES = 10;

		public string GetPoster()
		{
			return Convert.ToBase64String(Poster);
			//return Base64UrlEncoder.Encode(Poster);
		}

		public List<string> GetPixelatedPosters()
		{
			Bitmap orig;
			Bitmap poster;
			using (var ms = new MemoryStream(Poster))
			{
				orig = new Bitmap(ms);
				poster = new Bitmap(orig.Width, orig.Height, PixelFormat.Format32bppPArgb);

				using (Graphics gr = Graphics.FromImage(poster))
				{
					gr.DrawImage(orig, new Rectangle(0, 0, poster.Width, poster.Height));
				}
				/*using (MemoryStream stream = new MemoryStream())
				{
					JPEGposter.Save(stream, ImageFormat.Bmp);
					poster = new Bitmap(stream);
				}*/
			}

			List<string> pixelatedPosters = new List<string>() { };

			for (int i = 1; i < PHASES; i++)
			{
				Bitmap px = PixelateLockBits(poster, new Rectangle(new Point(0, 0), poster.Size), 2 * i);
				using (var ms = new MemoryStream())
				{
					px.Save(ms, ImageFormat.Jpeg);
					var pixelated = Convert.ToBase64String(ms.GetBuffer()); //Get Base64
					pixelatedPosters.Add(pixelated);
				}
				//px.Dispose();
			}
			poster.Dispose();
			return pixelatedPosters;
		}

		private static Bitmap PixelateLockBits(Bitmap image, Rectangle rectangle, int pixelateSize)
		{
			using (LockBitmap lockBitmap = new LockBitmap(image))
			{
				var width = image.Width;
				var height = image.Height;

				for (Int32 xx = rectangle.X; xx < rectangle.X + rectangle.Width && xx < image.Width; xx += pixelateSize)
				{
					for (Int32 yy = rectangle.Y; yy < rectangle.Y + rectangle.Height && yy < image.Height; yy += pixelateSize)
					{
						Int32 offsetX = pixelateSize / 2;
						Int32 offsetY = pixelateSize / 2;

						// make sure that the offset is within the boundry of the image
						while (xx + offsetX >= image.Width) offsetX--;
						while (yy + offsetY >= image.Height) offsetY--;

						// get the pixel color in the center of the soon to be pixelated area
						Color pixel = lockBitmap.GetPixel(xx + offsetX, yy + offsetY);

						// for each pixel in the pixelate size, set it to the center color
						for (Int32 x = xx; x < xx + pixelateSize && x < image.Width; x++)
							for (Int32 y = yy; y < yy + pixelateSize && y < image.Height; y++)
								lockBitmap.SetPixel(x, y, pixel);
					}
				}
			}
			return image;
		}
	}

	public class LockBitmap : IDisposable
	{
		Bitmap source = null;
		IntPtr Iptr = IntPtr.Zero;
		BitmapData bitmapData = null;
		int step = 0;

		public byte[] Pixels { get; set; }
		public int Depth { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }

		public LockBitmap(Bitmap source)
		{
			this.source = source;
			LockBits();
		}

		/// <summary>
		/// Lock bitmap data
		/// </summary>
		private void LockBits()
		{
			try
			{
				// Get width and height of bitmap
				Width = source.Width;
				Height = source.Height;

				// get total locked pixels count
				int PixelCount = Width * Height;

				// Create rectangle to lock
				Rectangle rect = new Rectangle(0, 0, Width, Height);

				// get source bitmap pixel format size
				Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);

				// Check if bpp (Bits Per Pixel) is 8, 24, or 32
				if (Depth != 8 && Depth != 24 && Depth != 32)
				{
					throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
				}

				// Lock bitmap and return bitmap data
				bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
													  source.PixelFormat);

				// create byte array to copy pixel values
				step = Depth / 8;
				Pixels = new byte[PixelCount * step];
				Iptr = bitmapData.Scan0;

				// Copy data from pointer to array
				Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Unlock bitmap data
		/// </summary>
		private void UnlockBits()
		{
			try
			{
				// Copy data from byte array to pointer
				Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);

				// Unlock bitmap data
				source.UnlockBits(bitmapData);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Get the color of the specified pixel
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public Color GetPixel(int x, int y)
		{
			Color clr = Color.Empty;

			// Get start index of the specified pixel
			int i = ((y * Width) + x) * step;

			if (i > Pixels.Length - step)
				throw new IndexOutOfRangeException();

			if (Depth == 32) // For 32 bpp get Red, Green, Blue and Alpha
			{
				byte b = Pixels[i];
				byte g = Pixels[i + 1];
				byte r = Pixels[i + 2];
				byte a = Pixels[i + 3]; // a
				clr = Color.FromArgb(a, r, g, b);
			}
			if (Depth == 24) // For 24 bpp get Red, Green and Blue
			{
				byte b = Pixels[i];
				byte g = Pixels[i + 1];
				byte r = Pixels[i + 2];
				clr = Color.FromArgb(r, g, b);
			}
			if (Depth == 8)
			// For 8 bpp get color value (Red, Green and Blue values are the same)
			{
				byte c = Pixels[i];
				clr = Color.FromArgb(c, c, c);
			}
			return clr;
		}

		/// <summary>
		/// Set the color of the specified pixel
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="color"></param>
		public void SetPixel(int x, int y, Color color)
		{
			// Get start index of the specified pixel
			int i = ((y * Width) + x) * step;

			if (Depth == 32) // For 32 bpp set Red, Green, Blue and Alpha
			{
				Pixels[i] = color.B;
				Pixels[i + 1] = color.G;
				Pixels[i + 2] = color.R;
				Pixels[i + 3] = color.A;
			}
			if (Depth == 24) // For 24 bpp set Red, Green and Blue
			{
				Pixels[i] = color.B;
				Pixels[i + 1] = color.G;
				Pixels[i + 2] = color.R;
			}
			if (Depth == 8)
			// For 8 bpp set color value (Red, Green and Blue values are the same)
			{
				Pixels[i] = color.B;
			}
		}

		public void Dispose()
		{
			UnlockBits();
		}
	}
}