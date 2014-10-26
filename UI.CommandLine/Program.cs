using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CommandLine;
using VisualStudioCleaner.Workers;

namespace VisualStudioCleaner.UI.CommandLine
{
    class Program
    {
        static void Main( string[] args )
        {
            ConditionalRebuildTestDirectories();

            ConditionalOverrideArgs( ref args );

            var options = new CommandLineOptions();

            if( !Parser.Default.ParseArguments( args, options ) )
            {
                Console.WriteLine( "Invalid command line arguments. See help text above." );
                Console.ReadLine();
                return;
            }

            if( Approver.IsApproved( options, str => Speak( str ) ) )
            {
                Speak( "Cleaning..." );

                try
                {
                    Clean( options );
                }
                catch( Exception ex )
                {
                    Speak( "An Error Occurred" );
                    Console.WriteLine( ex.Message );
                }
            }

            Speak( "Done!" );

            ConditionalPause();
        }

        private static void Clean( CommandLineOptions options )
        {
            IVSCleaner worker = WorkerFactory.Create( options );

            IVSCleanerResults result = worker.DoWork();

            var sb = new StringBuilder();

            BuildOptionsResult( sb, options );
            BuildItemsResult( sb, "Directories Deleted", result.DirectoriesDeleted );
            BuildItemsResult( sb, "Files Deleted", result.FilesDeleted );
            BuildItemsResult( sb, "Source Control Files Cleaned", result.SouceControlFilesCleaned );

            WriteLogAndOpen( sb );
        }

        private static void BuildOptionsResult( StringBuilder sb, CommandLineOptions options )
        {
            sb.AppendLine( string.Format( "Directory: {0}", options.Directory ) );
            sb.AppendLine( string.Format( "Options: {0}", options.CleanerOptions ) );
        }

        private static void BuildItemsResult( StringBuilder sb, string header, IEnumerable<string> result )
        {
            List<string> items = result.ToList();

            AddHeader( sb, string.Format( "{0}: {1}", header, items.Count ) );

            foreach( string item in items )
            {
                sb.AppendLine( item );
            }
        }

        private static void AddHeader( StringBuilder sb, string header )
        {
            var border = new string( '=', 50 );

            sb.AppendLine();
            sb.AppendLine( border );
            sb.AppendLine( header );
            sb.AppendLine( border );
        }

        private static void Speak( string str, bool newLineAtTop = true )
        {
            Console.WriteLine( Whisper( str, newLineAtTop ) );
        }

        private static string Whisper( string str, bool newLineAtTop = true )
        {
            string topLine = newLineAtTop ? Environment.NewLine : string.Empty;
            string top = topLine + new string( '=', 50 ) + Environment.NewLine;
            string bottom = Environment.NewLine + new string( '=', 50 ) + Environment.NewLine;

            string result = string.Format( "{0}Visual Studio Cleaner Says: {1}{2}", top, str, bottom );

            return result;
        }

        private static void WriteLogAndOpen( StringBuilder sb )
        {
            string dir = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );

            if( string.IsNullOrWhiteSpace( dir ) || !Directory.Exists( dir ) )
            {
                return;
            }

            string fileName = string.Format( "VSCleaner-{0}.txt", DateTime.Now.ToString( "yyyy-MM-dd-hh-mm-ss" ) );
            string file = Path.Combine( dir, fileName );

            sb.Insert( 0, Whisper( "Results...", false ) );
            sb.AppendLine( Whisper( "Done!" ) );

            File.WriteAllText( file, sb.ToString() );
            Process.Start( file );
        }

        [Conditional( "DEBUG" )]
        [Conditional( "RELEASE_LOCAL" )]
        private static void ConditionalRebuildTestDirectories()
        {
            ResetTestDirectories.Go( 1 );
        }

        [Conditional( "DEBUG" )]
        // ReSharper disable once RedundantAssignment
        private static void ConditionalOverrideArgs( ref string[] args )
        {
            Speak( "Running in Debug Mode. Args are overriden.", false );

            string dir = Path.Combine( ResetTestDirectories.RootDirectory, "vs1" );
            const string options = "All";

            args = new[] { "-d", dir, "-o", options };
        }

        [Conditional( "DEBUG" )]
        [Conditional( "RELEASE_LOCAL" )]
        private static void ConditionalPause()
        {
            Console.ReadLine();
        }
    }
}
