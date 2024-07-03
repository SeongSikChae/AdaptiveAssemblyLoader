namespace System.Runtime.Loader
{
    using Reflection;

    public class AdaptiveAssemblyLoader : IDisposable
    {
        public AdaptiveAssemblyLoader(string dependencyAssembliesPath)
        {
            weakReference = new WeakReference<AdaptiveAssemblyLoadContext?>(new AdaptiveAssemblyLoadContext(dependencyAssembliesPath));
        }

        private WeakReference<AdaptiveAssemblyLoadContext?> weakReference;
        private bool disposedValue;

        public IEnumerable<Assembly> GetAssemblies()
        {
            if (disposedValue || !weakReference.TryGetTarget(out AdaptiveAssemblyLoadContext? context))
                throw new ObjectDisposedException(nameof(context));
            return context.GetAssemblies();
        }

        public Assembly? LoadAssembly(AssemblyName assemblyName)
        {
            if (disposedValue || !weakReference.TryGetTarget(out AdaptiveAssemblyLoadContext? context))
                throw new ObjectDisposedException(nameof(context));
            return context.LoadFromAssemblyName(assemblyName);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (weakReference.TryGetTarget(out AdaptiveAssemblyLoadContext? context))
                    {
                        context.Unload();
                        weakReference.SetTarget(null);
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
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
