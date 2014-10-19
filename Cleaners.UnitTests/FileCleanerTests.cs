using System;
using System.Collections.Generic;
using System.IO;
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
    public class FileCleanerTests
    {
        private static string _contextDirectory;

        private FileCleaner _cleaner;
        private Mock<IFileFinder> _mockFinder;

        [ClassInitialize]
        public static void ClassInit( TestContext context )
        {
            _contextDirectory = DirectoryUtils.CreateContextDirectory( context, "_FileCleanerTests" );
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            DirectoryUtils.CleanupContextDirectory( _contextDirectory );
        }

        [TestInitialize]
        public void TestInit()
        {
            _mockFinder = new Mock<IFileFinder>( MockBehavior.Strict );

            _cleaner = new FileCleaner( _mockFinder.Object );
        }

        [TestMethod]
        public void FileCleaner_Implements_IFileCleaner()
        {
            AssertClass<FileCleaner>.ImplementsInterface<IFileCleaner>();
        }

        [TestMethod]
        public void FileCleaner_IsInternal()
        {
            AssertClass<FileCleaner>.IsInternal();
        }

        [TestMethod]
        public void FileCleaner_IsSealed()
        {
            AssertClass<FileCleaner>.IsSealed();
        }

        [TestMethod]
        public void Clean_Deletes_AllFiles_Ext_1()
        {
            var extensions = new[] { FileExt.Ext1 };

            AssertFileCleaner( extensions );
        }

        [TestMethod]
        public void Clean_Deletes_AllFiles_Ext_2_3()
        {
            var extensions = new[] { FileExt.Ext2, FileExt.Ext3 };

            AssertFileCleaner( extensions );
        }

        [TestMethod]
        public void Clean_Deletes_AllFiles_Ext_4_5_6()
        {
            var extensions = new[] { FileExt.Ext4, FileExt.Ext5, FileExt.Ext6 };

            AssertFileCleaner( extensions );
        }

        private void AssertFileCleaner( string[] extensions )
        {
            int expectedNumberOfFilesCreated = DirectoryUtils.CreateDirectoryStructureCount * extensions.Length;

            string rootDirectory;
            List<string> createdFiles = CreateDirectoryStructure( extensions, out rootDirectory );

            Assert.AreEqual( expectedNumberOfFilesCreated, createdFiles.Count );

            _mockFinder.Setup( x => x.Find( extensions ) ).Returns( createdFiles );

            List<string> actualDeletedFiles = _cleaner.Clean( extensions );

            _mockFinder.Verify( x => x.Find( extensions ), Times.Once );

            Assert.AreEqual( createdFiles.Count, actualDeletedFiles.Count );

            AssertFilesDoNotExist( createdFiles );
        }

        private List<string> CreateDirectoryStructure( string[] extensions, out string rootDirectory )
        {
            string uniqueDirectory = Path.GetRandomFileName();
            rootDirectory = Path.Combine( _contextDirectory, uniqueDirectory );

            Directory.CreateDirectory( rootDirectory );

            List<string> directories = DirectoryUtils.CreateDirectoryStructure( rootDirectory );

            Assert.AreEqual( DirectoryUtils.CreateDirectoryStructureCount, directories.Count, string.Join( Environment.NewLine, directories ) );

            List<string> files = FileUtils.CreateReadOnlyFiles( directories, extensions );

            AssertFilesExist( files );

            // include other files not included in the extensions to ensure the file cleaner
            // does not pick up files other than the extensions provided.
            List<string> otherFiles = FileUtils.CreateReadOnlyFiles( directories, FileExt.File, FileExt.Dir );
            AssertFilesExist( otherFiles );

            return files;
        }

        private void AssertFilesDoNotExist( IEnumerable<string> files )
        {
            foreach( string file in files )
            {
                Assert.IsFalse( File.Exists( file ) );
            }
        }

        private void AssertFilesExist( IEnumerable<string> files )
        {
            foreach( string file in files )
            {
                Assert.IsTrue( File.Exists( file ) );
            }
        }
    }
}