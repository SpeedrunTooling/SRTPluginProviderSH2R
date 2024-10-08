namespace SRTPluginProviderSH2R
{
    public interface IGameMemorySH2R
    {
        string GameName { get; }

        string VersionInfo { get; }

        float PlayerHP { get; }
    }
}
