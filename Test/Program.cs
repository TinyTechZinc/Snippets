using Games.WordGuessingGame;

WordGame game = new WordGame(new List<string> { "apple", "thing", "these", "dates", "tests" });

Console.WriteLine(game.StringGuessWord("apple"));

Console.WriteLine(game.StringGuessWord("thing"));

Console.WriteLine(game.StringGuessWord("these"));

Console.WriteLine(game.ChooseWord());

