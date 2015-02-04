using System.IO;
using AcessConverter;
using AcessConverter.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Common.Contracts;

namespace Onec.Tests.ParserTests
{
	[TestClass]
	[DeploymentItem(@"ParserTests\1c_format_demo.txt")]
	[DeploymentItem(@"ParserTests\Not1c_format_demo.txt")]
	public class TestParsing
	{
		private const string OnecFile = "1c_format_demo.txt";
		private const string NotOnecFile = "Not1c_format_demo.txt";
		

		[TestMethod]
		[ExpectedException(typeof (ContractViolationException))]
		public void AssumeContractViolationExceptionThrown()
		{
			_parser.Parse(string.Empty);
		}

		[TestMethod]
		public void TestExtractPair()
		{
			var pair = Parser.ExtractPair("Получатель=Бухгалтерский учет, редакция 4.2");
			pair.Item1.Should().Be("Получатель");
			pair.Item2.Should().Be("Бухгалтерский учет, редакция 4.2");


			pair = Parser.ExtractPair("ОКАТО=");
			pair.Item1.Should().Be("ОКАТО");
			pair.Item2.Should().BeEmpty();
		}

		[TestMethod]
		public void AssumeFileIsOnec()
		{
			_parser.Parse(OnecFile);
		}

		[TestMethod]
		[ExpectedException(typeof (FileNotFoundException))]
		public void AssumeFileNotFoundThrown()
		{
			_parser.Parse("CCC");
		}

		[TestMethod]
		[ExpectedException(typeof (NotOnecException))]
		public void AssumeFileNotOnec()
		{
			_parser.Parse(NotOnecFile);
		}

		[TestInitialize]
		public void InitTest()
		{
			_parser = new Parser();
		}

		private Parser _parser;
	}
}