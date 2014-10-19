using System.IO;

namespace VisualStudioCleaner.UI.Consolas
{
    static class ResetTestDirectories
    {
        public const string RootDirectory = @"C:\_dev\_tmp\VSCleaner\";

        public static void Go( int i )
        {
            Create( i );
        }

        private static void Create( int max )
        {
            string sourceDir = Path.Combine( RootDirectory, "_sample" );
            string targetDir = Path.Combine( RootDirectory, "vs" );

            if( !Directory.Exists( sourceDir ) )
            {
                System.Console.WriteLine( "Unable to create test directories. No directory exists:" );
                System.Console.WriteLine( sourceDir );
                return;
            }

            var source = new DirectoryInfo( sourceDir );

            for( int i = 1; i <= max; i++ )
            {
                var target = new DirectoryInfo( targetDir + i );

                CopyFiles( source, target );
            }
        }

        static void CopyFiles( DirectoryInfo source, DirectoryInfo target )
        {
            foreach( DirectoryInfo dir in source.GetDirectories() )
            {
                CopyFiles( dir, target.CreateSubdirectory( dir.Name ) );
            }

            foreach( FileInfo file in source.GetFiles() )
            {
                file.CopyTo( Path.Combine( target.FullName, file.Name ), true );
            }
        }
    }
}