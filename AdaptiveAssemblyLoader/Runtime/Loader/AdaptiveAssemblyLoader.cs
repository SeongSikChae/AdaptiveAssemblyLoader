namespace System.Runtime.Loader
{
    using Reflection;

    public class AdaptiveAssemblyLoader : IDisposable
    {
        public AdaptiveAssemblyLoader(string dependencyAssembliesPath)
        {
            adaptiveAssemblyLoadContext = new AdaptiveAssemblyLoadContext(dependencyAssembliesPath);
            weakReference = new WeakReference(adaptiveAssemblyLoadContext);
        }

        private AdaptiveAssemblyLoadContext? adaptiveAssemblyLoadContext;
        private readonly WeakReference weakReference;
        private bool disposedValue;

        public IEnumerable<Assembly> GetAssemblies()
        {
            if (disposedValue || adaptiveAssemblyLoadContext == null)
                throw new ObjectDisposedException(nameof(adaptiveAssemblyLoadContext));
            return adaptiveAssemblyLoadContext.Assemblies;
        }

        public Assembly? LoadAssembly(AssemblyName assemblyName)
        {
            if (disposedValue || adaptiveAssemblyLoadContext == null)
                throw new ObjectDisposedException(nameof(adaptiveAssemblyLoadContext));
            return adaptiveAssemblyLoadContext.LoadFromAssemblyName(assemblyName);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    adaptiveAssemblyLoadContext?.Unload();
                    adaptiveAssemblyLoadContext = null;
                    while (weakReference.IsAlive)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }

                disposedValue = true;
            }
        }

        // ~AdaptiveAssemblyLoader()
        // {
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
