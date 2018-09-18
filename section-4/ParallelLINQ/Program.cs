using System;
using System.Linq;

namespace ParallelLINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
              PLINQ methods
              AsParallel(from LINQ to PLINQ)
              AsSequential(from PLINQ to LINQ) 
            */
            string sentence = "the quick brown fox jumper over the lazy dog";
            var words = sentence.Split().Select((word) => new string(word.Reverse().ToArray()));
            Console.WriteLine("Ordinary LINQ call: " + string.Join(" ", words));

            /* working in Parallel */
            words = sentence.Split()
                      .AsParallel()
                      .AsOrdered()
                      .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                      .Select((word) => new string(word.Reverse().ToArray()));
            Console.WriteLine("As parallel result: " + string.Join(" ", words));

            /*
              as default, AsParallel methods decides if it's run in parallel or not
              force paralellism is possible
              ex.: .WithExecutionMode(ParallelExecutionMode.ForceParalelism)
              When you preserve AsUnordered with AsParallel, probably you will
              get a better performance
            */
        }
    }
}
