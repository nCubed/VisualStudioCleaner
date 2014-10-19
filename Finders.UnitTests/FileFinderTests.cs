using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeAsserter;
using VisualStudioCleaner.Common.Domain.Finders;
using VisualStudioCleaner.UnitTestTools;

namespace VisualStudioCleaner.Finders.UnitTests
{
    [TestClass]
    public class FileFinderTests
    {
        private static string _contextDirectory;
        private static List<string> _allDirectories;

        private FileFinder _fileFinder;

        [ClassInitialize]
        public static void ClassInit( TestContext context )
        {
            _contextDirectory = DirectoryUtils.CreateContextDirectory( context, "_FileFinderTests" );

            _allDirectories = DirectoryUtils.CreateDirectoryStructure( _contextDirectory );

            // add files that the finders should not be finding.
            FileUtils.CreateReadOnlyFiles( _allDirectories, FileExt.File, FileExt.Dir );
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            DirectoryUtils.CleanupContextDirectory( _contextDirectory );
        }

        [TestInitialize]
        public void TestInit()
        {
            _fileFinder = new FileFinder( _contextDirectory );
        }

        [TestMethod]
        public void FileFinder_Implements_IFileFinder()
        {
            AssertClass<FileFinder>.ImplementsInterface<IFileFinder>();
            AssertClass<FileFinder>.ImplementsInterface<IFinder>();
        }

        [TestMethod]
        public void FileFinder_IsInternal()
        {
            AssertClass<FileFinder>.IsInternal();
        }

        [TestMethod]
        public void FileFinder_IsSealed()
        {
            AssertClass<FileFinder>.IsSealed();
        }

        [TestMethod]
        public void Find_Returns_SameFileExtension_InAllDirectories_Ext_1()
        {
            var expectedFiles = new List<string>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach( string d in _allDirectories )
            {
                string file = FileUtils.CreateFile( d, d, FileExt.Ext1 );
                expectedFiles.Add( file );
            }

            var fileExtensions = new[] { FileExt.Ext1 };
            List<string> actualFiles = _fileFinder.Find( fileExtensions );

            Assert.AreEqual( DirectoryUtils.CreateDirectoryStructureCount, actualFiles.Count );
            CollectionAssert.AreEquivalent( expectedFiles, actualFiles );
        }

        [TestMethod]
        public void Find_Returns_OneFile_FromAllDirectories_Ext_2()
        {
            const int middleDirectory = DirectoryUtils.CreateDirectoryStructureCount / 2;
            string dir = _allDirectories.Skip( middleDirectory ).First();
            string file = FileUtils.CreateFile( dir, dir, FileExt.Ext2 );

            var fileExtensions = new[] { FileExt.Ext2 };
            List<string> actualFiles = _fileFinder.Find( fileExtensions );

            Assert.AreEqual( 1, actualFiles.Count );
            Assert.AreEqual( file, actualFiles.Single() );
        }

        [TestMethod]
        public void Find_Matches_TwoFileExtenions_InAllDirectories_Ext_3_4()
        {
            const int expectedFileCount = DirectoryUtils.CreateDirectoryStructureCount * 2;
            var expectedFiles = new List<string>();

            foreach( string d in _allDirectories )
            {
                string fileExt3 = FileUtils.CreateFile( d, d, FileExt.Ext3 );
                expectedFiles.Add( fileExt3 );

                string fileExt4 = FileUtils.CreateFile( d, d, FileExt.Ext4 );
                expectedFiles.Add( fileExt4 );
            }

            var fileExtensions = new[] { FileExt.Ext3, FileExt.Ext4 };
            List<string> actualFiles = _fileFinder.Find( fileExtensions );

            Assert.AreEqual( expectedFileCount, actualFiles.Count );
            CollectionAssert.AreEquivalent( expectedFiles, actualFiles );
        }

        [TestMethod]
        public void Find_Matches_MultipleFileExtensions_InDifferentDirectories_Ext_5_6_7()
        {
            string dir1 = _allDirectories.Skip( 1 ).First();
            string dir2 = _allDirectories.Skip( 2 ).First();
            string dir3 = _allDirectories.Skip( 3 ).First();
            string file1 = FileUtils.CreateFile( dir1, dir1, FileExt.Ext5 );
            string file2 = FileUtils.CreateFile( dir2, dir2, FileExt.Ext6 );
            string file3 = FileUtils.CreateFile( dir3, dir3, FileExt.Ext7 );

            var fileExtensions = new[] { FileExt.Ext5, FileExt.Ext6, FileExt.Ext7 };
            Assert.AreEqual( 3, fileExtensions.Distinct().Count() );

            List<string> actualFiles = _fileFinder.Find( fileExtensions );

            Assert.AreEqual( 3, actualFiles.Count );
            Assert.IsTrue( actualFiles.Contains( file1 ) );
            Assert.IsTrue( actualFiles.Contains( file2 ) );
            Assert.IsTrue( actualFiles.Contains( file3 ) );
        }
    }
}