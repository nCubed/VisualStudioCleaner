using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VisualStudioCleaner.UnitTestTools.ToolsTests
{
    [TestClass]
    internal class FileExtTests
    {
        [TestMethod]
        public static void AssertAllExtensionsAreUnique()
        {
            const int expectedCount = 12;

            var extensions = new[]
            {
                FileExt.Dir, FileExt.File, 
                FileExt.Ext1, FileExt.Ext2, FileExt.Ext3, FileExt.Ext4, FileExt.Ext5, 
                FileExt.Ext6, FileExt.Ext7, FileExt.Ext8, FileExt.Ext9, FileExt.Ext10,
            };

            List<string> expectedExtensions = extensions.Distinct().ToList();

            Assert.AreEqual( expectedCount, expectedExtensions.Count );

            CollectionAssert.AreEquivalent( extensions, expectedExtensions );

            CollectionAssert.AreEquivalent( expectedExtensions, FileExt.AllExtensions.ToList() );
        }
    }
}
