using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcessConverter
{
	public static class Rules
	{
		private const string Cclientbankexchange = @"1CClientBankExchange";

		public static bool IsClientBankExchange(string currentString)
		{
			return string.CompareOrdinal(currentString, Cclientbankexchange) == 0;
		}

		public static bool IsAccountSection(string currentString)
		{
			return false;

		}

		public static bool IsDocumentSection(string key)
		{
			return false;
		}
	}
}
