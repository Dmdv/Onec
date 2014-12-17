using System;
using System.Runtime.Serialization;

namespace AcessConverter.Exceptions
{
	[Serializable]
	public class NotValidOnecFileException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public NotValidOnecFileException()
		{
		}

		public NotValidOnecFileException(string message)
			: base(message)
		{
		}

		public NotValidOnecFileException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected NotValidOnecFileException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}