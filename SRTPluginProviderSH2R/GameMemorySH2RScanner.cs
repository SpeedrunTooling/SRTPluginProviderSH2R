﻿using ProcessMemory;
using System;
using System.Diagnostics;

namespace SRTPluginProviderSH2R
{
    internal class GameMemorySH2RScanner : IDisposable
    {
        // Variables
        private GameVersion gameVersion;
        private ProcessMemoryHandler memoryAccess;
        private GameMemorySH2R gameMemoryValues;
        public bool HasScanned;
        public bool ProcessRunning => memoryAccess != null && memoryAccess.ProcessRunning;
        public uint ProcessExitCode => (memoryAccess != null) ? memoryAccess.ProcessExitCode : 0;

        // Pointer Address Variables
        private int pointerPlayerStatus;
        private int pointerEnemyStatus;

        // Pointer Classes
        private IntPtr BaseAddress { get; set; }
        private MultilevelPointer PointerPlayerStatus { get; set; }
        private MultilevelPointer[] PointerEnemyStatus { get; set; }

        internal GameMemorySH2RScanner(Process process = null)
        {
            gameMemoryValues = new GameMemorySH2R();
            if (process != null)
                Initialize(process);
        }

        internal unsafe void Initialize(Process process)
        {
            if (process == null)
                return; // Do not continue if this is null.

            gameVersion = SelectPointerAddresses(GameHashes.DetectVersion(process.MainModule.FileName));
            if (gameVersion == GameVersion.Unknown)
                return; // Unknown version.

            int pid = GetProcessId(process).Value;
            memoryAccess = new ProcessMemoryHandler((uint)pid);
            if (ProcessRunning)
            {
                BaseAddress = process?.MainModule?.BaseAddress ?? IntPtr.Zero; // Bypass .NET's managed solution for getting this and attempt to get this info ourselves via PInvoke since some users are getting 299 PARTIAL COPY when they seemingly shouldn't.

                // Setup the pointers.
                PointerPlayerStatus = new MultilevelPointer(memoryAccess, (nint*)(BaseAddress + pointerPlayerStatus), 0x30, 0x2F0, 0x6C8, 0x1A8, 0x28, 0x6B8);

                PointerEnemyStatus = new MultilevelPointer[GameMemorySH2R.ENEMY_ARRAY_SIZE];
                for (int i = 0; i < GameMemorySH2R.ENEMY_ARRAY_SIZE; ++i)
                    PointerEnemyStatus[i] = new MultilevelPointer(memoryAccess, (nint*)(BaseAddress + pointerEnemyStatus), 0x268, 0x110, (i + 1) * 0x8, 0x90, 0x938);
            }
        }

        private GameVersion SelectPointerAddresses(GameVersion version)
        {
            switch (version)
            {
                // DX12
                case GameVersion.SH2R_20241006_071403:
                    {
                        pointerPlayerStatus = 0x08857AC0;
                        pointerEnemyStatus = 0x08876338;
                        return version;
                    }
                default:
                    return GameVersion.Unknown;
            }
        }

        internal void UpdatePointers()
        {
            PointerPlayerStatus.UpdatePointers();

            for (int i = 0; i < GameMemorySH2R.ENEMY_ARRAY_SIZE; ++i)
                PointerEnemyStatus[i].UpdatePointers();
        }

        internal unsafe IGameMemorySH2R Refresh()
        {
            PointerPlayerStatus.TryDerefFloat(0xF8, ref gameMemoryValues.playerHP);

            for (int i = 0; i < GameMemorySH2R.ENEMY_ARRAY_SIZE; ++i)
                if (!PointerEnemyStatus[i].TryDerefFloat(0xCC, ref gameMemoryValues.enemyHP[i]))
                    gameMemoryValues.enemyHP[i] = 0f;

            HasScanned = true;
            return gameMemoryValues;
        }

        private int? GetProcessId(Process process) => process?.Id;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (memoryAccess != null)
                        memoryAccess.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~REmake1Memory() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
