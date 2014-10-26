using System.Configuration;
using VisualStudioCleaner.Common;
using VisualStudioCleaner.Common.Domain;
using VisualStudioCleaner.DependencyResolution;
using VisualStudioCleaner.Workers;

namespace VisualStudioCleaner.UI.CommandLine
{
    internal static class WorkerFactory
    {
        public static IVSCleaner Create( CommandLineOptions options )
        {
            IRootDirectory root = new VSCleanerRootDirectory( options.Directory );

            string directories = ConfigurationManager.AppSettings["Directories"];
            string files = ConfigurationManager.AppSettings["Files"];
            string sccDirectories = ConfigurationManager.AppSettings["SourceControlDirectories"];
            string sccFiles = ConfigurationManager.AppSettings["SourceControlFiles"];

            if( options.CleanerOptions.HasOption( VSCleanerOptions.All, VSCleanerOptions.RemoveSourceControlBindings ) )
            {
                directories = string.Format( "{0};{1}", directories, sccDirectories );
                files = string.Format( "{0};{1}", files, sccFiles );
            }

            using( var container = new RootContainer( root ) )
            {
                var worker = container.Create<IVSCleaner>();
                worker.SetUp( directories, files, options.CleanerOptions );

                return worker;
            }
        }
    }
}