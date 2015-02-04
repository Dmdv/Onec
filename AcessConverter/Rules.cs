using System.Collections.Generic;

namespace AcessConverter
{
	public static class Rules
	{
		private const string Cclientbankexchange = @"1CClientBankExchange";
		private const string Bik1 = @"ПлательщикБИК";
		private const string Bik2 = @"ПолучательБИК";

		public static void CorrectBikNumbers(Dictionary<string, string> table)
		{
			if (table.ContainsKey(Bik1))
			{
				
			}

			if (table.ContainsKey(Bik2))
			{

			}
		}

		public static bool IsAccountSection(string currentString)
		{
			return currentString.Contains("СекцияРасчСчет");
		}

		public static bool IsAccountSectionComplete(string currentString)
		{
			return currentString.Contains("КонецРасчСчет");
		}

		public static bool IsClientBankExchange(string currentString)
		{
			return string.CompareOrdinal(currentString, Cclientbankexchange) == 0;
		}

		public static bool IsDocumentSection(string currentString)
		{
			return currentString.Contains("СекцияДокумент");
		}

		public static bool IsDocumentSectionComplete(string currentString)
		{
			return currentString.Contains("КонецДокумента");
		}
	}
}