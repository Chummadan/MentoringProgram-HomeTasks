/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            #region a, b, c implementations

            var antecedentCaseABC = Task.Run(() =>
            {
                Console.WriteLine("Antecedent task started.");
                throw new Exception("Parent Task failed.");
            });

            antecedentCaseABC.ContinueWith(task =>
            {
                Console.WriteLine("Executed regardless of the result of the parent task.");
            });

            antecedentCaseABC.ContinueWith(task =>
            {
                if (!task.IsCompletedSuccessfully)
                    Console.WriteLine("Executed when the parent task finished without success.");
            });

            antecedentCaseABC.ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Console.WriteLine("Executed when the parent task finished with fail and reused parent task thread.\n" +
                        $"Parent ex.message: {antecedentCaseABC.Exception.Message}");
            }, TaskContinuationOptions.ExecuteSynchronously);

            #endregion

            #region d implementation

            var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var antecedentCaseD = Task.Run(() =>
            {
                Console.WriteLine("Antecedent task started.");
                cancellationToken.ThrowIfCancellationRequested();
            }, cancellationToken);

            tokenSource.Cancel();

            antecedentCaseD.ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Console.WriteLine("Executed outside of the thread pool when the parent task was canceled.\n" +
                        $"IsThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread}");
                }
            }, TaskContinuationOptions.LongRunning);

            #endregion

            Console.ReadLine();
        }
    }
}
