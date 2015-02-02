using System;
using System.IO;

namespace AcessConverter
{
	public static class StreamExtensions
	{
		public static void Process(
			this TextReader reader,
			Predicate<string> predicateOfExit,
			Action<TextReader, string> processor)
		{
			string currentLine;

			while ((currentLine = reader.ReadLine()) != null)
			{
				if (predicateOfExit(currentLine))
				{
					break;
				}

				processor(reader, currentLine);
			}
		}

		public static void ProcessWhileRead(this TextReader reader, Action<TextReader, string> processor)
		{
			string currentLine;

			while ((currentLine = reader.ReadLine()) != null)
			{
				if (string.IsNullOrWhiteSpace(currentLine) || string.IsNullOrEmpty(currentLine))
				{
					continue;
				}

				processor(reader, currentLine);
			}
		}
	}
}