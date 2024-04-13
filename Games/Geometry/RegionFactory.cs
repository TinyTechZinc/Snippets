namespace Games.Geometry
{
	/// <summary>
	/// Helper class to create simple regions.
	/// </summary>
	public static class RegionFactory
	{
		/// <summary>
		/// Create a region that forms a rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static Region CreateRectangle(float x, float y, float width, float height)
		{
			float x2 = x + width;
			float y2 = y + height;
			return new Region([
				new Line(x, y, x, y2),
				new Line(x, y, x2, y),
				new Line(x2, y, x2, y2),
				new Line(x, y2, x2, y2)
			]);
		}
	}
}
