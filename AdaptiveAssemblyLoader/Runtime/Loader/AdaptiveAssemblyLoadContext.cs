namespace System.Runtime.Loader
{
    using Reflection;

    internal sealed class AdaptiveAssemblyLoadContext : AssemblyLoadContext
    {
        public AdaptiveAssemblyLoadContext(string dependencyAssembliesPath) : base(isCollectible: true)
        {
            DirectoryInfo dependencyDirectoryInfo = new DirectoryInfo(dependencyAssembliesPath);
            if (!dependencyDirectoryInfo.Exists)
                dependencyDirectoryInfo.Create();
            resolver = new CombineAssemblyDependencyResolver(dependencyDirectoryInfo);
        }

        private readonly CombineAssemblyDependencyResolver resolver;

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            string? dependencyAssemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
            if (dependencyAssemblyPath != null)
                return LoadFromAssemblyPath(dependencyAssemblyPath);
            return null;
        }
    }
}
