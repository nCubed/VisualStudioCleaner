using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeAsserter;
using VisualStudioCleaner.Common.Domain.Finders;
using VisualStudioCleaner.UnitTestTools;

namespace VisualStudioCleaner.Finders.UnitTests
{
    [TestClass]
    public class DirectoryFinderTests
    {
        private static string _contextDirectory;
        private static List<string> _allDirectories;

        private DirectoryFinder _directoryFinder;

        [ClassInitialize]
        public static void ClassInit( TestContext context )
        {
            _contextDirectory = DirectoryUtils.CreateContextDirectory( context, "_DirectoryFinderTests" );

            _allDirectories = DirectoryUtils.CreateDirectoryStructure( _contextDirectory );

            FileUtils.CreateReadOnlyFiles( _allDirectories, FileExt.Dir );
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            DirectoryUtils.CleanupContextDirectory( _contextDirectory );
        }

        [TestInitialize]
        public void TestInit()
        {
            _directoryFinder = new DirectoryFinder( _contextDirectory );
        }

        [TestMethod]
        public void DirectoryFinder_Implements_IDirectoryFinder()
        {
            AssertClass<DirectoryFinder>.ImplementsInterface<IDirectoryFinder>();
            AssertClass<DirectoryFinder>.ImplementsInterface<IFinder>();
        }

        [TestMethod]
        public void DirectoryFinder_IsInternal()
        {
            AssertClass<DirectoryFinder>.IsInternal();
        }

        [TestMethod]
        public void DirectoryFinder_IsSealed()
        {
            AssertClass<DirectoryFinder>.IsSealed();
        }

        [TestMethod]
        public void Find_GeneralWildCard_ReturnsAllDirectories()
        {
            var searchPatterns = new[] { "*" };

            List<string> directories = _directoryFinder.Find( searchPatterns );

            Assert.AreEqual( _allDirectories.Count, directories.Count );

            CollectionAssert.AreEquivalent( _allDirectories, directories );
        }

        [TestMethod]
        public void Find_Matches_WholeDirectoryName_WhereOneExists()
        {
            var searchPatterns = new[] { "ab" };

            List<string> directories = _directoryFinder.Find( searchPatterns );

            Assert.AreEqual( 1, directories.Count );

            string dir1 = Path.Combine( _contextDirectory, "a", "ab" );
            CollectionAssert.Contains( directories, dir1 );
        }

        [TestMethod]
        public void Find_Matches_WholeDirectoryName_WhereFourExists()
        {
            var searchPatterns = new[] { "xxx" };

            List<string> directories = _directoryFinder.Find( searchPatterns );

            Assert.AreEqual( 4, directories.Count );

            bool endsWith = directories.TrueForAll( str => str.EndsWith( "\\xxx" ) );
            Assert.IsTrue( endsWith );
        }

        [TestMethod]
        public void Find_Matches_DirectoryNameStartsWith_WhereOneExists()
        {
            var searchPatterns = new[] { "abcde*" };

            List<string> directories = _directoryFinder.Find( searchPatterns );

            Assert.AreEqual( 1, directories.Count );

            string dir1 = Path.Combine( _contextDirectory, "a", "ab", "abc", "abcd", "abcde" );
            CollectionAssert.Contains( directories, dir1 );
        }

        [TestMethod]
        public void Find_Matches_DirectoryNameStartsWith_WhereTwoExists()
        {
            var searchPatterns = new[] { "abcd*" };

            List<string> directories = _directoryFinder.Find( searchPatterns );

            Assert.AreEqual( 2, directories.Count );

            string dir1 = Path.Combine( _contextDirectory, "a", "ab", "abc", "abcd" );
            CollectionAssert.Contains( directories, dir1 );

            string dir2 = Path.Combine( _contextDirectory, "a", "ab", "abc", "abcd", "abcde" );
            CollectionAssert.Contains( directories, dir2 );
        }

        [TestMethod]
        public void Find_Matches_DirectoryNameEndsWith_WhereOneExists()
        {
            var searchPatterns = new[] { "*bcde" };

            List<string> directories = _directoryFinder.Find( searchPatterns );

            Assert.AreEqual( 1, directories.Count );

            string dir1 = Path.Combine( _contextDirectory, "a", "ab", "abc", "abcd", "abcde" );
            CollectionAssert.Contains( directories, dir1 );
        }

        [TestMethod]
        public void Find_Matches_DirectoryNameEndsWith_WhereFourExists()
        {
            var searchPatterns = new[] { "*xx" };

            List<string> directories = _directoryFinder.Find( searchPatterns );

            Assert.AreEqual( 4, directories.Count );

            bool endsWith = directories.TrueForAll( str => str.EndsWith( "\\xxx" ) );
            Assert.IsTrue( endsWith );
        }

        [TestMethod]
        public void Find_Matches_MultiplePatterns_WholeDirectoryNames_WhereTwoExists()
        {
            var searchPatterns = new[] { "a", "abcde" };

            List<string> directories = _directoryFinder.Find( searchPatterns );

            Assert.AreEqual( 2, directories.Count );

            string dir1 = Path.Combine( _contextDirectory, "a" );
            CollectionAssert.Contains( directories, dir1 );

            string dir2 = Path.Combine( _contextDirectory, "a", "ab", "abc", "abcd", "abcde" );
            CollectionAssert.Contains( directories, dir2 );
        }

        [TestMethod]
        public void Find_Matches_MultipleMixedPatterns_OneExists()
        {
            var searchPatterns = new[] { "abcde*", "*bcde" };

            List<string> directories = _directoryFinder.Find( searchPatterns );

            Assert.AreEqual( 1, directories.Count );

            string dir1 = Path.Combine( _contextDirectory, "a", "ab", "abc", "abcd", "abcde" );
            CollectionAssert.Contains( directories, dir1 );
        }

        [TestMethod]
        public void Find_Matches_MultipleMixedPatterns_WhereThreeExists()
        {
            var searchPatterns = new[] { "abcd*", "a?c" };

            List<string> directories = _directoryFinder.Find( searchPatterns );

            Assert.AreEqual( 3, directories.Count );

            string dir1 = Path.Combine( _contextDirectory, "a", "ab", "abc" );
            CollectionAssert.Contains( directories, dir1 );

            string dir2 = Path.Combine( _contextDirectory, "a", "ab", "abc", "abcd" );
            CollectionAssert.Contains( directories, dir2 );

            string dir3 = Path.Combine( _contextDirectory, "a", "ab", "abc", "abcd", "abcde" );
            CollectionAssert.Contains( directories, dir3 );
        }
    }
}
