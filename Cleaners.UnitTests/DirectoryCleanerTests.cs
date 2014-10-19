using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TypeAsserter;
using VisualStudioCleaner.Cleaners.IOCleaners;
using VisualStudioCleaner.Common.Domain.Cleaners;
using VisualStudioCleaner.Common.Domain.Finders;
using VisualStudioCleaner.UnitTestTools;

namespace VisualStudioCleaner.Cleaners.UnitTests
{
    [TestClass]
    public class DirectoryCleanerTests
    {
        private static string _contextDirectory;

        private Mock<IDirectoryFinder> _mockFinder;

        private DirectoryCleaner _directoryCleaner;

        [ClassInitialize]
        public static void ClassInit( TestContext context )
        {
            _contextDirectory = DirectoryUtils.CreateContextDirectory( context, "_DirectoryCleanerTests" );
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            DirectoryUtils.CleanupContextDirectory( _contextDirectory );
        }

        [TestInitialize]
        public void TestInit()
        {
            _mockFinder = new Mock<IDirectoryFinder>( MockBehavior.Strict );

            _directoryCleaner = new DirectoryCleaner( _mockFinder.Object );
        }

        [TestMethod]
        public void DirectoryCleaner_Implements_IDirectoryCleaner()
        {
            AssertClass<DirectoryCleaner>.ImplementsInterface<IDirectoryCleaner>();
        }

        [TestMethod]
        public void DirectoryCleaner_IsInternal()
        {
            AssertClass<DirectoryCleaner>.IsInternal();
        }

        [TestMethod]
        public void DirectoryCleaner_IsSealed()
        {
            AssertClass<DirectoryCleaner>.IsSealed();
        }

        [TestMethod]
        public void Clean_DeletesAllDirectories()
        {
            string rootDirectory;
            List<string> createdDirectories = CreateDirectoryStructure( out rootDirectory );

            var searchPattern = new[] { "*" };
            var mockFinderResult = new List<string>( createdDirectories );
            _mockFinder.Setup( x => x.Find( searchPattern ) ).Returns( mockFinderResult );

            AssertClean( searchPattern, mockFinderResult );
        }

        [TestMethod]
        public void Clean_DeletesSingleDirectory()
        {
            string rootDirectory;
            List<string> createdDirectories = CreateDirectoryStructure( out rootDirectory );

            string dir1 = Path.Combine( rootDirectory, "a", "ab", "abc", "abcd", "abcde" );
            CollectionAssert.Contains( createdDirectories, dir1 );

            var searchPattern = new[] { "abcde" };
            var mockFinderResult = new List<string> { dir1 };
            _mockFinder.Setup( x => x.Find( searchPattern ) ).Returns( mockFinderResult );

            AssertClean( searchPattern, mockFinderResult );
        }

        [TestMethod]
        public void Clean_DeletesSomeDirectories()
        {
            string rootDirectory;
            List<string> createdDirectories = CreateDirectoryStructure( out rootDirectory );

            var searchPattern = new[] { "xxx" };
            List<string> mockFinderResult = createdDirectories.Where( x => x.EndsWith( "\\xxx" ) ).ToList();
            _mockFinder.Setup( x => x.Find( searchPattern ) ).Returns( mockFinderResult );

            Assert.AreEqual( 4, mockFinderResult.Count );

            AssertClean( searchPattern, mockFinderResult );
        }

        private void AssertClean( string[] searchPattern, List<string> expectedResult )
        {
            AssertDirectoriesExist( expectedResult );

            List<string> actualDeletedDirectories = _directoryCleaner.Clean( searchPattern );

            _mockFinder.Verify( x => x.Find( searchPattern ), Times.Once );

            Assert.AreEqual( expectedResult.Count, actualDeletedDirectories.Count );

            CollectionAssert.AreEquivalent( expectedResult, actualDeletedDirectories );

            AssertDirectoriesDoNotExist( expectedResult );
        }

        private List<string> CreateDirectoryStructure( out string rootDirectory )
        {
            string uniqueDirectory = Path.GetRandomFileName();
            rootDirectory = Path.Combine( _contextDirectory, uniqueDirectory );

            Directory.CreateDirectory( rootDirectory );

            List<string> directories = DirectoryUtils.CreateDirectoryStructure( rootDirectory );

            Assert.AreEqual( DirectoryUtils.CreateDirectoryStructureCount, directories.Count, string.Join( Environment.NewLine, directories ) );

            FileUtils.CreateReadOnlyFiles( directories, FileExt.Dir );

            AssertDirectoriesExist( directories );

            return directories;
        }

        private void AssertDirectoriesDoNotExist( IEnumerable<string> directories )
        {
            foreach( string directory in directories )
            {
                Assert.IsFalse( Directory.Exists( directory ) );
            }
        }

        private void AssertDirectoriesExist( IEnumerable<string> directories )
        {
            foreach( string directory in directories )
            {
                Assert.IsTrue( Directory.Exists( directory ) );
            }
        }
    }
}
