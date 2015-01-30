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

		public static string GuessEncoding(this FileInfo file, float probability = 0.5f)
		{
			using (var fs = file.OpenRead())
			{
				var cdet = new CharsetDetector();

				cdet.Feed(fs);
				cdet.DataEnd();

				return
					cdet.Charset != null && cdet.Confidence >= probability
						? cdet.Charset
						: null;
			}
		}

		public static void ParseWith(this FileInfo file, Action<string> checkAction, Encoding encoding)
		{
			Guard.CheckTrue(file.Exists, () => new FileNotFoundException(file.FullName));

			// TODO: file parser with sed, awk or grep.
			using (var reader = new StreamReader(file.OpenRead(), encoding))
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

					checkAction(currentLine);
				}
			}
		}
	}
}