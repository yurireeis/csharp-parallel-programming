using System;
using System.Threading;

namespace ThreadSyncWithAutoResetEvents
{
    class Program
    {
        public static int Result = 0;
        private static object LockHandle = new object();

        // this AutoResetEvent is one comunication channel (from work Thread to main Thread)
        public static EventWaitHandle ReadyForResult = new AutoResetEvent(false);

        // this AutoResetEvent is another comunication chanell (from main to work Thread)
        public static EventWaitHandle DoneWithWritingResult = new AutoResetEvent(false);
        static void Main(string[] args)
        {
            // start the thread
            Thread t1 = new Thread(DoWork);
            t1.Start();

            // collect result every 10 milliseconds
            for (int i = 0; i < 100; i++)
            {
                // this method tells if main thread is ready to receive the result
                ReadyForResult.Set();

                // this method is waiting if Work Thread has Done with this writing shared variabe job
                DoneWithWritingResult.WaitOne();

                lock (LockHandle) { Console.WriteLine(Result); }

                // simulate other work
                Thread.Sleep(10);
            }
        }
        public static void DoWork()
        {
            while(true)
            {
                // shared field for work result
                int i = Result;

                // wait until main loop is ready to receive result
                // if it's not ready to receive, the Thread will be suspended
                ReadyForResult.WaitOne();

                // return result
                lock (LockHandle) { Result = i + 1; }
                DoneWithWritingResult.Set();
            }
        }
    }
}
