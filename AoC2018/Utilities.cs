using System;
using System.Collections.Generic;
using System.Text;

namespace AoC2018
{
    class Utilities
    {
        public static string InputPath(int dayNo)
        {
            const string pathTemplate = @"C:\Projects\AoC2018\AoC2018\Day{0}\input.txt";
            return string.Format(pathTemplate, dayNo);
        }
    }
}
