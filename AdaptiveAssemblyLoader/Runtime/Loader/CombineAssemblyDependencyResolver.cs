namespace System.Runtime.Loader
{
    using Reflection;

    internal sealed class CombineAssemblyDependencyResolver
    {
        public CombineAssemblyDependencyResolver(DirectoryInfo dependencyDirectoryInfo)
        {
            FileInfo[] assemblies = dependencyDirectoryInfo.GetFiles("*.dll", SearchOption.AllDirectories);
            foreach (FileInfo assembly in assemblies)
                resolvers.Add(new AssemblyDependencyResolver(assembly.FullName));
        }

        private readonly HashSet<AssemblyDependencyResolver> resolvers = new HashSet<AssemblyDependencyResolver>();

        public string? ResolveAssemblyToPath(AssemblyName assemblyName)
        {
            foreach (AssemblyDependencyResolver resolver in resolvers)
            {
                string? assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
                if (assemblyPath != null)
                    return assemblyPath;
            }
            return null;
        }

        public string? ResolveUnmanagedDllToPath(string unmanagedDllName)
        {
            foreach (AssemblyDependencyResolver resolver in resolvers)
            {
                string? assemblyPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
                if (assemblyPath != null)
                    return assemblyPath;
            }
            return null;
        }
    }
}
