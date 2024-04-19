using System.Drawing;

namespace Games.Geometry
{
	/// <summary>
	/// A hit box class that detects clipping between hit boxes.
	/// 
	/// Made up of one or more regions.
	/// </summary>
	public class HitBox
	{
		/// <summary>
		/// A rectangle representing the outer bounds of the hit box.
		/// </summary>
		public RectangleF Bounds => Regions.Select(region => region.Bounds).Aggregate((a, b) => RectangleF.Union(a, b));
		/// <summary>
		/// The regions that make up the hit box.
		/// </summary>
		public List<Region> Regions = [];
		/// <summary>
		/// Get all the lines in this hit box.
		/// </summary>
		public IEnumerable<Line> Lines => Regions.SelectMany(region => region.Lines);
		/// <summary>
		/// Create a hit box with the set of regions.
		/// </summary>
		/// <param name="regions"></param>
		public HitBox(IEnumerable<Region> regions) => Regions = regions.ToList();
		/// <summary>
		/// Check if the hit box intersects with another hit box.
		/// Assumes the hit boxes have the same origin.
		/// </summary>
		/// <param name="hitBox"></param>
		/// <returns></returns>
		public bool Intersects(HitBox hitBox) => Regions.Any(r => hitBox.Regions.Any(r.Intersects));
		/// <summary>
		/// Check if moving this hit box clips into/through another hit box.
		/// 
		/// It is possible for the other hit box to clip into this hit box without this hit box clipping into the other hit box.
		/// </summary>
		/// <param name="hitBox"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		[Obsolete]
		public bool ClipsThrough(HitBox hitBox, float dx, float dy)
		{
			// Get all the points in this hitbox
			var points = Regions.SelectMany(region => region.Points).ToList();
			// Draw lines from each point to its new location
			foreach (var point in points)
			{
				var line = new Line(point.X, point.Y, point.X + dx, point.Y + dy);
				// Check if the line intersects with any of the lines in the other hitbox
				if (hitBox.Regions.Any(region => region.IntersectedBy(line)))
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Offset the hit box by the given amount.
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public HitBox Offset(float dx, float dy) => new HitBox(Regions.Select(region => region.Offset(dx, dy)));
		/// <summary>
		/// Check if moving this hit box will cause the hit boxes to clip into each other.
		/// </summary>
		/// <param name="hitBox"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public bool WillClip(HitBox hitBox, float dx, float dy)
		{
			// Get all the points in this hitbox and create lines to each one's new location
			var myLines = Regions.SelectMany(region => region.Points).Select(point => new Line(point.X, point.Y, point.X + dx, point.Y + dy)).ToList();
			// Do the same for the other hitbox
			var otherLines = hitBox.Regions.SelectMany(region => region.Points).Select(point => new Line(point.X, point.Y, point.X - dx, point.Y - dy)).ToList();
			// Check if the movement lines intersect any of the other hitbox's normal lines
			foreach (var lineM in myLines)
			{
				if (hitBox.Lines.Any(lineM.Intersects))
				{
					return true;
				}
			}
			foreach (var lineO in otherLines)
			{
				if (Lines.Any(lineO.Intersects))
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Get the hit boxes that will be clipped by this hit box when moves.
		/// </summary>
		/// <param name="hitBoxes"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public IEnumerable<HitBox> GetCollisions(IEnumerable<HitBox> hitBoxes, float dx, float dy) => hitBoxes.Where(hitBox => WillClip(hitBox, dx, dy));
		/// <summary>
		/// Handle the collision between this hit box and the other hit boxes.
		/// </summary>
		/// <param name="hitBoxes"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <param name="margin">Margin for error.</param>
		/// <param name="doSlide">Whether to move sideways after colliding.</param>
		/// <param name="friction">Movement kept after colliding; used to determine slide distance.</param>
		/// <returns>The change in position, accounting for collisions.</returns>
		public (float dX, float dY) Collide(IEnumerable<HitBox> hitBoxes, float dx, float dy, float margin, bool doSlide = false, float friction = 1)
		{
			// Check for collisions
			if (!GetCollisions(hitBoxes, dx, dy).Any())
			{
				return (dx, dy);
			}
			// Reduce the movement to the point of collision
			float safeX = 0;
			float safeY = 0;
			float testX = dx / 2;
			float testY = dy / 2;
			// Within some margin of error
			while (Math.Sqrt(testX * testX + testY * testY) > margin)
			{
				// Check for collisions
				if (!GetCollisions(hitBoxes, safeX + testX, safeY + testY).Any())
				{
					safeX += testX;
					safeY += testY;
				}
				testX /= 2;
				testY /= 2;
			}
			if (doSlide)
			{
				// Move along x-axis
				testX = friction * (dx - safeX) / 2;
				while (Math.Abs(testX) > margin)
				{
					if (!GetCollisions(hitBoxes, safeX + testX, safeY).Any())
					{
						safeX += testX;
					}
					testX /= 2;
				}
				// Move along y-axis
				testY = friction * (dy - safeY) / 2;
				while (Math.Abs(testY) > margin)
				{
					if (!GetCollisions(hitBoxes, safeX, safeY + testY).Any())
					{
						safeY += testY;
					}
					testY /= 2;
				}
			}
			if (hitBoxes.Any(hitBoxes => hitBoxes.Intersects(this.Offset(safeX, safeY))))
			{
				//throw new Exception("Collision not resolved.");
			}
			// Return the resulting movement
			return (safeX, safeY);
		}
		/// <summary>
		/// Rotate the hit box by the given angle. Stops when the hit box collides with another hit box.
		/// </summary>
		/// <param name="hitBoxes"></param>
		/// <param name="angle"></param>
		/// <param name="step"></param>
		/// <returns></returns>
		public HitBox RotateCollide(IEnumerable<HitBox> hitBoxes, float angle, float step)
		{
			if (hitBoxes.Any(Intersects))
			{
				return this;
			}
			angle %= (float)(2 * Math.PI);
			var safe = this;
			var test = this;
			float safeAngle = 0;
			float testAngle = 0;
			while (!hitBoxes.Any(test.Intersects) && safeAngle != angle)
			{
				safe = test;
				safeAngle = testAngle;
				testAngle = (safeAngle + step) % (float)(2 * Math.PI);
				if (testAngle > angle)
				{
					testAngle = angle;
				}
				test = Rotate(testAngle);
			}
			return safe;
		}
		/// <summary>
		/// Rotate the hit box by the given angle.
		/// 
		/// The rotation is around the center of the hit box.
		/// </summary>
		/// <param name="angle"></param>
		/// <returns>A new hit box with the rotation applied.</returns>
		public HitBox Rotate(float angle) => new HitBox(Regions.Select(region => region.Rotate(angle, Bounds.Width / 2 - Bounds.Location.X, Bounds.Height / 2 - Bounds.Location.Y)));
	}
}
