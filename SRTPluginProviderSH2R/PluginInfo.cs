using SRTPluginBase;
using System;

namespace SRTPluginProviderSH2R
{
    internal class PluginInfo : IPluginInfo
    {
        public string Name => "Game Memory Provider (Silent Hill 2 (2024))";

        public string Description => "A game memory provider plugin for Silent Hill 2 (2024).";

        public string Author => "Squirrelies";

        public Uri MoreInfoURL => new Uri("https://github.com/SpeedrunTooling/SRTPluginProviderSH2R");

        public int VersionMajor => assemblyVersion.Major;

        public int VersionMinor => assemblyVersion.Minor;

        public int VersionBuild => assemblyVersion.Build;

        public int VersionRevision => assemblyVersion.Revision;

        private Version assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    }
}
