using System.IO;
using System.Text;
using AcessConverter.Exceptions;
using Common.Contracts;

namespace AcessConverter
{
	public class Parser
	{
		private const string Cclientbankexchange = @"1CClientBankExchange";

		public bool IsInAccountSection { get; set; }

		public bool IsInDocumentSection { get; set; }

		public bool IsInHeader { get; set; }

		public bool IsOnec { get; set; }

		public bool Started { get; set; }

		public void Parse(string file)
		{
			Guard.CheckContainsText(file, "file");

			var fileInfo = new FileInfo(file);
			var guessEncoding = fileInfo.GuessEncoding();
			var encoding = Encoding.GetEncoding(guessEncoding);
			fileInfo.ParseWith(CurrentLineProcessor, encoding);
		}

		private bool CurrentValue { get; set; }

		private void ApplyRules(string value)
		{
			if (!IsOnec)
			{
				if (string.CompareOrdinal(value, Cclientbankexchange) == 0)
				{
					IsOnec = true;
				}
				else
				{
					throw new NotOnecException("This is not 1C Exchange format");
				}
			}
		}

		private void CurrentLineProcessor(string value)
		{
			Guard.CheckContainsText(value, "value");

			ApplyRules(value);
		}
	}
}