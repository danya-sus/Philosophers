using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Philosophers
{
    public static class ConsoleHelper
    {
        public static object LockObject = new object();
        public static void WriteToConsole(string info, string write)
        {
            lock (LockObject)
            {
                Console.WriteLine(info + " : " + write);
            }
        }
    }
}
