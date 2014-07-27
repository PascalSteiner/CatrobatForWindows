﻿using Catrobat.IDE.Core.Tests.Misc;
using Catrobat.IDE.Core.Tests.SampleData;
using Catrobat.IDE.Core.Xml.VersionConverter;
using Catrobat.IDE.Core.Xml.VersionConverter.Versions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Catrobat.IDE.Core.Tests.Tests.Data
{
    [TestClass]
    public class VersionConverter092To091
    {
        [ClassInitialize]
        public static void TestClassInitialize(TestContext testContext)
        {
            TestHelper.InitializeTests();
        }

        [TestMethod, TestCategory("GatedTests.Obsolete")]
        public void ObjectReferences()
        {
            TestSampleData("Converter/092_091/Test1");
        }

        private void TestSampleData(string path)
        {
            var actualDocument = SampleLoader.LoadSampleXDocument(path + "_Input");
            var expectedDocument = SampleLoader.LoadSampleXDocument(path + "_Output");


            var error = CatrobatVersionConverter.ConvertVersions(
                "0.92", "0.91", actualDocument);
            Assert.AreEqual(CatrobatVersionConverter.VersionConverterError.NoError, 
                error);

            XmlDocumentComparer.Compare(expectedDocument, actualDocument);
        }

    }
}