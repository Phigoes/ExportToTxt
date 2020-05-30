using System;
using System.Collections.Generic;
using System.Text;

namespace ExportToTxt
{
	public class TextProgressBar
	{
		public TextProgressBar(int progress, int total)
		{
			DrawTextProgressBar(progress, total);
		}

		private static void DrawTextProgressBar(int progress, int total)
		{
			//draw empty progress bar
			Console.CursorLeft = 0;
			Console.Write("[");
			Console.CursorLeft = 32;
			Console.Write("]");
			Console.CursorLeft = 1;
			double oneChunk = 30.0 / total;

			//draw filled part
			var position = 1;
			for (int i = 0; i < oneChunk * (progress + 1); i++)
			{
				Console.BackgroundColor = ConsoleColor.Gray;
				Console.CursorLeft = position++;
				Console.Write(" ");
			}

			////draw unfilled part
			//for (int i = 0; i <= 31; i++)
			//{
			//	Console.BackgroundColor = ConsoleColor.Black;
			//	Console.CursorLeft = position++;
			//	Console.Write(" ");
			//}

			//draw totals
			Console.CursorLeft = 35;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.Write($"{(progress + 1)} of {total}    ");
		}
	}
}
