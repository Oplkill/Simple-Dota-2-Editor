using System.Collections.Generic;
using System.IO;
using SteamDatabase.ValvePak;

namespace SomeUtils
{
    public static class DotaResourceManager
    {
        public static void CreateCacheIcons(string dotaPathPak01DirVpk, string path)
        {
            if (!File.Exists(dotaPathPak01DirVpk))
                return;
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            SteamDatabase.ValvePak.Package package = new Package();
            package.Read(dotaPathPak01DirVpk);

            var imagesList = new List<PackageEntry>();

            foreach (var file in package.Entries["png"])
            {
                if (file.DirectoryName.Contains("resource/flash3/images") && !file.DirectoryName.Contains("hud_skins"))
                {
                    imagesList.Add(file);
                }
            }

            foreach (var img in imagesList)
            {
                byte[] bytes;
                package.ReadEntry(img, out bytes);
                dirInfo.CreateSubdirectory(img.DirectoryName);
                var file = File.Create("DotaCache\\" + img.GetFullPath());
                file.Write(bytes, 0, bytes.Length);
                file.Close();
            }
        }
    }
}