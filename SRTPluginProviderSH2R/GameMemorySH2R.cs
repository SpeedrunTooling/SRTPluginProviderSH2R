using System.Diagnostics;
using System.Reflection;
using SRTPluginProviderSH2R.Structs;

namespace SRTPluginProviderSH2R
{
    public struct GameMemorySH2R : IGameMemorySH2R
    {
        public string GameName => "SH2R";

        public string VersionInfo => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        internal const int ENEMY_ARRAY_SIZE = 32;

        public GameMemorySH2R()
        {
            enemyHP = new EnemyInfo[ENEMY_ARRAY_SIZE];
            for (var i = 0; i < ENEMY_ARRAY_SIZE; ++i)
                enemyHP[i] = new()
                {
                    isValid = false,
                    hp = 0f
                };
        }

        internal float playerHP;
        public float PlayerHP => playerHP;

        internal EnemyInfo[] enemyHP;
        public EnemyInfo[] EnemyHP => enemyHP;
    }
}
