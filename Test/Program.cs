using Games.WordGuessingGame;
using Games.Geometry;
using Extensions;

TestCollision();

void TestGame()
{
	WordGame game = new WordGame();

	do
	{
		Console.WriteLine("Enter Word");
		string? word = Console.ReadLine();
		if (word == null || word.Length != 5)
		{
			Console.WriteLine("Enter a 5-letter word");
		}
		else
		{
			try
			{
				Console.WriteLine(game.StringGuessWord(word));
			}
			catch (WordGameException e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
	while (true);
}

void TestPrint()
{
	MyTree tree = new MyTree(new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 });
	tree.PrintTree(t => t.Left, t => t.Right, t => t.Value.ToString());
}

void TestCollision()
{
	var player = new HitBox([RegionFactory.CreateRectangle(20, 20, 50, 50)]);
	var walls = new List<HitBox> { new HitBox([RegionFactory.CreateRectangle(0, 0, 100, 100)]) };
	var safe = player.Collide(walls, 60, 0, 0.5f, true);
	player = player.Offset(safe.dX, safe.dY);
}


class MyTree
{
	public MyTree(List<int> nums)
	{
		if (nums.Count == 0)
		{
			throw new ArgumentException("nums must have at least one element");
		}
		Value = nums.First();
		if (nums.Count > 1)
		{
			Left = new MyTree(nums.Skip(1).Take(nums.Count / 2).ToList());
		}
		if (nums.Count > 2)
		{
			Right = new MyTree(nums.Skip(nums.Count / 2 + 1).ToList());
		}
	}
	public int Value { get; set; }
	public MyTree? Left { get; set; }
	public MyTree? Right { get; set; }
}
