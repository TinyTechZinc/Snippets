using System.Drawing;

namespace Games.Geometry
{
	/// <summary>
	/// A simple line segment.
	/// </summary>
	public struct Line
	{
		/// <summary>
		/// One of the end points.
		/// </summary>
		public PointF Start { get; set; }
		/// <summary>
		/// The other end point.
		/// </summary>
		public PointF End { get; set; }
		/// <summary>
		/// Creates a new line segment.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public Line(PointF start, PointF end)
		{
			Start = start;
			End = end;
		}
		/// <summary>
		/// Creates a new line segment.
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		public Line(float x1, float y1, float x2, float y2)
		{
			Start = new PointF(x1, y1);
			End = new PointF(x2, y2);
		}
		/// <summary>
		/// Check for intersection with another line segment.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Intersects(Line other)
		{
			// https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection
			var x1 = Start.X;
			var y1 = Start.Y;
			var x2 = End.X;
			var y2 = End.Y;
			var x3 = other.Start.X;
			var y3 = other.Start.Y;
			var x4 = other.End.X;
			var y4 = other.End.Y;
			var d = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
			if (d == 0)
			{
				return false;
			}
			var a = x1 * y2 - y1 * x2;
			var b = x3 * y4 - y3 * x4;
			var x = (a * (x3 - x4) - (x1 - x2) * b) / d;
			var y = (a * (y3 - y4) - (y1 - y2) * b) / d;
			return x >= Math.Min(x1, x2) && x <= Math.Max(x1, x2) && y >= Math.Min(y1, y2) && y <= Math.Max(y1, y2) &&
				   x >= Math.Min(x3, x4) && x <= Math.Max(x3, x4) && y >= Math.Min(y3, y4) && y <= Math.Max(y3, y4);
		}
	}
}
