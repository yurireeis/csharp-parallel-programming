using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Coding.Exercise
{
    class Program
    {
        static void Main(string[] args)
        {
            string sentence = "Mark Farragher";
            Console.WriteLine($"{Exercise.PigLatinString(sentence)}");
        }
    }
    
    public class Exercise
    {
        private static string[] Map(string sentence) => sentence.Split();
        private static string[] Process(string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                int index = i;
                Task.Factory.StartNew(() => words[index] = PigLatin(words[index]),
                    TaskCreationOptions.AttachedToParent |
                    TaskCreationOptions.LongRunning
                );
            }
                
            return words;
        }
        private static string Reduce(string[] words)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string word in words) { sb.Append($"{word} "); }
            return sb.ToString();
        }

        private static string PigLatin(string word)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{word}{word[0]}ay").Remove(0,1);
            return sb.ToString().ToLower();
        }

        public static string PigLatinString(string sentence)
        {
            var tasks = Task<string[]>.Factory.StartNew(() => Map(sentence))
                .ContinueWith<string[]>(m => Process(m.Result))
                .ContinueWith<string>(p => Reduce(p.Result));
            return tasks.Result;
        }
    }
}
