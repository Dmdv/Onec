using System;
using System.IO;
using System.Text;
using Common.Annotations;
using Net.Common.Contracts;
using Ude;

namespace AcessConverter
{
	public static class FileExtensions
	{
		public static string GuessEncoding(
			[NotNull] this FileInfo file,
			float probability = 0.5f)
		{
			Guard.CheckNotNull(file, "file");

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

		public static void ParseWith(
			[NotNull] this FileInfo file,
			[NotNull] Action<TextReader> processor,
			[NotNull] Encoding encoding)
		{
			Guard.CheckNotNull(file, "file");
			Guard.CheckNotNull(processor, "processor");
			Guard.CheckNotNull(encoding, "encoding");
			Guard.CheckTrue(file.Exists, () => new FileNotFoundException(file.FullName));

			// TODO: file parser with sed, awk or grep
			using (var reader = new StreamReader(file.OpenRead(), encoding))
			{
				processor(reader);
			}
		}

		// TODO: create async functions.

		public static void ProcessFiles(
			[NotNull] Action<string> processor,
			[NotNull] string fileOrFolder,
			[NotNull] string searchPattern = "*",
			SearchOption options = SearchOption.AllDirectories)
		{
			Guard.CheckNotNull(processor, "processor");
			Guard.CheckContainsText(fileOrFolder, "fileOrFolder");
			Guard.CheckNotNull(searchPattern, "searchPattern");

			if (File.Exists(fileOrFolder))
			{
				processor(fileOrFolder);
				return;
			}

			var info = new DirectoryInfo(fileOrFolder);

			foreach (var fileInfo in info.EnumerateFiles(searchPattern, options))
			{
				processor(fileInfo.FullName);
			}
		}
	}
}