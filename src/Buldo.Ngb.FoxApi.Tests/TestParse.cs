namespace Buldo.Ngb.FoxApi.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using AngleSharp.Dom.Html;
    using AngleSharp.Parser.Html;
    using HtmlExamples;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
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

            expected.CodesOnLocation.Add("2À",  3);
            expected.CodesOnLocation.Add("2Â+", 1);
            expected.CodesOnLocation.Add("2À+", 5);
            expected.CodesOnLocation.Add("1Â",  1);
            expected.CodesOnLocation.Add("1À",  1);
            expected.CodesOnLocation.Add("1À+", 1);

            RealParseText(ExamplesPatches.NewTask, expected);
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
                CollectionAssert.AreEquivalent(expected.CodesOnLocation,actual.CodesOnLocation);
                CollectionAssert.AreEquivalent(expected.EnteredCodes, expected.EnteredCodes);
            }
        }

        private Stream GetResourceStream(string resource)
        {
            return _assembly.GetManifestResourceStream(resource);
        }
    }
}
