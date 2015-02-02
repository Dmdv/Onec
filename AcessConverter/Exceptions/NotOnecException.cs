using System;
using System.Runtime.Serialization;

namespace AcessConverter.Exceptions
{
	[Serializable]
	public class NotOnecException : Exception
	{
		public NotOnecException()
		{
		}

		public NotOnecException(string message)
			: base("This is not 1C Exchange format")
		{
		}

		public NotOnecException(string message, Exception inner)
			: base("This is not 1C Exchange format", inner)
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