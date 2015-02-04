using System;
using AcessConverter;
using Net.Common.Extensions;

namespace ConsoleConverter
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length > 0)
			{
				var fileOrFolderName = args[0];
				Console.WriteLine("Processing file(s) from {0}...".FormatString(fileOrFolderName));

				var parser = new Parser();
				try
				{
					FileExtensions.ProcessFiles(parser.Parse, fileOrFolderName);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Exception: {0}".FormatString(ex.Message));
					return;
				}

				Console.WriteLine("Done.");

				Console.WriteLine("Validating...");
				var validator = new Validator(parser.Dictionary);
				validator.Validate();
				Console.Write("Done.");
				Console.WriteLine();

				Console.WriteLine("Write MS Access...");
				var writer = new MdbWriter();
				writer.Write(parser.Dictionary);
				Console.Write("Done.");
				Console.WriteLine();
			}
			else
			{
				Console.WriteLine("Enter file name or folder to create MS Access mbd base...");
				Console.WriteLine("Press Enter to exit...");
				Console.ReadLine();
			}
		}
	}
}