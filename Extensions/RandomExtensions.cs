using System.Drawing;

namespace Extensions
{
	public static class RandomExtensions
	{
		public static Color RandomColor(this Random random)
		{
			return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
		}
		/// <summary>
		/// Return a random sequence of these elements using Durstenfeld's variant of the Fisher-Yates shuffle algorithm.
		/// </summary>
		/// <remarks>
		/// Reference: https://stackoverflow.com/a/1287572/294804
		/// By Jon Skeet
		/// </remarks>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="random"></param>
		/// <returns></returns>
		public static IEnumerable<T> Shuffle<T>(this Random random, IEnumerable<T> source)
		{
			T[] elements = source.ToArray();
			for (int i = elements.Length - 1; i >= 0; i--)
			{
				// Swap element "i" with a random earlier element it (or itself)
				int swapIndex = random.Next(i + 1);
				yield return elements[swapIndex];
				elements[swapIndex] = elements[i];
				// ... except we don't really need to swap it fully, as we can
				// return it immediately, and afterwards it's irrelevant.
			}
		}
		/// <summary>
		/// Returns an IEnumerable of unique integers from min to max (inclusive) in a random order.
		/// </summary>
		/// <remarks>
		/// Uses <see cref="Shuffle{T}(IEnumerable{T}, Random)"/>
		/// </remarks>
		/// <param name="min">Minimum value (inclusive)</param>
		/// <param name="max">Maximum value (inclusive)</param>
		/// <returns></returns>
		public static IEnumerable<int> PermuteIntegers(this Random random, int min, int max)
		{
			return random.Shuffle(Enumerable.Range(min, max - min + 1));
		}
	}
}
