using Games.Properties;
using System.Text.RegularExpressions;

// Pulled from another repository of mine: https://github.com/TimothyZink/Maui_First

namespace Games.WordGuessingGame
{
	/// <summary>
	/// Represents the game and its current state.
	/// </summary>
	public class WordGame
	{
		private readonly List<string> validWords;
		private readonly List<string> solutionWords;
		private string correctWord = default!;
		private readonly Random random;
		private readonly uint length;
		private readonly bool enforceValidWords = true;
		/// <summary>
		/// The default list of valid words.
		/// Retrieved from: https://gist.github.com/dracos/dd0668f281e685bad51479e5acaadb93
		/// </summary>
		public static List<string> DefaultValidWords
		{
			get => Resources._5LetterWords.Split(new string[] { "\n", "\r\n", "\r" }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
					.Where(s => s.Length == 5).ToList();
		}
		/// <summary>
		/// The default list of solution words.
		/// Retrieved from: https://gist.github.com/scholtes/94f3c0303ba6a7768b47583aff36654d
		/// </summary>
		public static List<string> DefaultSolutionWords
		{
			get => Resources.SolutionWords.Split(new string[] { "\n", "\r\n", "\r" }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
					.Where(s => s.Length == 5).ToList();
		}
		/// <summary>
		/// Initialize the game with the default word lists.
		/// </summary>
		public WordGame() : this(DefaultValidWords, DefaultSolutionWords)
		{ }
		/// <summary>
		/// Initialize the game.
		/// </summary>
		/// <param name="validWords">List of words to consider valid in addition to <see cref="solutionWords"/>.</param>
		/// <param name="solutionWords">The solution will be selected from this list.</param>
		/// <param name="length">The length of the words used, this limits guesses to this length.</param>
		/// <param name="enforceValidWords">If true, checks each word guessed against the list of valid words.</param>
		/// <exception cref="WordGameException"></exception>
		public WordGame(List<string> validWords, List<string> solutionWords, uint length = 5, bool enforceValidWords = true)
		{
			this.length = length;
			this.validWords = validWords.Union(solutionWords).ToList();
			this.solutionWords = solutionWords;
			this.enforceValidWords = enforceValidWords;
			if (validWords.Count == 0)
			{
				throw new WordGameException("No words in valid list.");
			}
			if (solutionWords.Count == 0)
			{
				throw new WordGameException("No words in solution list.");
			}
			random = new Random();
			ChooseWord();
		}
		/// <summary>
		/// Test if a word is the right length and is in the list of valid words.
		/// </summary>
		/// <param name="word"></param>
		/// <returns>True if the word is valid.</returns>
		public bool WordIsValid(string word)
		{
			return word.Length == length
				&& validWords.Contains(word);
		}
		/// <summary>
		/// Choose a new word, effectively resetting the game.
		/// Also returns the previous word.
		/// </summary>
		/// <returns></returns>
		public string ChooseWord()
		{
			var temp = correctWord;
			correctWord = solutionWords[random.Next(solutionWords.Count)].ToLower();
			return temp;
		}
		/// <summary>
		/// Wrapper around GuessWord that returns a string of the states.
		/// </summary>
		/// <param name="wordGuess"></param>
		/// <returns><list type="table">
		/// <item><term>r</term><description><see cref="CharacterStates.Correct"/></description></item>
		/// <item><term>p</term><description><see cref="CharacterStates.Partial"/></description></item>
		/// <item><term>i</term><description><see cref="CharacterStates.Incorrect"/></description></item>
		/// <item><term>' '</term><description><see cref="CharacterStates.None"/></description></item>
		/// </list></returns>
		public string StringGuessWord(string wordGuess)
		{
			var guess = GuessWord(wordGuess);
			return string.Join("", guess.Select(c => c.State switch
			{
				CharacterStates.Correct => 'r',
				CharacterStates.Partial => 'p',
				CharacterStates.Incorrect => 'i',
				_ => ' '
			}));
		}
		/// <summary>
		/// Guess a word and get a list of characters and their state (Correct, incorrect, partial).
		/// </summary>
		/// <param name="testWord"></param>
		/// <returns></returns>
		/// <exception cref="WordGameException"></exception>
		public List<CharacterAndState> GuessWord(string testWord)
		{
			testWord = testWord.ToLower();
			if (testWord.Length != length || correctWord.Length != length)
			{
				throw new WordGameException("Invalid word length(s).");
			}
			if (testWord != Regex.Replace(testWord, "[^a-zA-Z0-9]", ""))
			{
				throw new WordGameException("Invalid characters in test word.");
			}
			if (enforceValidWords && !validWords.Contains(testWord))
			{
				throw new WordGameException("Invalid word (not a word).");
			}
			// Get a list of the chars (this will keep track of characters already used
			var charList = correctWord.ToList();
			// Convert the test word to a list of characters with state
			var testCharList = testWord.Select(c => new CharacterAndState { Character = c, State = CharacterStates.None }).ToList();
			// First mark characters in the correct position
			for (int i = 0; i < length; i++)
			{
				if (testWord[i] == correctWord[i])
				{
					testCharList[i].State = CharacterStates.Correct;
					charList.Remove(testWord[i]);
				}
			}
			// Now mark contains but not correct position (and also mark not included)
			for (int i = 0; i < length; i++)
			{
				// Wrong position
				if (testCharList[i].State == CharacterStates.None)
				{
					if (charList.Contains(testWord[i]))
					{
						testCharList[i].State = CharacterStates.Partial;
						charList.Remove(testWord[i]);
					}
					// Only other option is the character is not in the word.
					else
					{
						testCharList[i].State = CharacterStates.Incorrect;
					}
				}
			}
			return testCharList;
		}
	}
}
