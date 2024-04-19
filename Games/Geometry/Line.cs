using System.Drawing;
using static System.Net.WebRequestMethods;

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
		/// <br/>
		/// <see href="https://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/"/>
		/// </summary>
		/// <param name="other"></param>
		/// <returns>True if the lines intersect.</returns>
		public bool Intersects(Line other)
		{
			static bool CoLinearPointOnSegment(PointF start, PointF end, PointF point)
			{
				return point.X <= Math.Max(start.X, end.X) && point.X >= Math.Min(start.X, end.X)
					&& point.Y <= Math.Max(start.Y, end.Y) && point.Y >= Math.Min(start.Y, end.Y);
			}
			static int GetOrientation(PointF a, PointF b, PointF c)
			{
				float val = (b.Y - a.Y) * (c.X - b.X) - (b.X - a.X) * (c.Y - b.Y);
				if (val == 0) return 0; // Co-linear
				return (val > 0) ? 1 : 2; // Clockwise or counter-clockwise
			}
			var o1 = GetOrientation(Start, End, other.Start);
			var o2 = GetOrientation(Start, End, other.End);
			var o3 = GetOrientation(other.Start, other.End, Start);
			var o4 = GetOrientation(other.Start, other.End, End);

			if (o1 != o2 && o3 != o4) return true;

			if (o1 == 0 && CoLinearPointOnSegment(Start, End, other.Start)) return true;
			if (o2 == 0 && CoLinearPointOnSegment(Start, End, other.End)) return true;
			if (o3 == 0 && CoLinearPointOnSegment(other.Start, other.End, Start)) return true;
			if (o4 == 0 && CoLinearPointOnSegment(other.Start, other.End, End)) return true;

			return false;
		}
	}
}
