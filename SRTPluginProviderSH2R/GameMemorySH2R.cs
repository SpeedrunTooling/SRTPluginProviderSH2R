using System.Diagnostics;
using System.Reflection;

namespace SRTPluginProviderSH2R
{
    public struct GameMemorySH2R : IGameMemorySH2R
    {
        public string GameName => "SH2R";

        public string VersionInfo => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        internal float playerHP;
        public float PlayerHP => playerHP;
    }
}
