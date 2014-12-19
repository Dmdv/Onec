using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Onec.Tests.FormatTests
{
	[TestClass]
	[DeploymentItem(@"ParserTests\1c_format.json")]
	public class OnecFormatTests
	{
		private const string JsonFile = "1c_format.json";

		[TestMethod]
		public void AssumeFormatContainsSomeFields()
		{
			var text = File.ReadAllText(JsonFile);

			dynamic jObject = JObject.Parse(text);

			Assert.IsNotNull(jObject["Документ"]);
			Assert.IsNotNull(jObject.Документ);
			Assert.IsNotNull(jObject.Документ.Values);
			Assert.IsTrue(jObject.Документ.Values[0] == "Платежное поручение");
		}
	}
}