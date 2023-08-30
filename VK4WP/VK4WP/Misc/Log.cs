using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace VK4WP
{
    public static class Log
    {

        public static void WriteLine(string fmt, params object[] args)
        {
            Debug.WriteLine(fmt, args);
        }

        public static void AssertWrite(bool cond, string fmt, params object[] args)
        {
            if (cond)
                Debug.WriteLine(fmt, args);
        }
    }
}
