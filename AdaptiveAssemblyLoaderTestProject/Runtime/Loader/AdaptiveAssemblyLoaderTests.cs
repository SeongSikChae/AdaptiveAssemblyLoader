using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plugin;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace System.Runtime.Loader.Tests
{
    public class TraceWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string? value)
        {
            Trace.WriteLine(value);
        }
    }

    [TestClass]
    public class AdaptiveAssemblyLoaderTests
    {
        [TestMethod]
        public void AdaptiveAssemblyLoaderTest()
        {
            Console.SetOut(new TraceWriter());

            using (PluginManager manager = new PluginManager())
            {
                manager.Load();
                manager.Unload();
            }
        }
    }

    public sealed class PluginManager : IDisposable
    {
        private AdaptiveAssemblyLoader? adaptiveAssemblyLoader = new AdaptiveAssemblyLoader("Plugins");
        private HashSet<IPlugin>? plugins = new HashSet<IPlugin>();
        private bool disposedValue;

        public void Load()
        {
            {
                Assembly? assembly = adaptiveAssemblyLoader?.LoadAssembly(new AssemblyName("TestPlugin1"));
                Type? t = assembly?.GetType("Plugin.Plugin1");
                if (t != null)
                {
                    IPlugin? plugin = Activator.CreateInstance(t) as IPlugin;
                    if (plugin != null)
                    {
                        plugins?.Add(plugin);
                        plugin.Load();
                    }
                }
            }

            {
                Assembly? assembly = adaptiveAssemblyLoader?.LoadAssembly(new AssemblyName("TestPlugin2"));
                Type? t = assembly?.GetType("Plugin.Plugin2");
                if (t != null)
                {
                    IPlugin? plugin = Activator.CreateInstance(t) as IPlugin;
                    if (plugin != null)
                    {
                        plugins?.Add(plugin);
                        plugin.Load();
                    }
                }
            }
        }

        public void Unload()
        {
            if (plugins != null)
            {
                foreach (IPlugin plugin in plugins)
                    plugin.Unload();
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    plugins?.Clear();
                    plugins = null;
                    if (adaptiveAssemblyLoader != null)
                        adaptiveAssemblyLoader.Dispose();
                    adaptiveAssemblyLoader = null;
                }

                disposedValue = true;
            }
        }

        // ~PluginManager()
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