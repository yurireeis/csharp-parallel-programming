using System;
using System.Threading;

namespace RaceConditions
{
    class Program
    {
        public static int i = 0;
        public static void DoWork()
        {
            for (int i = 0; i < 5; i++) { Console.Write("*"); }
        }
        static void Main(string[] args)
        {
            Thread t1 = new Thread(DoWork);
            t1.Start();
            DoWork();
            // t1 thread and DoWork will fight for the same value (i)
            // hence (por isso),the start will be not output ten times as expected
        }
    }
}
