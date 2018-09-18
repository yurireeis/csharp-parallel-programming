using System;
using System.Threading.Tasks;
using System.Threading;

namespace WorkingWithTasks
{
    class Program
    {
        /*
          with Task, There's no need to use AUTHORITATIVE EVENTS and
          SYNCRONIZATION VARIABLES for critical sections
         */
        static void Main(string[] args)
        {
            /*
              start task
              - create the work to be done with a lambda expression
              - access the result property when you need the result
              - this task is marked as string because will return a string
            */
            var task = Task<string>.Factory.StartNew(() => {
                Thread.Sleep(2000);
                return "Yuri Reis";
            });

            // use result
            Console.WriteLine($"Your name is: {task.Result}");

            /*
              - To run a task that don't return any data, use data without type especification
              - if you have a LongRunningTask (i.e.: app that interact with another app),
              you need to provide the parameter LongRunningTask
              - When you receive a inner task exception, you will receive a AggregateException.
              - AggregateException has the real Exception in InnerExceptions property
              - Tasks propagate exceptions from the Task inside
            */
            var task2 = Task.Factory.StartNew(() => {
                Console.WriteLine($"Is background thread? {Thread.CurrentThread.IsBackground}");
                Console.WriteLine($"This threadpool thread? {Thread.CurrentThread.IsThreadPoolThread}");
                throw new InvalidOperationException("Something went wrong");
            }, TaskCreationOptions.LongRunning);

            /*
              wait for task to complete
              - it's a method to turn the task in a foreground Thread
            */
            try
            {
                task2.Wait();
            }
            catch (System.AggregateException)
            {
                Console.WriteLine("Invalid operation exception");
            }
        }
    }
}
