using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGames.Logic
{
    public static class PlayerStatePathFactory
    {
        public static string[] createBluePlayerPath()
        {
            return Enumerable.Range(1, 56).Select(x => x.ToString())
                .Concat(Enumerable.Range(1, 6).Select(x => "b" + x)).ToArray();
        }

        public static string[] createRedPlayerPath()
        {
            return Enumerable.Range(15, 56 - 14).Select(x => x.ToString())
                .Concat(Enumerable.Range(1, 14).Select(x => x.ToString()))
                .Concat(Enumerable.Range(1, 6).Select(x => "r" + x)).ToArray();
        }

        public static string[] createYellowPlayerPath()
        {
            return Enumerable.Range(29, 56 - 28).Select(x => x.ToString())
                .Concat(Enumerable.Range(1, 28).Select(x => x.ToString()))
                .Concat(Enumerable.Range(1, 6).Select(x => "y" + x)).ToArray();
        }

        public static string[] createGreenPlayerPath()
        {
            return Enumerable.Range(43, 56 - 42).Select(x => x.ToString())
                .Concat(Enumerable.Range(1, 42).Select(x => x.ToString()))
                .Concat(Enumerable.Range(1, 6).Select(x => "g" + x)).ToArray();
        }
    }
}
