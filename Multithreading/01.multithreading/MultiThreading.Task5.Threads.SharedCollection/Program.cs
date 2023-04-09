/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        const int ElementsCount = 10;
        static readonly object lockObject = new object();
        static List<int> sharedCollection = new List<int>();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            Task.Run(AddElements);
            Task.Run(PrintElements);

            Console.ReadLine();
        }

        static void AddElements()
        {
            for (int i = 0; i < ElementsCount; i++)
            {
                lock (lockObject)
                {
                    sharedCollection.Add(i);
                    Console.WriteLine($"Added element {i} to the collection.");
                    Monitor.Pulse(lockObject);
                }
                Thread.Sleep(1000);
            }
        }

        static void PrintElements()
        {
            while (sharedCollection.Count < ElementsCount)
            {
                lock (lockObject)
                {
                    Monitor.Wait(lockObject);
                    Console.WriteLine("Elements in the collection:");
                    Console.WriteLine(string.Join(", ", sharedCollection) + "\n");
                }
            }
        }
    }
}
