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
		/// The regions that make up the hit box.
		/// </summary>
		public IEnumerable<Region> Regions = [];
		/// <summary>
		/// Create a hit box with the set of regions.
		/// </summary>
		/// <param name="regions"></param>
		public HitBox(IEnumerable<Region> regions) => Regions = regions;
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
		public bool ClipsThrough(HitBox hitBox, float dx, float dy)
		{
			return Regions.SelectMany(region => region.Points).Any(currentPoint => hitBox.Regions.Any(
				region => region.IntersectedBy(new Line(currentPoint.X, currentPoint.Y, currentPoint.X + dx, currentPoint.Y + dy))));
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
		public bool WillClip(HitBox hitBox, float dx, float dy) => ClipsThrough(hitBox, dx, dy) || hitBox.ClipsThrough(this, -dx, -dy);
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
			// Return the resulting movement
			return (safeX, safeY);
		}
		/// <summary>
		/// Rotate the hit box by the given angle. Stops when the hit box collides with another hit box.
		/// </summary>
		/// <param name="hitBoxes"></param>
		/// <param name="angle"></param>
		/// <param name="margin">Margin of error.</param>
		/// <returns></returns>
		public HitBox Rotate(IEnumerable<HitBox> hitBoxes, float angle, float margin)
		{
			var test = new HitBox(Regions.Select(region => region.Rotate(angle)));
			if (!GetCollisions(hitBoxes, 0, 0).Any())
			{
				return test;
			}
			float safeAngle = 0;
			float testAngle = angle / 2;
			while (Math.Abs(testAngle) > margin)
			{
				test = test.Rotate(hitBoxes, testAngle, margin);
				if (!test.GetCollisions(hitBoxes, 0, 0).Any())
				{
					safeAngle += testAngle;
				}
				testAngle /= 2;
			}
			return test;
		}
	}
}
