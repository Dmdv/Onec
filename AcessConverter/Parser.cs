using System.IO;
using System.Text;
using AcessConverter.Exceptions;
using Net.Common.Contracts;

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

		private void ApplyRulesToCurrentLine(string value)
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

			// TODO check against json.
			// 1. Поля в секции должны быть в своей секции.
			// Посещая секцию, устанавливаем true для определенной секции и считываем все значения до окончания секции.
			// Полученный список значений записываем проверяем: а) Наличие б) проверяем значения по алгоритму п.2.
			// 2. Форматы должны соответствовать своим шаблонам.
			// "Format" : "чч:мм:сс", "дд.мм.гггг"
			// "Max" : максимальная длина поля.
			// 3. При наличии Children, все поля внутри секции/раздела должны быть в пределах "Children"
			// 4. При наличии Values, значение должно быть в пределах указанных значений.
			// 5. Если значение Required = true, и оно не найдено => исключение.
			// 6. На выходе HashTable ключей со значениями.
			// 7. Проверяем отдельные ключи по бизнес-правилам, выбираем отдельные ключи, проверяем.
			// 8. Проходим список всех ключей в json (это по сути Hashtable.)
			// 8. Создаем Access на основе.
		}

		private void CurrentLineProcessor(string value)
		{
			Guard.CheckContainsText(value, "value");

			ApplyRulesToCurrentLine(value);
		}
	}
}