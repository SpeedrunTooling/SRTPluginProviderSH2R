using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SRTPluginProviderSH2R
{
    /// <summary>
    /// SHA256 hashes for the SH2R game executables.
    /// https://steamdb.info/app/2124490/ / https://steamdb.info/depot/2124491/
    /// </summary>
    public static class GameHashes
    {
        private static readonly byte[] sh2r_20241021_085939 = new byte[32] { 0xED, 0xBD, 0x03, 0xCE, 0xB5, 0x82, 0xE4, 0x13, 0xD7, 0x25, 0xAB, 0x06, 0x03, 0xB8, 0x43, 0x45, 0x4F, 0x53, 0x56, 0xB2, 0xC2, 0xE1, 0x04, 0x64, 0xF3, 0xD9, 0xB7, 0x06, 0xC4, 0xCE, 0xCE, 0x8D };
        private static readonly byte[] sh2r_20241006_071402 = new byte[32] { 0x02, 0x21, 0x59, 0xD6, 0x34, 0x4F, 0x98, 0xFF, 0x04, 0xEF, 0xA4, 0x10, 0x86, 0x67, 0x1A, 0x43, 0xA4, 0x60, 0xD7, 0x1B, 0x91, 0xF7, 0xE6, 0x42, 0x02, 0x8E, 0xF2, 0xBC, 0x59, 0x2C, 0x08, 0x21 };

        private static void OutputVersionString(byte[] cs)
        {
            StringBuilder sb = new StringBuilder("private static readonly byte[] sh2r_00000000_000000 = new byte[32] { ");

            for (int i = 0; i < cs.Length; i++)
            {
                sb.AppendFormat("0x{0:X2}", cs[i]);

                if (i < cs.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append(" };");
            Console.WriteLine("Please contact Squirrelies with the version.log");
            // write output to file
            string filename = "version.log";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine(sb.ToString());
            }
        }

        public static GameVersion DetectVersion(string filePath)
        {
            byte[] checksum;
            using (SHA256 hashFunc = SHA256.Create())
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                checksum = hashFunc.ComputeHash(fs);

            if (checksum.SequenceEqual(sh2r_20241021_085939))
            {
                Console.WriteLine($"Game Detected! Version: {nameof(GameVersion.SH2R_20241021_085939)}");
                return GameVersion.SH2R_20241021_085939;
            }
            else if (checksum.SequenceEqual(sh2r_20241006_071402))
            {
                Console.WriteLine($"Game Detected! Version: {nameof(GameVersion.SH2R_20241006_071402)}");
                return GameVersion.SH2R_20241006_071402;
            }
            else
            {
                Console.WriteLine("Unknown Version");
                OutputVersionString(checksum);
                return GameVersion.Unknown;
            }
                
        }
    }
}
