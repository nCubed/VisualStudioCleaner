using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VisualStudioCleaner.UnitTestTools
{
    public static class DirectoryUtils
    {
        /// <summary>
        /// Creates a directory in the given <see cref="TestContext"/>'s directory.
        /// </summary>
        /// <param name="context">The <see cref="TestContext"/> to use.</param>
        /// <param name="directoryName">The name of the directory to create.</param>
        /// <returns>The fully qualified path of the directory.</returns>
        public static string CreateContextDirectory( TestContext context, string directoryName )
        {
            // NCrunch will use TestDir even though it's deprecated.
            string contextDir = string.IsNullOrWhiteSpace( context.TestRunDirectory )
                ? context.TestDir
                : context.TestRunDirectory;

            string dir = Path.Combine( contextDir, directoryName );

            Directory.CreateDirectory( dir );

            return dir;
        }

        /// <summary>
        /// Deletes the given <see cref="TestContext"/>'s directory and all sub directories.
        /// </summary>
        /// <param name="directoryName">The name of the directory to delete.</param>
        public static void CleanupContextDirectory( string directoryName )
        {
            var directories = Directory.GetDirectories( directoryName );
            foreach( string dir in directories )
            {
                CleanupContextDirectory( dir );
            }

            var di = new DirectoryInfo( directoryName )
            {
                Attributes = FileAttributes.Normal
            };

            FileInfo[] files = di.GetFiles();
            foreach( FileInfo file in files )
            {
                file.Attributes = FileAttributes.Normal;
            }

            di.Delete( true );
        }

        public const int CreateDirectoryStructureCount = 9;

        /// <summary>
        /// Creates a directory structure. See the <see cref="CreateDirectoryStructureCount"/>
        /// constant for the total expected number of directories that will be created.
        /// </summary>
        /// <param name="rootDirectory">The root directory to build the directory structure from.</param>
        /// <returns></returns>
        public static List<string> CreateDirectoryStructure( string rootDirectory )
        {
            Assert.IsTrue( Directory.Exists( rootDirectory ) );

            var dir1 = new List<string> { "a", "ab", "abc", "abcd", "abcde" }; // 5 new directories
            var dir2 = new List<string> { "a", "ab", "abc", "abcd", "xxx" }; // 1 new directory
            var dir3 = new List<string> { "a", "ab", "abc", "xxx" }; // 1 new directory
            var dir4 = new List<string> { "a", "ab", "xxx" }; // 1 new directory
            var dir5 = new List<string> { "a", "xxx" }; // 1 new directory

            List<string> dirs = CreateDirectoryStructure( rootDirectory, dir1, dir2, dir3, dir4, dir5 );

            Assert.AreEqual( CreateDirectoryStructureCount, dirs.Count );

            return dirs;
        }

        public static List<string> CreateDirectoryStructure( string rootDirectory, params List<string>[] dirs )
        {
            foreach( List<string> path in dirs )
            {
                path.Insert( 0, rootDirectory );

                string dir = Path.Combine( path.ToArray() );

                Assert.IsFalse( Directory.Exists( dir ) );

                Directory.CreateDirectory( dir );

                Assert.IsTrue( Directory.Exists( dir ) );
            }

            return Directory.GetDirectories( rootDirectory, "*", SearchOption.AllDirectories ).ToList();
        }
    }
}
