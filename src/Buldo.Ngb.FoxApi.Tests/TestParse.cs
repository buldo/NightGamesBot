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

            expected.MainCodes.Add("2�",  3);
            expected.MainCodes.Add("2�+", 1);
            expected.MainCodes.Add("2�+", 5);
            expected.MainCodes.Add("1�",  1);
            expected.MainCodes.Add("1�",  1);
            expected.MainCodes.Add("1�+", 1);

            RealParseText(ExamplesPatches.NewTask, expected);
        }

        [TestMethod]
        public void TestNewTaskWithBonusesParse()
        {
            var expected = new FoxEngineStatus
            {
                TeamName = "����",
            };

            expected.MainCodes.Add("�+", 5);

            RealParseText(ExamplesPatches.NewTaskWithBonuses, expected);
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
                CollectionAssert.AreEquivalent(expected.EnteredCodes, expected.EnteredCodes);
            }
        }

        private Stream GetResourceStream(string resource)
        {
            return _assembly.GetManifestResourceStream(resource);
        }
    }
}
