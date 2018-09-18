using System;
using System.Threading;

namespace ThreadLocking
{
    class Program
    {
        /*
          - lock: synthatic sugar for a Monitor.Enter / Monitor.Exit
          - Monitor has a TryEnter method that supports a lock timeout value
          - Lock requires a type synchronization object. A unique private 
          object field is recommended
        */
        public static int value1 { get; set; }
        public static int value2 { get; set; }

        /*
          syncObject properties must be private
        */
        private static object syncObj = new object();
        static void Main(string[] args)
        {
            // starting two threads
            Thread t1 = new Thread(DoWorkWithSyntaxSugar);
            Thread t2 = new Thread(DoWorkWithMonitorTryEnterSyntax);
            t1.Start();
            t2.Start();
        }

        private static void DoWorkWithSyntaxSugar()
        {            
            // this critical section above is thread safe (multiple threads race conditions guarded)
            /*
              lock is a syntax sugar, that implements Monitor.TryEnter when it's built
              implements Monitor.TryEnter directly when you want to setup the timeout 
              of waiting time to access a critical section (see DoWork2)
            */
            lock (syncObj) {
              if (value2 > 0)
              {
                  /*
                    DoTheDivision has Lock too. So, We have here a nested lock
                    - assure that nested lock is controller by the same sync object
                  */
                  DoTheDivision();
                  value2 = 0;
              }
            }
        }

        public static void DoTheDivision() {
            lock (syncObj)
            {
                if (value2 > 0)
                {
                    Console.WriteLine(value1 / value2);
                }    
            }
        }

        public static void DoWorkWithMonitorTryEnterSyntax()
        {
            /*
              the advantage of this approach is that you can set for example, the time
              of timeout to expect
            */
            bool lockTaken = false;

            try
            {
                Monitor.TryEnter(syncObj, TimeSpan.FromMilliseconds(100), ref lockTaken);
                if (value2 > 0)
                {
                  /*
                    DoTheDivision has Lock too. So, We have here a nested lock
                    - assure that nested lock is controller by the same sync object
                  */
                    DoTheDivision();
                }
            }
            finally { if (lockTaken) { Monitor.Exit(syncObj); } }
        }
    }
}
