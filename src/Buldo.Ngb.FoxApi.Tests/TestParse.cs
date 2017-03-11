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

        [TestMethod]
        public void TestCodeAcceptedWithComment()
        {
            var expected = new FoxEngineStatus
            {
                TeamName = "Рома",
                InputResult = InputResult.CodeAccepted,
                Message = "Текст Комментария"
            };

            expected.MainCodes.Add("А+", 5);
            expected.BonusCodes.Add("-2", 5);

            RealParseText(ExamplesPatches.CodeAcceptedWithComment, expected);
        }

        [TestMethod]
        public void TestCodeAlreadyAccepted()
        {
            var expected = new FoxEngineStatus
            {
                TeamName = "Рома",
                InputResult = InputResult.CodeAlreadyAccepted
            };

            expected.MainCodes.Add("А+", 5);
            expected.BonusCodes.Add("-2", 5);

            RealParseText(ExamplesPatches.CodeAlreadyAccepted, expected);
        }

        [TestMethod]
        public void TestWrongSpoiler()
        {
            var expected = new FoxEngineStatus
            {
                TeamName = "SG-1",
                InputResult = InputResult.WrongSpoiler
            };

            expected.MainCodes.Add("О", 1);

            RealParseText(ExamplesPatches.WrongSpoiler, expected);
        }

        [TestMethod]
        public void TestGoodSpoiler()
        {
            var expected = new FoxEngineStatus
            {
                TeamName = "SG-1",
                InputResult = InputResult.SpoilerOpened
            };

            expected.MainCodes.Add("A+", 2);
            expected.MainCodes.Add("A", 5);
            expected.MainCodes.Add("B+", 2);

            RealParseText(ExamplesPatches.GoodSpoiler, expected);
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
                Assert.AreEqual(expected.Message, actual.Message);
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
