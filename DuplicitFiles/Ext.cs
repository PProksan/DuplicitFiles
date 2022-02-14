using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicitFiles
{
    internal static class Ext
    {
        internal static string ToFriendlySizeString(this long bytes)
        {
            double tmp = bytes;
            int exp = 0;
            while (tmp >= 1024)
            {
                tmp /= 1024;
                exp++;
            }

            switch (exp)
            {
                case 0: return $"{tmp:0.###} bytes";
                case 1: return $"{tmp:0.0##} KB";
                case 2: return $"{tmp:0.0##} MB";
                case 3: return $"{tmp:0.0##} GB";
                case 4: return $"{tmp:0.0##} TB";
                case 5: return $"{tmp:0.0##} PB";

                default: return $"{tmp}";
            }
        }
    }
}
