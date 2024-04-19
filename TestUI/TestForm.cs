using Games.Geometry;

namespace TestUI
{
	class Mover
	{
		public float dX;
		public float dY;
		public HitBox hitBox;
		public float dR = 10;
	}
	public partial class TestForm : Form
	{
		HitBox player = new HitBox([RegionFactory.CreateRectangle(50, 50, 50, 50)]);
		List<HitBox> Walls = [
			new HitBox([RegionFactory.CreateRectangle(0, 0, 500, 500)])
			];
		List<Mover> Movers = new List<Mover>();
		public TestForm()
		{
			InitializeComponent();
		}
		private void DrawHitBox(Graphics graphics, HitBox hitBox)
		{
			foreach (var line in hitBox.Regions.SelectMany(r => r.Lines))
			{
				graphics.DrawLine(Pens.Black, line.Start.X, line.Start.Y, line.End.X, line.End.Y);
			}
		}

		private void TestForm_Load(object sender, EventArgs e)
		{
			
		}

		private void pictureBox_Paint(object sender, PaintEventArgs e)
		{
			foreach (var h in Walls.Concat(Movers.Select(m => m.hitBox)))
			{
				DrawHitBox(e.Graphics, h);
			}
			DrawHitBox(e.Graphics, player);
			var b = new HitBox([RegionFactory.CreateRectangle(100, 100, 30, 30)]);
			DrawHitBox(e.Graphics, b);
			DrawHitBox(e.Graphics, b.Offset(100, 100));
			DrawHitBox(e.Graphics, b.Rotate(0.1f));
			var r = new HitBox([RegionFactory.CreateRectangle(100, 200, 30, 30)]);
			DrawHitBox(e.Graphics, r);
			DrawHitBox(e.Graphics, new HitBox([RegionFactory.CreateRectangle(100, 200, 30, 30).Rotate(0.2f, -100 + 30, -200 + 30)]));
        }

		private void TestForm_KeyUp(object sender, KeyEventArgs e)
		{
			var AllBoxes = Walls.Concat(Movers.Select(m => m.hitBox));
			(float dX, float dY) safe = (0, 0);
			switch (e.KeyCode)
			{
				case Keys.W:
					safe = player.Collide(AllBoxes, 0, -60, 0.5f, true);
					break;
				case Keys.A:
					safe = player.Collide(AllBoxes, -60, 0, 0.5f, true);
					break;
				case Keys.S:
					safe = player.Collide(AllBoxes, 0, 60, 0.5f, true);
					break;
				case Keys.D:
					safe = player.Collide(AllBoxes, 60, 0, 0.5f, true);
					break;
				default:
					break;
			}
			player = player.Offset(safe.dX, safe.dY);
			// Add the player hit box so it can be checked against
			AllBoxes = AllBoxes.Append(player);
			for(int i = 0; i < Movers.Count; i++)
			{
				var m = Movers[i];
				m.hitBox = m.hitBox.RotateCollide(AllBoxes.Where(h => h != m.hitBox), m.dR, 0.5f);
				var (dX, dY) = m.hitBox.Collide(AllBoxes.Where(h => h != m.hitBox), m.dX, m.dY, 0.5f, true);
				m.hitBox = m.hitBox.Offset(dX, dY);
			}
			pictureBox.Refresh();
		}

		private void pictureBox_Click(object sender, EventArgs e)
		{
		}

		private void pictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			// TODO: fix bug where mover can clip through walls (seems to happen if dx >= width)
			Movers.Add(new Mover { dX = 21, dY = 10, hitBox = new HitBox([RegionFactory.CreateRectangle(e.X, e.Y, 20, 20)]) });
		}
	}
}
