/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static Semaphore semaphore = new Semaphore(0, 10);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            var initialState = 10;

            #region implementation a

            var initialThread = new Thread(DecrementAndPrintWithThread);
            initialThread.Start(initialState);
            initialThread.Join();

            #endregion
            #region implementation b

            semaphore.Release(1);
            ThreadPool.QueueUserWorkItem(DecrementAndPrintWithThreadPool, initialState);
            semaphore.WaitOne();

            #endregion

            Console.ReadLine();
        }

        #region implementation a

        static void DecrementAndPrintWithThread(object state)
        {
            var currentState = (int)state;
            Console.WriteLine(currentState);

            if (currentState > 1)
            {
                var newState = currentState - 1;
                var thread = new Thread(DecrementAndPrintWithThread);
                thread.Start(newState);
                thread.Join();
            }
        }

        #endregion
        #region implementation b

        static void DecrementAndPrintWithThreadPool(object state)
        {
            var currentState = (int)state;
            Console.WriteLine(currentState);

            if (currentState > 1)
            {
                var newState = currentState - 1;
                ThreadPool.QueueUserWorkItem(DecrementAndPrintWithThreadPool, newState);
                semaphore.WaitOne();
            }
        }

        #endregion
    }
}
