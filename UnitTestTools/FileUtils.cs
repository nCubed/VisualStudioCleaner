using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VisualStudioCleaner.UnitTestTools
{
    public static class FileUtils
    {
        /// <summary>
        /// Creates a file with a random file name.
        /// </summary>
        /// <param name="contents">The contents of the file.</param>
        /// <param name="directory">The directory to create the file.</param>
        /// <param name="fileExtension">The file extension to use with the new file.</param>
        /// <returns>The fully qualified path of the file created.</returns>
        public static string CreateFile( string contents, string directory, string fileExtension )
        {
            string name = Guid.NewGuid().ToString();
            string path = Path.Combine( directory, name );
            string file = Path.ChangeExtension( path, fileExtension );

            Assert.IsFalse( File.Exists( file ) );

            File.WriteAllText( file, contents );

            Assert.IsTrue( File.Exists( file ) );

            AssertFile.AttributeFlag( file, FileAttributes.ReadOnly, AssertIs.False );

            return file;
        }

        /// <summary>
        /// Creates a readonly file with a random file name.
        /// </summary>
        /// <param name="contents">The contents of the file.</param>
        /// <param name="directory">The directory to create the file.</param>
        /// <param name="fileExtension">The file extension to use with the new file.</param>
        /// <returns>The fully qualified path of the file created.</returns>
        public static string CreateReadonlyFile( string contents, string directory, string fileExtension )
        {
            string file = CreateFile( contents, directory, fileExtension );

            File.SetAttributes( file, FileAttributes.ReadOnly );

            AssertFile.AttributeFlag( file, FileAttributes.ReadOnly, AssertIs.True );

            return file;
        }

        public static List<string> CreateReadOnlyFiles( IEnumerable<string> directories, params string[] fileExtensions )
        {
            var files = new List<string>();

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach( string dir in directories )
            {
                foreach( string ext in fileExtensions )
                {
                    string file = CreateReadonlyFile( dir, dir, ext );
                    files.Add( file );
                }
            }
            // ReSharper restore LoopCanBeConvertedToQuery

            return files;
        }

        /// <summary>
        /// Resets a file to a normal state, i.e., not readonly.
        /// </summary>
        /// <param name="file">The fully qualified path to the file.</param>
        public static void ClearFileAttributes( string file )
        {
            File.SetAttributes( file, FileAttributes.Normal );
        }
    }
}