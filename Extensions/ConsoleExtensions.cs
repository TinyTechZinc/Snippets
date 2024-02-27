namespace Extensions
{
	/// <summary>
	/// A collection of methods for printing things to the console.
	/// </summary>
	public static class ConsoleExtensions
	{
		/// <summary>
		/// Print a tree data structure to the console.
		/// </summary>
		/// <typeparam name="T">A tree node</typeparam>
		/// <param name="node">The starting node</param>
		/// <param name="getLeft">Returns the left child node</param>
		/// <param name="getRight">Returns the right child node</param>
		/// <param name="toString">Use for printing node to screen, uses <see cref="object.ToString"/> if null</param>
		public static void PrintTree<T>(this T node, Func<T, T?> getLeft, Func<T, T?> getRight, Func<T, string>? toString = null)
		{
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
				Console.WriteLine($"{prefix}{(goingRight != null ? goingRight.Value ? "┌── " : "└── " : "")}{(toString == null ? node.ToString() : toString(node))}");
				Recurse(getLeft(node), false, left);
			}

			Recurse(node, null);
		}
	}
}
