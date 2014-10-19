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

            Assert_ABC_Result( str );
        }

        [TestMethod]
        public void Split_Ignores_EmptyItems()
        {
            const string str = ";;a;;;b;;;c;;;";

            Assert_ABC_Result( str );
        }

        private void Assert_ABC_Result( string str )
        {
            var expectedResult = new List<string> { "a", "b", "c", };

            List<string> result = StringHelper.Split( str, ";" );

            CollectionAssert.AreEqual( expectedResult, result );
        }
    }
}
