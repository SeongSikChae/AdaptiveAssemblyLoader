namespace System.Runtime.Loader
{
    using Reflection;

    internal sealed class CombineAssemblyDependencyResolver
    {
        public CombineAssemblyDependencyResolver(DirectoryInfo dependencyDirectoryInfo)
        {
            FileInfo[] assemblies = dependencyDirectoryInfo.GetFiles("*.dll", SearchOption.AllDirectories);
            foreach (FileInfo assembly in assemblies)
            {
                if (assembly.Directory is not null)
                {
                    if (!resolvers.ContainsKey(assembly.Directory.FullName))
                        resolvers.Add(assembly.Directory.FullName, new AssemblyDependencyResolver(assembly.FullName));
                }
            }
        }

        private readonly Dictionary<string, AssemblyDependencyResolver> resolvers = new Dictionary<string, AssemblyDependencyResolver>();

        public string? ResolveAssemblyToPath(AssemblyName assemblyName)
        {
            foreach (AssemblyDependencyResolver resolver in resolvers.Values)
            {
                string? assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
                if (assemblyPath != null)
                    return assemblyPath;
            }
            return null;
        }

        public IEnumerable<string> ResolveAssemblyToPath()
        {
            HashSet<string> resolveAssemblyToPathSet = new HashSet<string>();

            foreach (AssemblyDependencyResolver resolver in resolvers.Values)
            {
                Type t = resolver.GetType();
                FieldInfo? fieldInfo = t.GetField("_assemblyPaths", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fieldInfo is not null)
                {
                    Dictionary<string, string>? assemblyPaths = fieldInfo.GetValue(resolver) as Dictionary<string, string>;
                    if (assemblyPaths is not null)
                    {
                        foreach (string path in assemblyPaths.Values)
                        {
                            if (resolveAssemblyToPathSet.Add(path))
                                yield return path;
                        }
                    }
                }
            }
        }

        public string? ResolveUnmanagedDllToPath(string unmanagedDllName)
        {
            foreach (AssemblyDependencyResolver resolver in resolvers.Values)
            {
                string? assemblyPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
                if (assemblyPath != null)
                    return assemblyPath;
            }
            return null;
        }
    }
}
