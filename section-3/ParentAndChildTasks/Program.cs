using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ParentAndChildTasks
{
    class Program
    {
        private static List<Task<string>> Tasks = new List<Task<string>>();
        private static string ReverseString(string s) {
          Thread.Sleep(1000);
          StringBuilder sb = new StringBuilder();
          for (int i = s.Length -1; i >= 0; i--) { sb.Append(s[i]); }
          return sb.ToString();
        }
        private static void ProcessSentence(string sentence)
        {
            foreach (string word in sentence.Split())
            {
                /*
                  when we create a task with AttachedToParent, we create a
                  parent task 
                */
                Tasks.Add(Task<string>.Factory.StartNew(() => 
                    $"{ReverseString(word)} ",
                    TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning
                ));
            }
        }

        static void Main(string[] args)
        {
            /*
              - child tasks throw exceptions and bubble to parent task
              - exceptions are aggregated and passed to parent task
            */
            string sentence = "the quick brown fox jumped over the lazy the quick brown fox jumped over the lazy";

            // run parent tasks to process sentence
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Task.Factory.StartNew(() => ProcessSentence(sentence)).Wait();
            sw.Stop();
            Console.WriteLine($"Total runtime: {sw.ElapsedMilliseconds} ms");

            // verify if is complete task status
            Tasks.ForEach(task => Console.WriteLine($"Task {task.GetHashCode()} is completed? {task.IsCompleted}"));

            // display results
            Console.Write("Result: ");
            foreach (Task<string> t in Tasks) { Console.Write(t.Result); }
        }
    }
}
