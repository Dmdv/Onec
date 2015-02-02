using System;
using System.IO;
using System.Text;
using Net.Common.Contracts;
using Ude;

namespace AcessConverter
{
	public static class FileExtensions
	{
		public static bool FindString(this FileInfo file, Predicate<string> predicate)
		{
			Guard.CheckNotNull(file, "file");
			Guard.CheckNotNull(predicate, "predicate");
			Guard.CheckTrue(file.Exists, () => new FileNotFoundException(file.FullName));

			// TODO: file parser with sed, awk or grep.
			using (var reader = new StreamReader(file.OpenRead()))
			{
				return ProcessStream(predicate, reader);
			}
		}

		public static string GuessEncoding(this FileInfo file, float probability = 0.5f)
		{
			using (var fs = file.OpenRead())
			{
				var charsetDetector = new CharsetDetector();

				charsetDetector.Feed(fs);
				charsetDetector.DataEnd();

				return
					charsetDetector.Charset != null && charsetDetector.Confidence >= probability
						? charsetDetector.Charset
						: null;
			}
		}

		public static void ParseWith(this FileInfo file, Action<TextReader, string> processor, Encoding encoding)
		{
			Guard.CheckTrue(file.Exists, () => new FileNotFoundException(file.FullName));

			// TODO: file parser with sed, awk or grep.
			using (var reader = new StreamReader(file.OpenRead(), encoding))
			{
				reader.ProcessWhileRead(processor);
			}
		}

		private static bool ProcessStream(Predicate<string> predicate, TextReader reader)
		{
			while (true)
			{
				var currentLine = reader.ReadLine();

				if (currentLine == null)
				{
					break;
				}

				if (string.IsNullOrWhiteSpace(currentLine))
				{
					continue;
				}

				if (predicate(currentLine))
				{
					return true;
				}
			}
			return false;
		}
	}
}