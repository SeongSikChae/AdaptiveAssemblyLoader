namespace Plugin
{
    public sealed class Plugin2 : IPlugin
    {
        public void Load()
        {
            Console.WriteLine($"{nameof(Plugin2)} Loaded");
        }

        public void Unload()
        {
            Console.WriteLine($"{nameof(Plugin2)} Unoaded");
        }
    }
}
