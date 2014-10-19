using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VisualStudioCleaner.UnitTestTools
{
    public static class AssertFile
    {
        /// <summary>
        /// Asserts that a file has the expected line count.
        /// </summary>
        /// <param name="file">The fully qualified path to the file.</param>
        /// <param name="lineCount">The expected line count.</param>
        public static void LineCount( string file, int lineCount )
        {
            string[] lines = File.ReadAllLines( file );
            Assert.AreEqual( lineCount, lines.Length );
        }

        /// <summary>
        /// Asserts that a file has (or does not have) the expected <see cref="FileAttributes"/> flag.
        /// </summary>
        /// <param name="file">The fully qualified path to the file.</param>
        /// <param name="flag">The expected <see cref="FileAttributes"/> flag.</param>
        /// <param name="assertIs">Assert true or Assert false.</param>
        public static void AttributeFlag( string file, FileAttributes flag, AssertIs assertIs )
        {
            FileAttributes attr = File.GetAttributes( file );

            if( assertIs == AssertIs.True )
            {
                Assert.IsTrue( attr.HasFlag( flag ) );
            }
            else
            {
                Assert.IsFalse( attr.HasFlag( flag ) );
            }
        }
    }
}