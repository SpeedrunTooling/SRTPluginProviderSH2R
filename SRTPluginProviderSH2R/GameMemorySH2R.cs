using System.Diagnostics;
using System.Reflection;

namespace SRTPluginProviderSH2R
{
    public struct GameMemorySH2R : IGameMemorySH2R
    {
        public string GameName => "SH2R";

        public string VersionInfo => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        internal const int ENEMY_ARRAY_SIZE = 16;

        public GameMemorySH2R()
        {
            enemyHP = new float[ENEMY_ARRAY_SIZE];
        }

        internal float playerHP;
        public float PlayerHP => playerHP;

        internal float[] enemyHP;
        public float[] EnemyHP => enemyHP;
    }
}
