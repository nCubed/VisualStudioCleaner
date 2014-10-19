using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using VisualStudioCleaner.Common.Domain;

namespace VisualStudioCleaner.DependencyResolution
{
    public sealed class RootContainer : IDisposable
    {
        private readonly CompositionContainer _container;

        public RootContainer( IRootDirectory rootDirectory )
        {
            CoerceReferences();
            _container = ComposeByAssembly( rootDirectory );
        }

        public T Create<T>()
        {
            return _container.GetExportedValue<T>();
        }

        private CompositionContainer ComposeByAssembly( IRootDirectory rootDirectory )
        {
            var assemblies = new List<Assembly>();
            assemblies.AddRange( GetAssemblies( Assembly.GetExecutingAssembly() ) );

            // finds all objects decorated with the Export attribute.
            // OfType<ComposablePartCatalog> is redundant, but good to clarify code intent.
            // ReSharper disable once RedundantEnumerableCastCall
            var parts = assemblies.Distinct().Select( x => new AssemblyCatalog( x ) ).OfType<ComposablePartCatalog>();

            var catalog = new AggregateCatalog( parts );

            var container = new CompositionContainer( catalog );

            var batch = new CompositionBatch();
            batch.AddExportedValue( rootDirectory );

            container.Compose( batch );

            return container;
        }

        private IEnumerable<Assembly> GetAssemblies( Assembly assembly )
        {
            if( assembly == null )
            {
                yield break;
            }

            yield return assembly;

            var referenced = assembly.GetReferencedAssemblies()
                .Where( x => x.FullName.StartsWith( "VisualStudioCleaner.", StringComparison.OrdinalIgnoreCase ) );

            foreach( AssemblyName name in referenced )
            {
                yield return Assembly.Load( name );
            }
        }

        private void CoerceReferences()
        {
            // ReSharper disable UnusedVariable
            var c1 = new Cleaners.CoerceReference();
            var c2 = new Finders.CoerceReference();
            var c3 = new Workers.CoerceReference();
            // ReSharper restore UnusedVariable
        }

        public void Dispose()
        {
            if( _container != null )
            {
                _container.Dispose();
            }
        }
    }
}
