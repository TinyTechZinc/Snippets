namespace Games.WordGuessingGame
{
	public class CharacterAndState
	{
		public char Character;
		public CharacterStates State;

		public override string ToString()
		{
			return $"[{Character}|{State}]";
		}
	}
}
