namespace Extensions
{
	/// <summary>
	/// A collection of methods for printing things to the console.
	/// </summary>
	public static class ConsoleExtensions
	{
		/// <summary>
		/// Prints a tree data structure.
		/// </summary>
		/// <typeparam name="T">A tree node</typeparam>
		/// <param name="node">The starting node</param>
		/// <param name="getLeft">Function returning the left child node</param>
		/// <param name="getRight">Function returning the right child node</param>
		/// <param name="toString">Used for converting node to a string, uses <see cref="object.ToString"/> if null</param>
		/// <param name="writeLine">Used for writing to stream, uses <see cref="Console.WriteLine(string)"/> if null</param>
		public static void PrintTree<T>(this T node, Func<T, T?> getLeft, Func<T, T?> getRight, Func<T, string>? toString = null, Action<string>? writeLine = null)
		{
			if (writeLine == null)
			{
				writeLine = Console.WriteLine;
			}
			void Recurse(T? node, bool? goingRight, string prefix = "")
			{
				if (node == null)
				{
					return;
				}
				string right = "";
				string left = "";
				if (goingRight == true)
				{
					right = $"{prefix}    ";
					left = $"{prefix}│   ";
				}
				else if (goingRight == false)
				{
					right = $"{prefix}|   ";
					left = $"{prefix}    ";
				}
				Recurse(getRight(node), true, right);
				writeLine($"{prefix}{(goingRight != null ? goingRight.Value ? "┌── " : "└── " : "")}{(toString == null ? node.ToString() : toString(node))}");
				Recurse(getLeft(node), false, left);
			}

			Recurse(node, null);
		}
	}
}
