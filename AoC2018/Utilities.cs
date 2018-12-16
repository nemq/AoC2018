using System;
using System.Collections.Generic;
using System.Text;

namespace AoC2018
{
    public class Utilities
    {
        public static string InputPath(int dayNo, bool test = false)
        {
            const string pathTemplate = @"C:\Projects\AoC2018\AoC2018\Day{0}\{1}";
            return string.Format(pathTemplate, dayNo, test? "test.txt" : "input.txt");
        }
    }
}
