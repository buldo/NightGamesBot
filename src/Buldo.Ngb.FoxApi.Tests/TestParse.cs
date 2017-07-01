namespace Buldo.Ngb.FoxApi.Tests
{
    using System;
    using System.Collections.Generic;
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
            var expextedMainCodes = new Dictionary<string, int>
            {
                {"2А", 3},
                {"2В+", 1},
                {"2А+", 5},
                {"1В", 1},
                {"1А", 1},
                {"1А+", 1},
            };


            var expected = new FoxEngineStatus("SG-1",
                                               true,
                                               InputResult.None,
                                               string.Empty,
                                               expextedMainCodes,
                                               new Dictionary<string, int>(),
                                               new List<AcceptedCode>());



            RealParseText(ExamplesPatches.NewTask, expected);
        }

        [TestMethod]
        public void TestNewTaskWithBonusesParse()
        {
            var expected = new FoxEngineStatus("Рома",
                                               true,
                                               InputResult.None,
                                               string.Empty,
                                               new Dictionary<string, int> {{"А+", 5}},
                                               new Dictionary<string, int> {{"-2", 5}},
                                               new List<AcceptedCode>());
            RealParseText(ExamplesPatches.NewTaskWithBonuses, expected);
        }

        [TestMethod]
        public void TestCodeNotExists()
        {
            var expected = new FoxEngineStatus("Рома",
                                               true,
                                               InputResult.CodeNotExists,
                                               string.Empty,
                                               new Dictionary<string, int> {{"А+", 5}},
                                               new Dictionary<string, int> {{"-2", 5}},
                                               new List<AcceptedCode>());

            RealParseText(ExamplesPatches.CodeNotExists, expected);
        }

        [TestMethod]
        public void TestCodeAccepted()
        {
            var expected = new FoxEngineStatus("Рома",
                                               true,
                                               InputResult.CodeAccepted,
                                               string.Empty,
                                               new Dictionary<string, int> {{"А+", 5}},
                                               new Dictionary<string, int> {{"-2", 5}},
                                               new List<AcceptedCode>());

            RealParseText(ExamplesPatches.CodeAccepted, expected);
        }

        [TestMethod]
        public void TestCodeAcceptedWithComment()
        {
            var expected = new FoxEngineStatus("Рома",
                                               true,
                                               InputResult.CodeAccepted,
                                               "Текст Комментария",
                                               new Dictionary<string, int> {{"А+", 5}},
                                               new Dictionary<string, int> {{"-2", 5}},
                                               new List<AcceptedCode>());

            RealParseText(ExamplesPatches.CodeAcceptedWithComment, expected);
        }

        [TestMethod]
        public void TestCodeAlreadyAccepted()
        {
            var expected = new FoxEngineStatus("Рома",
                                               true,
                                               InputResult.CodeAlreadyAccepted,
                                               string.Empty,
                                               new Dictionary<string, int> {{"А+", 5}},
                                               new Dictionary<string, int> {{"-2", 5}},
                                               new List<AcceptedCode>());

            RealParseText(ExamplesPatches.CodeAlreadyAccepted, expected);
        }

        [TestMethod]
        public void TestWrongSpoiler()
        {
            var expected = new FoxEngineStatus("SG-1",
                                               true,
                                               InputResult.WrongSpoiler,
                                               string.Empty,
                                               new Dictionary<string, int> {{"О", 1}},
                                               new Dictionary<string, int>(),
                                               new List<AcceptedCode>());

            RealParseText(ExamplesPatches.WrongSpoiler, expected);
        }

        [TestMethod]
        public void TestGoodSpoiler()
        {
            var expected = new FoxEngineStatus("SG-1",
                                               true,
                                               InputResult.SpoilerOpened,
                                               string.Empty,
                                               new Dictionary<string, int>
                                               {
                                                   {"A+", 2},
                                                   {"A", 5},
                                                   {"B+", 2}
                                               },
                                               new Dictionary<string, int>(),
                                               new List<AcceptedCode>());

            RealParseText(ExamplesPatches.GoodSpoiler, expected);
        }

        private void RealParseText(string examplePath, FoxEngineStatus expected)
        {
            using (var stream = GetResourceStream(examplePath))
            {
                var parser = new HtmlParser();
                var document = parser.Parse(stream);
                var actual = _parser.Parse(document);

                Assert.AreEqual(expected, actual);
            }
        }

        private Stream GetResourceStream(string resource)
        {
            return _assembly.GetManifestResourceStream(resource);
        }
    }
}
