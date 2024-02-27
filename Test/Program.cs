using Games.WordGuessingGame;

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

