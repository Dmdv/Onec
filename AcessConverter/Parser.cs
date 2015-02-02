using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AcessConverter.Exceptions;
using Net.Common.Contracts;
using Net.Common.Extensions;

namespace AcessConverter
{
	public class Parser
	{
		public bool IsAccountSection { get; set; }

		public bool IsDocumentSection { get; set; }

		public bool DocumentSectionCompleted { get; set; }

		public bool AccountSectionCompleted { get; set; }

		public bool IsHeader { get; set; }

		public bool IsOnec { get; set; }

		public bool Started { get; set; }

		public Dictionary<string,string> Dictionary { get; set; }

		public void Parse(string file)
		{
			Guard.CheckContainsText(file, "file");

			var fileInfo = new FileInfo(file);
			var guessEncoding = fileInfo.GuessEncoding();
			var encoding = Encoding.GetEncoding(guessEncoding);
			fileInfo.ParseWith(CurrentLineProcessor, encoding);
		}

		private static Tuple<string, string> ExtractPair(string currentString)
		{
			var strings = currentString.Split('='.YieldArray());

			var key = strings[0];
			var value = strings.Length == 3 ? strings[2] : string.Empty;

			return new Tuple<string, string>(key, value);
		}

		private void ReadAccountSection(TextReader stream, Dictionary<string, string> table)
		{
		}

		private void ApplyRulesToCurrentLine(TextReader stream, string currentString)
		{
			var pair = ExtractPair(currentString);
			var key = pair.Item1;
			var value = pair.Item2;

			if (!IsOnec)
			{
				if (Rules.IsClientBankExchange(key))
				{
					IsOnec = true;
				}
				else
				{
					throw new NotOnecException();
				}
			}

			Started = true;

			if (!AccountSectionCompleted && Rules.IsAccountSection(key))
			{
				IsAccountSection = true;
				// TODO: read all section;

				AccountSectionCompleted = true;
			}

			if (!DocumentSectionCompleted && Rules.IsDocumentSection(key))
			{
				IsDocumentSection = true;
				// TODO: read all section;

				DocumentSectionCompleted = true;
			}

			Dictionary[key] = value;
		}

		private void CurrentLineProcessor(TextReader stream, string value)
		{
			Guard.CheckContainsText(value, "value");

			ApplyRulesToCurrentLine(stream, value);

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
	}
}