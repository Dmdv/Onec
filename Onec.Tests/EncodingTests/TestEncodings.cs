using System.IO;
using AcessConverter;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Onec.Tests.EncodingTests
{
	[TestClass]
	[DeploymentItem(@"ParserTests\KOI8R.txt")]
	[DeploymentItem(@"ParserTests\Not1c_format_demo.txt")]
	public class TestEncodings
	{
		private const string Koi8RFile = "KOI8R.txt";
		private const string NotOnecFile = "Not1c_format_demo.txt";

		[TestMethod]
		public void AssumeFileIs1251()
		{
			var fileInfo = new FileInfo(NotOnecFile);
			var guessEncoding = fileInfo.GuessEncoding();
			guessEncoding.Should().Be("windows-1251");
		}

		[TestMethod]
		public void AssumeFileIsKoi8R()
		{
			var fileInfo = new FileInfo(Koi8RFile);
			var guessEncoding = fileInfo.GuessEncoding();
			guessEncoding.Should().Contain("KOI8-R");
		}
	}
}