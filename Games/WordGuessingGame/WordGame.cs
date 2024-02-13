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
		private string correctWord = default!;
		private readonly Random random;
		private readonly uint length;
		/// <summary>
		/// Initialize the game with a list of valid words.
		/// </summary>
		/// <param name="validWords">List of valid words.</param>
		/// <param name="filterByLength">False, assumes that all the valid words are the correct length.</param>
		/// <param name="length">The length of the words used, this limits guesses to this length.</param>
		/// <exception cref="WordGameException"></exception>
		public WordGame(List<string> validWords, bool filterByLength = false, uint length = 5)
		{
			this.length = length;
			if (filterByLength)
			{
				this.validWords = validWords.Where(w => w.Length == length).ToList();
			}
			else
			{
				this.validWords = validWords;
			}
			if (validWords.Count == 0)
			{
				throw new WordGameException("No valid words in list.");
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
			correctWord = validWords[random.Next(validWords.Count)].ToLower();
			return temp;
		}
		/// <summary>
		/// Wrapper around GuessWord that returns a string of the states of the characters.
		/// </summary>
		/// <param name="wordGuess"></param>
		/// <param name="ignoreInvalid"></param>
		/// <returns><list type="table">
		/// <item><term>r</term><description><see cref="CharacterStates.Correct"/></description></item>
		/// <item><term>p</term><description><see cref="CharacterStates.Partial"/></description></item>
		/// <item><term>i</term><description><see cref="CharacterStates.Incorrect"/></description></item>
		/// <item><term>' '</term><description><see cref="CharacterStates.None"/></description></item>
		/// </list></returns>
		public string StringGuessWord(string wordGuess, bool ignoreInvalid = false)
		{
			var guess = GuessWord(wordGuess, ignoreInvalid);
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
		/// <param name="ignoreInvalid">If true, an error will be thrown if the word is invalid.</param>
		/// <returns></returns>
		/// <exception cref="WordGameException"></exception>
		public List<CharacterAndState> GuessWord(string testWord, bool ignoreInvalid = false)
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
			if (!ignoreInvalid && !validWords.Contains(testWord))
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
