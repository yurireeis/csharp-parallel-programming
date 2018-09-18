using System;
using System.Threading;
using System.Threading.Tasks;

namespace InitializeAndCancellingTasks
{

    class Program
    {
        /*
          CancellationTokenSource is to either manually cancel an operation or cancel giving a timeout
        */
        private static CancellationTokenSource Cts = new CancellationTokenSource();
        // tasks will be cancelled by the cancelation token
        private static CancellationToken Token = Cts.Token;
        static void Main(string[] args)
        {
            var name = new Task<string>(() => {
                Token.ThrowIfCancellationRequested();
                return "Yuri Reis";
            }, Token);

            Console.WriteLine($"Meu nome é: {name.Result}");
        }
    }
}
