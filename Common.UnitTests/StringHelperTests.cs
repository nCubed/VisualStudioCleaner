using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeAsserter;

namespace VisualStudioCleaner.Common.UnitTests
{
    [TestClass]
    public class StringHelperTests
    {
        [TestMethod]
        public void StringHelper_IsStatic()
        {
            AssertClass.IsStatic( typeof( StringHelper ), ClassVisibility.Public );
        }

        [TestMethod]
        public void Split_Returns_ThreeElements()
        {
            const string str = "one;two;three";

            List<string> result = StringHelper.Split( str, ";" );

            Assert.AreEqual( 3, result.Count );
            CollectionAssert.Contains( result, "one" );
            CollectionAssert.Contains( result, "two" );
            CollectionAssert.Contains( result, "three" );
        }

        [TestMethod]
        public void Split_Returns_OrderedCollection()
        {
            const string str = "x;b;a;z;c;y";
            var expectedResult = new List<string> { "a", "b", "c", "x", "y", "z", };

            List<string> result = StringHelper.Split( str, ";" );

            CollectionAssert.AreEqual( expectedResult, result );
        }

        [TestMethod]
        public void Split_Ignores_EmptyString()
        {
            const string str = "a; ;b;c";

            AssertSplit_ABC_Result( str );
        }

        [TestMethod]
        public void Split_Ignores_EmptyItems()
        {
            const string str = ";;a;;;b;;;c;;;";

            AssertSplit_ABC_Result( str );
        }

        [TestMethod]
        public void CleanFileExtension_DoesNotModifyWellFormedExtension()
        {
            const string expecedExtension = ".txt";

            AsssertCleanFileExtension_Result( expecedExtension, expecedExtension );
        }

        [TestMethod]
        public void CleanFileExtension_Removes_WhiteSpace_LeftSide()
        {
            const string expectedExtension = ".txt";
            const string extWithWhiteSpace = " .txt";

            AsssertCleanFileExtension_Result( expectedExtension, extWithWhiteSpace );
        }

        [TestMethod]
        public void CleanFileExtension_Removes_WhiteSpace_RightSide()
        {
            const string expectedExtension = ".txt";
            const string extWithWhiteSpace = ".txt ";

            AsssertCleanFileExtension_Result( expectedExtension, extWithWhiteSpace );
        }

        [TestMethod]
        public void CleanFileExtension_Removes_WhiteSpace_BothSides()
        {
            const string expectedExtension = ".txt";
            const string extWithWhiteSpace = " .txt ";

            AsssertCleanFileExtension_Result( expectedExtension, extWithWhiteSpace );
        }

        [TestMethod]
        public void CleanFileExtension_Handles_MultiDotExtensions()
        {
            const string expectedExtension = ".file.txt";

            AsssertCleanFileExtension_Result( expectedExtension, expectedExtension );
        }

        [TestMethod]
        public void CleanFileExtension_AddsDot_WhenMissing()
        {
            const string expectedExtension = ".txt";
            const string extWithMissingDot = "txt";

            AsssertCleanFileExtension_Result( expectedExtension, extWithMissingDot );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void CleanFileExtensions_ThrowsArgumentNullException_WhenNullOrWhiteSpace()
        {
            StringHelper.CleanFileExtension( null );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void CleanFileExtensions_ThrowsArgumentNullException_WhenEmpty()
        {
            StringHelper.CleanFileExtension( string.Empty );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void CleanFileExtensions_ThrowsArgumentNullException_WhenWhiteSpace()
        {
            StringHelper.CleanFileExtension( " " );
        }

        private void AsssertCleanFileExtension_Result( string expectedExtension, string extensionToClean )
        {
            string result = StringHelper.CleanFileExtension( extensionToClean );

            Assert.AreEqual( expectedExtension, result );
        }

        private void AssertSplit_ABC_Result( string str )
        {
            var expectedResult = new List<string> { "a", "b", "c", };

            List<string> result = StringHelper.Split( str, ";" );

            CollectionAssert.AreEqual( expectedResult, result );
        }
    }
}
