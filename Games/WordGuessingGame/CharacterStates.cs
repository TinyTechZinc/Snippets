namespace Games.WordGuessingGame
{
	/// <summary>
	/// Possible states for a character in a word guess.
	/// </summary>
	public enum CharacterStates
	{
		/// <summary>
		/// Default state; this is not a valid state.
		/// </summary>
		None,
		/// <summary>
		/// The character is not in the word.
		/// </summary>
		Incorrect,
		/// <summary>
		/// The character is in the word, but not in the correct position.
		/// </summary>
		Partial,
		/// <summary>
		/// The character is in the word and in the correct position.
		/// </summary>
		Correct
	}
}
