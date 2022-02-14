using System;
using System.Collections.Generic;

namespace DuplicitFiles
{
    internal class Data
    {
        private Data() { }

        private static readonly Data Instance = new();
        private Dictionary<long, List<string>> Dict { get; set; } = new();

        public static Data GetInstance() => Instance;

        public void Add(long key, string path)
        {
            if (Dict.ContainsKey(key))
            {
                if (!Dict[key].Contains(path))
                {
                    Dict[key].Add(path);
                }
            }
            else
            {
                Dict.Add(key, new List<string> { path });
            }
        }

        public void Add(Dictionary<long, List<string>> dict)
        {
            foreach (var kvp in dict)
            {
                foreach (var item in kvp.Value)
                {
                    Add(kvp.Key, item);
                }
            }
        }

        public Dictionary<long, List<string>> GetDict() => Instance.Dict;
    }
}
