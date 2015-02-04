using AcessConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Common.Functionals;

namespace Onec.Tests.FileTests
{
	[TestClass]
	public class FileExtensionsTests
	{
		[TestMethod]
		public void TestEnumFileOrFolder()
		{
			FileExtensions.ProcessFiles(Actions.DoNothing, @"e:\Development\GitHub\1c-bankstatement2ruby\");
		}
	}
}