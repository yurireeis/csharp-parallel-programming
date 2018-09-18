using System;
using System.Threading;

namespace MultiThreadCodeFundamentals
{
    class Program
    {
        private const int REPETITIONS = 10;
        public static void DoWork()
        {
            for (int i = 0; i < REPETITIONS; i++) { Console.WriteLine("B"); }
        }
        
        public static void DoWork(char value)
        {
            for (int i = 0; i < REPETITIONS; i++) { Console.WriteLine(value); }
        }
        static void Main(string[] args)
        {
            // main program thread
            // start new thread (three forms)
            // Thread t1 = new Thread(new ThreadStart(DoWork)); [Formal form]
            // Thread t1 = new Thread(() => DoWork()); [Anonymous function]
            Thread t1 = new Thread(DoWork); // simplified form
            t1.Start();

            // foreground tasks Don't allow the program to end / Background tasks keeps alive when foreground threads is running
            Thread t2 = new Thread(() => {
              try
              {
                  DoWork('C');
                  Console.ReadKey();
              }
              catch (System.InvalidOperationException ioe)
              {
                  Console.WriteLine("There's no more foreground tasks alive!");
              }
            });
            t2.IsBackground = true;
            t2.Start();

            // continue simultaneous work
            for (int i = 0; i < REPETITIONS; i++) { Console.WriteLine("A"); }
        }
    }
}
