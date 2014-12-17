using System;
using System.Runtime.Serialization;

namespace AcessConverter.Exceptions
{
	[Serializable]
	public class NotOnecException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public NotOnecException()
		{
		}

		public NotOnecException(string message)
			: base(message)
		{
		}

		public NotOnecException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected NotOnecException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}