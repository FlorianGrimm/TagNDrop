using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TagNDropBuisTests {
    [TestClass]
    public class TNDMetaDataExtractorTest {
        static string getPath() { return @"C:\data\TagNDrop\TagNDropBuisTests"; }
        static string getPathDocx() { return getPath() + @"\abc.docx"; }
        static string getPathDoc() { return getPath() + @"\def.doc"; }
        [TestMethod]
        public void ReadDocx() {
            NPOI.XWPF.UserModel.XWPFDocument doc;
            using (var fs = new System.IO.FileStream(getPathDocx(), System.IO.FileMode.Open)) {
                doc = new NPOI.XWPF.UserModel.XWPFDocument(fs);
            }
            var properties = doc.GetProperties();
            var coreProperties = properties.CoreProperties;
            var customProperties = properties.CustomProperties;
            var extendedProperties = properties.ExtendedProperties;
            var sb = new System.Text.StringBuilder();
            foreach (var p in doc.Paragraphs) {
                sb.AppendLine(p.Text);
            }
            Assert.IsTrue(sb.ToString() != "");
            //customProperties.AddProperty("greet", "hallole");
            //using (var fs = new System.IO.FileStream(getPath() + @"\abc2.docx", System.IO.FileMode.Create)) {
            //    doc.Write(fs);
            //}
        }

        [TestMethod]
        public void ReadDoc() {
            NPOI.HWPF.HWPFDocument doc;
            using (var fs = new System.IO.FileStream(getPathDoc(), System.IO.FileMode.Open)) {
                doc = new NPOI.HWPF.HWPFDocument(fs);
            }
            var docProperties = doc.GetDocProperties();
            var documentSummaryInformation = doc.DocumentSummaryInformation;
            var customProperties = documentSummaryInformation.CustomProperties;
            //customProperties.Add("greet", "hallole");
            var content = doc.Text.ToString();
            Assert.IsTrue(content != "");
            //using (var fs = new System.IO.FileStream(getPath() + @"\def2.doc", System.IO.FileMode.Create)) {
            //    doc.Write(fs);
            //}
        }
    }
}
