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
    public class ProjectFileCleanerTests
    {
        private static string _contextDirectory;
        private static IEnumerable<string> _extensionsToFind;
        private const string ExpectedFileExtension = ".csproj";

        private ProjectFileCleaner _cleaner;
        private Mock<IFileFinder> _mockFinder;

        [ClassInitialize]
        public static void ClassInit( TestContext context )
        {
            _contextDirectory = DirectoryUtils.CreateContextDirectory( context, "_ProjectFileCleanerTests" );
            _extensionsToFind = new[] { ExpectedFileExtension };
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
            _cleaner = new ProjectFileCleaner( _mockFinder.Object );
        }

        [TestMethod]
        public void SolutionFileCleaner_Implements_ISourceControlFileCleaner()
        {
            AssertClass<ProjectFileCleaner>.ImplementsInterface<IProjectFileCleaner>();
            AssertClass<ProjectFileCleaner>.ImplementsInterface<ISourceControlCleaner>();
        }

        [TestMethod]
        public void ProjectFileCleaner_Inherits_SourceControlFileCleanerBase()
        {
            AssertClass<ProjectFileCleaner>.InheritsAbstractBaseClass<SourceControlFileCleanerBase>();
        }

        [TestMethod]
        public void ProjecFileCleaner_IsInternal()
        {
            AssertClass<ProjectFileCleaner>.IsInternal();
        }

        [TestMethod]
        public void ProjectFileCleaner_IsSealed()
        {
            AssertClass<ProjectFileCleaner>.IsSealed();
        }

        [TestMethod]
        public void ProjectFileCleaner_FileExtension_Is_csproj()
        {
            Assert.AreEqual( ExpectedFileExtension, _cleaner.FileExtension );
        }

        [TestMethod]
        public void Clean_RemovesSourceControlSettings_FromProjectFile()
        {
            string file = FileUtils.CreateFile( Consts.ProjectFile.Trim(), _contextDirectory, _cleaner.FileExtension );

            _mockFinder.Setup( x => x.Find( _extensionsToFind ) )
                .Returns( () => new List<string> { file } );

            AssertFileLineCount( file, FileState.IsNotCleansed );

            _cleaner.Clean();

            _mockFinder.Verify( x => x.Find( _extensionsToFind ), Times.Once );

            AssertFileLineCount( file, FileState.IsCleansed );
        }

        [TestMethod]
        public void Clean_RemovesSourceControlSettings_FromReadonlyProjectFile()
        {
            string readonlyFile = FileUtils.CreateReadonlyFile( Consts.ProjectFile.Trim(), _contextDirectory, _cleaner.FileExtension );

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
            const int originalLineCount = 68;
            const int cleansedLineCount = 68 - 4; // 4 lines should be removed from the test file.

            int lineCount = state == FileState.IsCleansed ? cleansedLineCount : originalLineCount;

            AssertFile.LineCount( file, lineCount );
        }

        private static class Consts
        {
            public const string ProjectFile = @"
<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProjectGuid>{B73A7274-70A1-48AD-BF0D-C434DC3712C2}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>ImperialArmy.Common</RootNamespace>
    <AssemblyName>ImperialArmy.Common</AssemblyName>
    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <SccProjectName>SAK</SccProjectName>
    <SccLocalPath>SAK</SccLocalPath>
    <SccAuxPath>SAK</SccAuxPath>
    <SccProvider>SAK</SccProvider>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""System"" />
    <Reference Include=""System.Core"" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include=""Inventory\IBowAndArrow.cs"" />
    <Compile Include=""Soldier\IFlyingRainOfFire.cs"" />
    <Compile Include=""Soldier\IFootSoldier.cs"" />
    <Compile Include=""Soldier\IHorseman.cs"" />
    <Compile Include=""Soldier\IArcher.cs"" />
    <Compile Include=""Inventory\IHorse.cs"" />
    <Compile Include=""SoldierOrder\IFightToTheDeath.cs"" />
    <Compile Include=""SoldierOrder\ILeadTheCharge.cs"" />
    <Compile Include=""SoldierOrder\IShootDistantFoes.cs"" />
    <Compile Include=""SoldierOrder\ISoldierOrders.cs"" />
    <Compile Include=""Soldier\ISoldier.cs"" />
    <Compile Include=""Properties\AssemblyInfo.cs"" />
    <Compile Include=""Rank.cs"" />
    <Compile Include=""SoldierOrder\ITrampleFoes.cs"" />
  </ItemGroup>
  <ItemGroup>
    <None Include=""_designFiles\Common.cd"" />
    <None Include=""_designFiles\Common.Soldiers.cd"" />
  </ItemGroup>
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name=""BeforeBuild"">
  </Target>
  <Target Name=""AfterBuild"">
  </Target>
  -->
</Project>";

        }

    }
}