﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExportToTxt
{
	public class Export
	{
		static void Main(string[] args)
		{
			var extension = new string[] { "*.cs", "*.sql", "*.config", "*.html", "*.cshtml", "*.css", "*.js", "*.aspx", "*.xml", };

			CheckBox cb = new CheckBox("Select at least one of the following options", true, true, extension);
			var cbResult = cb.Select();

			Console.Clear();

			var rootPath = string.Empty;
			var outputPath = string.Empty;

			do
			{
				Console.WriteLine("Write the directory which the application will read:");
				rootPath = Console.ReadLine();

				if (!Directory.Exists(rootPath))
					Console.WriteLine("This directory doesn't exist.");

			} while (!Directory.Exists(rootPath));

			Console.Write(Environment.NewLine);

			do
			{
				Console.WriteLine("Write the directory which the application will put result:");
				outputPath = Console.ReadLine();

				if (!Directory.Exists(outputPath))
					Console.WriteLine("This directory doesn't exist.");

			} while (!Directory.Exists(outputPath));

			var fullResult = new StringBuilder();
			var header = "********************************************" + Environment.NewLine;

			Console.WriteLine($"Export archives from {rootPath}" + Environment.NewLine);

			foreach (var item in cbResult.Select(c => c.Option))
			{
				var result = new List<string>();
				var singleStr = string.Empty;

				Console.WriteLine($"Start to reading {item} files " + Environment.NewLine);

				var files = Directory.GetFiles(rootPath, item, SearchOption.AllDirectories);
				var totalCount = files.Count();

				if (totalCount == 0)
				{
					Console.WriteLine("Extension not found");
					Console.WriteLine(Environment.NewLine + "--------------------------------------------------------------------------------" + Environment.NewLine);
					continue;
				}

				Console.WriteLine($"{totalCount} files was found." + Environment.NewLine);
				Console.WriteLine($"Reading and sorting out {totalCount} files." + Environment.NewLine);

				for (int i = 1; i < files.Length; i++)
				{
					var name = Path.GetFileName(files[i-1]);
					var contents = File.ReadAllText(files[i-1]);
					var line = Environment.NewLine + header + "Filename: " + name + Environment.NewLine + header + contents;

					result.Add(line);
					DrawTextProgressBar(i, totalCount);
				}

				Console.WriteLine("Done." + Environment.NewLine);
				Console.WriteLine("Start to saving" + Environment.NewLine);

				for (int i = 1; i < files.Length; i++)
				{
					singleStr += string.Join(Environment.NewLine, result[i - 1]);
					DrawTextProgressBar(i, totalCount);
				}

				Console.WriteLine("Done." + Environment.NewLine);

				fullResult.Append(singleStr);

				Console.WriteLine("--------------------------------------------------------------------------------" + Environment.NewLine);
			}

			var twoLastWords = Regex.Match(outputPath, @"(.{2})\s*$");
			var output = string.Empty;

			if (string.Equals(twoLastWords.Value, "s\\"))
				output = outputPath + "output.txt";
			else
				output = outputPath + @"\output.txt";

			Console.WriteLine($"Saving output result on {output}");

			File.WriteAllText(output, fullResult.ToString(), Encoding.UTF8);

			Console.WriteLine(Environment.NewLine + "Done.");
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
			var position = 0;
			for (int i = 0; i < oneChunk * progress; i++)
			{
				Console.BackgroundColor = ConsoleColor.Gray;
				Console.CursorLeft = position++;
				Console.Write(" ");
			}

			//draw unfilled part
			for (int i = 0; i <= 31; i++)
			{
				Console.BackgroundColor = ConsoleColor.Black;
				Console.CursorLeft = position++;
				Console.Write(" ");
			}

			//draw totals
			Console.CursorLeft = 35;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.Write($"{progress} of {total}    ");
		}
	}
}
