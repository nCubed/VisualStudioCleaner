using System;
using VisualStudioCleaner.Common;

namespace VisualStudioCleaner.UI.Consolas
{
    internal static class Approver
    {
        public static bool IsApproved( CommandLineOptions options, Action<string> speaker )
        {
            Console.WriteLine( "Directory: {0}", options.Directory );
            Console.WriteLine( "Options  : {0}", options.CleanerOptions );
            Console.Write( "Do you want to continue? (Y or N): " );

            ConsoleKey key = Console.ReadKey().Key;
            Console.WriteLine( Environment.NewLine );

            if( key == ConsoleKey.Y )
            {
                if( options.CleanerOptions.HasOption( VSCleanerOptions.All, VSCleanerOptions.RemoveSourceControlBindings ) )
                {
                    return IsSourceControlApproved( speaker );
                }

                return true;
            }

            if( key == ConsoleKey.N )
            {
                Console.WriteLine( "\t...Canceled by user..." );
                return false;
            }

            Console.Clear();

            return IsApproved( options, speaker );
        }

        private static bool IsSourceControlApproved( Action<string> speaker )
        {
            speaker( "Remove Source Control Bindings?" );

            Console.WriteLine( "You've selected to remove Source Control Bindings." );
            Console.Write( "Are you sure? (Y or N): " );

            ConsoleKey key = Console.ReadKey().Key;
            Console.WriteLine( Environment.NewLine );

            if( key == ConsoleKey.Y )
            {
                return true;
            }

            if( key == ConsoleKey.N )
            {
                Console.WriteLine( "\t...Canceled by user..." );
                return false;
            }

            Console.Clear();

            return IsSourceControlApproved( speaker );
        }
    }
}