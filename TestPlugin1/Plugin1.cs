namespace Plugin
{
    public sealed class Plugin1 : IPlugin
    {
        public void Load()
        {
            Console.WriteLine($"{nameof(Plugin1)} Loaded");
        }

        public void Unload()
        {
            Console.WriteLine($"{nameof(Plugin1)} Unoaded");
        }
    }
}
