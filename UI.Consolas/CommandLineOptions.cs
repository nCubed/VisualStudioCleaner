using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using VisualStudioCleaner.Common;

namespace VisualStudioCleaner.UI.Consolas
{
    internal class CommandLineOptions
    {
        /// <summary>
        /// Auto build usage text. Using a lazy loader since there appears to be some type
        /// of issue with the CommandLine API that adds "--help" to the end of the help
        /// text every time help is called; or, more likely, I screwed up the implementation.
        /// </summary>
        private readonly Lazy<string> _usageText;

        /// <summary>
        /// Fully qualified path to the Visual Studio soltuion directory.
        /// </summary>
        [Option(
            'd', "directory",
            HelpText = "Fully qualified path to the Visual Studio soltuion directory.",
            Required = true )]
        public string Directory { get; set; }

        /// <summary>
        /// VSCleaner Options: Default, ExcludeFiles, ExcludeDirectories, RemoveSourceControlBindings, All.
        /// </summary>
        [OptionList(
            'o', "options",
            Separator = '|',
            HelpText = "VSCleaner Options: Default, ExcludeFiles, ExcludeDirectories, RemoveSourceControlBindings, All.",
            Required = false )]
        public IList<string> Options { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return _usageText.Value;
        }

        public VSCleanerOptions CleanerOptions { get { return ParseOptions(); } }

        public CommandLineOptions()
        {
            _usageText = new Lazy<string>( () => HelpText.AutoBuild( this, text => HelpText.DefaultParsingErrorsHandler( this, text ) ) );

            Options = new List<string> { VSCleanerOptions.Default.ToString() };
        }

        private VSCleanerOptions ParseOptions()
        {
            var options = VSCleanerOptions.Default;

            foreach( string text in Options )
            {
                VSCleanerOptions opt;
                if( Enum.TryParse( text, true, out opt ) )
                {
                    options = options | opt;
                }
            }

            return options;
        }
    }
}