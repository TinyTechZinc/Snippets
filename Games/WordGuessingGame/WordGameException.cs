namespace Games.WordGuessingGame
{
    /// <summary>
    /// Generic exception for the word game.
    /// </summary>
    public class WordGameException : Exception
    {
        /// <summary>
        /// Create a new exception with a message.
        /// </summary>
        /// <param name="message"></param>
        public WordGameException(string message) : base(message)
        {
        }
    }
}
