using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AcessConverter.Exceptions;
using Common.Annotations;
using Net.Common.Contracts;
using Net.Common.Extensions;
using Net.Common.StreamExtensions;

namespace AcessConverter
{
	public class Parser
	{
		public bool AccountSectionCompleted { get; set; }

		public Dictionary<string, string> Dictionary { get; set; }

		public bool DocumentSectionCompleted { get; set; }

		// public bool IsAccountSection { get; set; }

		// public bool IsDocumentSection { get; set; }

		public bool IsHeader { get; set; }

		public bool IsOnec { get; set; }

		public bool Started { get; set; }

		public void Parse(string file)
		{
			Guard.CheckContainsText(file, "file");

			Dictionary = new Dictionary<string, string>();

			var fileInfo = new FileInfo(file);
			var guessEncoding = fileInfo.GuessEncoding();
			var encoding = Encoding.GetEncoding(guessEncoding);
			fileInfo.ParseWith(OnecProcessor, encoding);
		}

		private void OnecProcessor(TextReader stream)
		{
			Guard.CheckNotNull(stream, "stream");

			var pair = ExtractPair(stream.ReadLine());
			var key = pair.Item1;

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

			stream.ProcessWhileRead(ProcessBody);

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

		// TODO: Непонятно, зачем это. Можно просто все зачитать в хэш и проверять в хэше.
		// TODO: Оставляю этот момент на рефакторинг.
		// TODO: Если надо проверять в момент парсинга, то исп. HashSet.
		private void ProcessAccountSection(TextReader stream)
		{
			stream.ProcessWhileRead(ProcessCurrentString, Rules.IsAccountSectionComplete);
		}

		private void ProcessBody(TextReader stream, string currentString)
		{
			var pair = ExtractPair(currentString);
			var key = pair.Item1;

			Started = true;

			if (!AccountSectionCompleted && Rules.IsAccountSection(key))
			{
				ProcessAccountSection(stream);

				AccountSectionCompleted = true;
			}

			if (!DocumentSectionCompleted && Rules.IsDocumentSection(key))
			{
				ProcessDocumentSection(stream);

				DocumentSectionCompleted = true;
			}

			ProcessCurrentString(stream, currentString);
		}

		private void ProcessCurrentString(TextReader textReader, string currentString)
		{
			var pair = ExtractPair(currentString);
			var key = pair.Item1;
			var value = pair.Item2;
			Dictionary[key] = value;
		}

		private void ProcessDocumentSection(TextReader stream)
		{
			stream.ProcessWhileRead(ProcessCurrentString, Rules.IsDocumentSectionComplete);
		}

		[NotNull]
		internal static Tuple<string, string> ExtractPair(string currentString)
		{
			var strings = currentString.Split('='.YieldArray());

			var key = strings[0];
			var value = strings.Length == 2 ? strings[1] : string.Empty;

			return new Tuple<string, string>(key, value);
		}
	}
}