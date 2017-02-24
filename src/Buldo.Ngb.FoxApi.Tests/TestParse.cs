namespace Buldo.Ngb.FoxApi.Tests
{
    using System.IO;
    using System.Reflection;
    using AngleSharp.Parser.Html;
    using HtmlExamples;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestParse
    {
        private readonly Assembly _assembly;
        private readonly FoxResponseParser _parser = new FoxResponseParser();

        public TestParse()
        {
            _assembly = GetType().GetTypeInfo().Assembly;
        }


        [TestMethod]
        public void TestNewTaskParse()
        {
            var expected = new FoxEngineStatus
            {
                TeamName = "SG-1",
            };

            expected.MainCodes.Add("2А",  3);
            expected.MainCodes.Add("2В+", 1);
            expected.MainCodes.Add("2А+", 5);
            expected.MainCodes.Add("1В",  1);
            expected.MainCodes.Add("1А",  1);
            expected.MainCodes.Add("1А+", 1);

            RealParseText(ExamplesPatches.NewTask, expected);
        }

        [TestMethod]
        public void TestNewTaskWithBonusesParse()
        {
            var expected = new FoxEngineStatus
            {
                TeamName = "Рома",
            };

            expected.MainCodes.Add("А+", 5);
            expected.BonusCodes.Add("-2", 5);
            RealParseText(ExamplesPatches.NewTaskWithBonuses, expected);
        }

        [TestMethod]
        public void TestCodeNotExists()
        {
            var expected = new FoxEngineStatus
            {
                TeamName = "Рома",
                InputResult = InputResult.CodeNotExists
            };

            expected.MainCodes.Add("А+", 5);
            expected.BonusCodes.Add("-2", 5);

            RealParseText(ExamplesPatches.CodeNotExists, expected);
        }

        [TestMethod]
        public void TestCodeAccepted()
        {
            var expected = new FoxEngineStatus
            {
                TeamName = "Рома",
                InputResult = InputResult.CodeAccepted
            };

            expected.MainCodes.Add("А+", 5);
            expected.BonusCodes.Add("-2", 5);

            RealParseText(ExamplesPatches.CodeAccepted, expected);
        }

        private void RealParseText(string examplePath, FoxEngineStatus expected)
        {
            using (var stream = GetResourceStream(examplePath))
            {
                var parser = new HtmlParser();
                var document = parser.Parse(stream);
                var actual = _parser.Parse(document);

                Assert.AreEqual(expected.TeamName, actual.TeamName);
                Assert.AreEqual(expected.InputResult, actual.InputResult);
                Assert.AreEqual(expected.MessageText, actual.MessageText);
                CollectionAssert.AreEquivalent(expected.MainCodes,actual.MainCodes);
                CollectionAssert.AreEquivalent(expected.BonusCodes, actual.BonusCodes);
                CollectionAssert.AreEquivalent(expected.AcceptedCodes, expected.AcceptedCodes);
            }
        }

        private Stream GetResourceStream(string resource)
        {
            return _assembly.GetManifestResourceStream(resource);
        }
    }
}
