using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DuplicitFiles
{
    public class Engine
    {
        public static Dictionary<long, List<string>> CheckDuplicity(string folderPath, string searchPattern = "*.*", bool allDirectories = true)
        {
            if(folderPath == null || !Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"Složka [{folderPath}] nenalezena");
            }

            var files = new DirectoryInfo(folderPath).GetFiles(searchPattern, allDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            var group = files.GroupBy(f => f.Length);

            return group
                .Where(g => g.Count() > 1)
                .ToArray()
                .ToDictionary(x => x.Key, x => x.Select(s => s.FullName).ToList());

        }
    }
}
