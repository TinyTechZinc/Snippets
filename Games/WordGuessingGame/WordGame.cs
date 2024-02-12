using System.Text.RegularExpressions;

// Pulled from another repository of mine: https://github.com/TimothyZink/Maui_First

namespace Games.WordGuessingGame
{
	public class WordGame
	{
		private readonly List<string> validWords;
		private string correctWord = default!;
		private readonly Random random;
		private readonly uint length;
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
				throw new Exception("No valid words in list.");
			}
			random = new Random();
			ChooseWord();
		}

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
		public List<CharacterAndState> TestWord(string testWord, bool ignoreInvalid = false)
		{
			testWord = testWord.ToLower();
			if (testWord.Length != length || correctWord.Length != length)
			{
				throw new Exception("Invalid word length(s).");
			}
			if (testWord != Regex.Replace(testWord, "[^a-zA-Z0-9]", ""))
			{
				throw new Exception("Invalid characters in test word.");
			}
			if (!ignoreInvalid && !validWords.Contains(testWord))
			{
				throw new Exception("Invalid word (not a word).");
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
					testCharList[i].State = CharacterStates.Good;
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
						testCharList[i].State = CharacterStates.Bad;
					}
				}
			}
			return testCharList;
		}
	}
}
