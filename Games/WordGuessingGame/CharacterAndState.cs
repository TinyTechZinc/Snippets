namespace Games.WordGuessingGame
{
	/// <summary>
	/// Represents the a character and its state in a guess.
	/// </summary>
	public class CharacterAndState
	{
		/// <summary>
		/// The character.
		/// </summary>
		public char Character;
		/// <summary>
		/// The state of the character.
		/// </summary>
		public CharacterStates State;
		/// <summary>
		/// Returns a string representation of the character and its state.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"[{Character}|{State}]";
		}
	}
}
