using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeAsserter;
using VisualStudioCleaner.Common.Domain.Cleaners;
using VisualStudioCleaner.Common.Domain.Finders;
using VisualStudioCleaner.DependencyResolution;
using VisualStudioCleaner.UnitTestTools;
using VisualStudioCleaner.Workers;

namespace DependencyResolution.UnitTests
{
    [TestClass]
    public class RootContainerTests
    {
        private static string _contextDirectory;
        private static RootContainer _container;

        [ClassInitialize]
        public static void ClassInit( TestContext context )
        {
            _contextDirectory = DirectoryUtils.CreateContextDirectory( context, "_RootContainerTests" );
            _container = new RootContainer( new MockRootDirectory( _contextDirectory ) );
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _container.Dispose();
        }

        [TestMethod]
        public void RootContainer_Implements_IDisposable()
        {
            AssertClass<RootContainer>.ImplementsInterface<IDisposable>();
        }

        [TestMethod]
        public void Create_Returns_IFileFinder()
        {
            var obj = AssertCreate<IFileFinder>();

            Assert.AreEqual( _contextDirectory, obj.RootDirectory );
        }

        [TestMethod]
        public void Create_Returns_IDirectoryFinder()
        {
            var obj = AssertCreate<IDirectoryFinder>();

            Assert.AreEqual( _contextDirectory, obj.RootDirectory );
        }

        [TestMethod]
        public void Create_Returns_ISolutionFileCleaner()
        {
            AssertCreate<ISolutionFileCleaner>();
        }

        [TestMethod]
        public void Create_Returns_IProjectFileCleaner()
        {
            AssertCreate<IProjectFileCleaner>();
        }

        [TestMethod]
        public void Create_Returns_IFileCleaner()
        {
            AssertCreate<IFileCleaner>();
        }

        [TestMethod]
        public void Create_Returns_IDirectoryCleaner()
        {
            AssertCreate<IDirectoryCleaner>();
        }

        [TestMethod]
        public void Create_Returns_IVSCleanerTools()
        {
            AssertCreate<IVSCleanerTools>();
        }

        [TestMethod]
        [ExpectedException( typeof( ImportCardinalityMismatchException ) )]
        public void Create_Exception_IFinder()
        {
            AssertCreate<IFinder>();
        }

        [TestMethod]
        [ExpectedException( typeof( ImportCardinalityMismatchException ) )]
        public void Create_Exception_ISourceControlCleaner()
        {
            AssertCreate<ISourceControlCleaner>();
        }

        private T AssertCreate<T>()
        {
            var obj = _container.Create<T>();

            Assert.IsNotNull( obj );
            Assert.IsInstanceOfType( obj, typeof( T ) );

            return obj;
        }
    }
}
