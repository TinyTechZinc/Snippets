using Games.WordGuessingGame;
using Extensions;

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
