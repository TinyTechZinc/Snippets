using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace Extensions
{
	public static class ColorExtensions
	{
		[SupportedOSPlatform("windows")]
		public static ImageAttributes BuildAttributes(this Color color)
		{
			ColorMatrix matrix = new(
			[
				[color.R / 255f, 0, 0, 0, 0],
				[0, color.G / 255f, 0, 0, 0],
				[0, 0, color.B / 255f, 0, 0],
				[0, 0, 0, color.A / 255f, 0],
				[0, 0, 0, 0, 1]
			]);
			ImageAttributes toReturn = new();
			toReturn.SetColorMatrix(matrix);
			return toReturn;
		}
	}
}
