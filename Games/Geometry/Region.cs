using System.Drawing;

namespace Games.Geometry
{
	/// <summary>
	/// Stores arbitrary lines to represent a region.
	/// The region does not have to be closed.
	/// </summary>
	public class Region
	{
		IEnumerable<Line> _Lines = [];
		IEnumerable<PointF> _Points = [];
		RectangleF _Bounds = RectangleF.Empty;
		/// <summary>
		/// Lines defining the region.
		/// </summary>
		public IEnumerable<Line> Lines
		{
			get => _Lines;
			set
			{
				_Lines = value.Distinct();
				_Points = _Lines.SelectMany(line => new[] { line.Start, line.End }).Distinct();
				_Bounds = new RectangleF(
					_Points.Min(point => point.X),
					_Points.Min(point => point.Y),
					_Points.Max(point => point.X) - _Points.Min(point => point.X),
					_Points.Max(point => point.Y) - _Points.Min(point => point.Y));
			}
		}
		/// <summary>
		/// Distinct end points of the lines.
		/// </summary>
		public IEnumerable<PointF> Points => _Points;
		/// <summary>
		/// Rectangle representing the outer bounds the region.
		/// </summary>
		public RectangleF Bounds => _Bounds;
		/// <summary>
		/// Create a region with the set of lines.
		/// </summary>
		/// <param name="lines"></param>
		public Region(IEnumerable<Line> lines) => Lines = lines;
		/// <summary>
		/// Checks if the point in within the outer bounds of the region.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public bool MayContain(PointF point) => Bounds.Contains(point);
		/// <summary>
		/// Checks if the outer bounds of one region is fully contained within the outer bounds of this region.
		/// </summary>
		/// <param name="region"></param>
		/// <returns></returns>
		public bool MayContain(Region region) => Bounds.Contains(region.Bounds);
		/// <summary>
		/// Checks if the outer bounds of one region intersects with the outer bounds of this region.
		/// </summary>
		/// <param name="region"></param>
		/// <returns></returns>
		public bool MayIntersect(Region region) => Bounds.IntersectsWith(region.Bounds);
		/// <summary>
		/// Checks if the line intersects with the outer bounds of the region.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public bool MayIntersect(Line line)
		{
			// Check using line segments of bounds
			return line.Intersects(new Line(Bounds.X, Bounds.Y, Bounds.X, Bounds.Bottom)) ||
				   line.Intersects(new Line(Bounds.X, Bounds.Y, Bounds.Right, Bounds.Y)) ||
				   line.Intersects(new Line(Bounds.Right, Bounds.Y, Bounds.Right, Bounds.Bottom)) ||
				   line.Intersects(new Line(Bounds.X, Bounds.Bottom, Bounds.Right, Bounds.Bottom));
		}
		/// <summary>
		/// Checks if the other region may intersect or may be contained within this region.
		/// </summary>
		/// <param name="region"></param>
		/// <returns></returns>
		public bool MayOverlap(Region region) => MayIntersect(region) || MayContain(region);
		/// <summary>
		/// Checks if the line intersects with any of the lines in the region.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public bool IntersectedBy(Line line)
		{
			if (!MayIntersect(line))
			{
				return false;
			}
			return Lines.Any(line.Intersects);
		}
		/// <summary>
		/// Checks if any of the line of the other region intersects with any of the lines of this region.
		/// </summary>
		/// <param name="region"></param>
		/// <returns></returns>
		public bool Intersects(Region region)
		{
			if (!MayIntersect(region))
			{
				return false;
			}
			return Lines.Any(region.IntersectedBy);
		}
		/// <summary>
		/// Offset the region by the given amount.
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public Region Offset(float dx, float dy)
		{
			return new Region(Lines.Select(line => new Line(line.Start.X + dx, line.Start.Y + dy, line.End.X + dx, line.End.Y + dy)));
		}
		/// <summary>
		/// Rotate the region by the given angle. Assumes the origin is at (0, 0).
		/// </summary>
		/// <param name="angle">Angle in radians</param>
		/// <returns></returns>
		public Region Rotate(float angle)
		{
			var cos = (float)Math.Cos(angle);
			var sin = (float)Math.Sin(angle);
			return new Region(Lines.Select(line => new Line(
				line.Start.X * cos - line.Start.Y * sin,
				line.Start.X * sin + line.Start.Y * cos,
				line.End.X * cos - line.End.Y * sin,
				line.End.X * sin + line.End.Y * cos)));
		}
	}
}
