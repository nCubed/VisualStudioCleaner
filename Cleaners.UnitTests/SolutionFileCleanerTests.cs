using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TypeAsserter;
using VisualStudioCleaner.Cleaners.SourceControlCleaners;
using VisualStudioCleaner.Common.Domain.Cleaners;
using VisualStudioCleaner.Common.Domain.Finders;
using VisualStudioCleaner.UnitTestTools;

namespace VisualStudioCleaner.Cleaners.UnitTests
{
    [TestClass]
    public class SolutionFileCleanerTests
    {
        private static string _contextDirectory;
        private static IEnumerable<string> _extensionsToFind;
        private const string ExpectedFileExtension = ".sln";

        private SolutionFileCleaner _cleaner;
        private Mock<IFileFinder> _mockFinder;

        [ClassInitialize]
        public static void ClassInit( TestContext context )
        {
            _contextDirectory = DirectoryUtils.CreateContextDirectory( context, "_SolutionFileCleaner" );
            _extensionsToFind = new[] { ExpectedFileExtension };
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Directory.Delete( _contextDirectory, true );
        }

        [TestInitialize]
        public void TestInit()
        {
            _mockFinder = new Mock<IFileFinder>( MockBehavior.Strict );
            _cleaner = new SolutionFileCleaner( _mockFinder.Object );
        }

        [TestMethod]
        public void SolutionFileCleaner_Implements_ISourceControlFileCleaner()
        {
            AssertClass<SolutionFileCleaner>.ImplementsInterface<ISolutionFileCleaner>();
            AssertClass<SolutionFileCleaner>.ImplementsInterface<ISourceControlCleaner>();
        }

        [TestMethod]
        public void SolutionFileCleaner_Inherits_SourceControlFileCleanerBase()
        {
            AssertClass<SolutionFileCleaner>.InheritsAbstractBaseClass<SourceControlFileCleanerBase>();
        }

        [TestMethod]
        public void SolutionFileCleaner_IsInternal()
        {
            AssertClass<SolutionFileCleaner>.IsInternal();
        }

        [TestMethod]
        public void SolutionFileCleaner_IsSealed()
        {
            AssertClass<SolutionFileCleaner>.IsSealed();
        }

        [TestMethod]
        public void SolutionFileCleaner_FileExtension_Is_sln()
        {
            Assert.AreEqual( ExpectedFileExtension, _cleaner.FileExtension );
        }


        [TestMethod]
        public void Cleanse_RemovesSourceControlSettings_FromSolutionFile()
        {
            string file = FileUtils.CreateFile( Consts.SolutionFile.Trim(), _contextDirectory, _cleaner.FileExtension );

            _mockFinder.Setup( x => x.Find( _extensionsToFind ) )
               .Returns( () => new List<string> { file } );

            AssertFileLineCount( file, FileState.IsNotCleansed );

            _cleaner.Clean();

            _mockFinder.Verify( x => x.Find( _extensionsToFind ), Times.Once );

            AssertFileLineCount( file, FileState.IsCleansed );
        }

        [TestMethod]
        public void Cleanse_RemovesSourceControlSettings_FromReadOnlySolutionFile()
        {
            string readonlyFile = FileUtils.CreateReadonlyFile( Consts.SolutionFile.Trim(), _contextDirectory, _cleaner.FileExtension );

            _mockFinder.Setup( x => x.Find( _extensionsToFind ) )
               .Returns( () => new List<string> { readonlyFile } );

            AssertFile.AttributeFlag( readonlyFile, FileAttributes.ReadOnly, AssertIs.True );

            AssertFileLineCount( readonlyFile, FileState.IsNotCleansed );

            _cleaner.Clean();

            _mockFinder.Verify( x => x.Find( _extensionsToFind ), Times.Once );

            AssertFile.AttributeFlag( readonlyFile, FileAttributes.ReadOnly, AssertIs.True );

            AssertFileLineCount( readonlyFile, FileState.IsCleansed );

            FileUtils.ClearFileAttributes( readonlyFile );

            AssertFile.AttributeFlag( readonlyFile, FileAttributes.ReadOnly, AssertIs.False );
        }

        private static void AssertFileLineCount( string file, FileState state )
        {
            const int originalLineCount = 53;
            const int cleansedLineCount = 53 - 15; // 15 lines should be removed from the test file.

            int lineCount = state == FileState.IsCleansed ? cleansedLineCount : originalLineCount;

            AssertFile.LineCount( file, lineCount );
        }

        private static class Consts
        {
            public const string SolutionFile = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""ImperialArmy.Common"", ""ImperialArmy.Common\ImperialArmy.Common.csproj"", ""{B73A7274-70A1-48AD-BF0D-C434DC3712C2}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""ImperialArmy.Military.UnitTests"", ""ImperialArmy.Military.UnitTests\ImperialArmy.Military.UnitTests.csproj"", ""{5E746B1C-E1CC-48BE-A567-2CD296E9A82F}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""ImperialArmy.Military"", ""ImperialArmy.Military\ImperialArmy.Military.csproj"", ""{5F1C3D26-A112-415B-B545-05F40CCA45E4}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = "".nuget"", "".nuget"", ""{178D2D37-3495-4A95-9B93-7E8B0C04F705}""
	ProjectSection(SolutionItems) = preProject
		.nuget\NuGet.Config = .nuget\NuGet.Config
		.nuget\NuGet.exe = .nuget\NuGet.exe
		.nuget\NuGet.targets = .nuget\NuGet.targets
	EndProjectSection
EndProject
Global
	GlobalSection(TeamFoundationVersionControl) = preSolution
		SccNumberOfProjects = 4
		SccEnterpriseProvider = {4CA58AB2-18FA-4F8D-95D4-32DDF27D184C}
		SccTeamFoundationServer = https://github.com/nCubed
		SccLocalPath0 = .
		SccProjectUniqueName1 = ImperialArmy.Common\\ImperialArmy.Common.csproj
		SccProjectName1 = ImperialArmy.Common
		SccLocalPath1 = ImperialArmy.Common
		SccProjectUniqueName2 = ImperialArmy.Military.UnitTests\\ImperialArmy.Military.UnitTests.csproj
		SccProjectName2 = ImperialArmy.Military.UnitTests
		SccLocalPath2 = ImperialArmy.Military.UnitTests
		SccProjectUniqueName3 = ImperialArmy.Military\\ImperialArmy.Military.csproj
		SccProjectName3 = ImperialArmy.Military
		SccLocalPath3 = ImperialArmy.Military
	EndGlobalSection
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{B73A7274-70A1-48AD-BF0D-C434DC3712C2}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{B73A7274-70A1-48AD-BF0D-C434DC3712C2}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{B73A7274-70A1-48AD-BF0D-C434DC3712C2}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{B73A7274-70A1-48AD-BF0D-C434DC3712C2}.Release|Any CPU.Build.0 = Release|Any CPU
		{5E746B1C-E1CC-48BE-A567-2CD296E9A82F}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{5E746B1C-E1CC-48BE-A567-2CD296E9A82F}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{5E746B1C-E1CC-48BE-A567-2CD296E9A82F}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{5E746B1C-E1CC-48BE-A567-2CD296E9A82F}.Release|Any CPU.Build.0 = Release|Any CPU
		{5F1C3D26-A112-415B-B545-05F40CCA45E4}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{5F1C3D26-A112-415B-B545-05F40CCA45E4}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{5F1C3D26-A112-415B-B545-05F40CCA45E4}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{5F1C3D26-A112-415B-B545-05F40CCA45E4}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal";

        }

    }
}