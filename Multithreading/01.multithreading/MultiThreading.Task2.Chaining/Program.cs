/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        const int ArrayLength = 10;
        static readonly Random Random = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Task.Run(GenerateRandomArray)
                .ContinueWith(MultiplyArray)
                .ContinueWith(SortArray)
                .ContinueWith(CalculateAverage);

            Console.ReadLine();
        }

        static int[] GenerateRandomArray()
        {
            var arr = Enumerable.Range(0, ArrayLength)
                    .Select(i => Random.Next(0, 100))
                    .ToArray();

            PrintCollection("Generated Array:", arr);
            return arr;
        }

        static int[] MultiplyArray(Task<int[]> previousTask)
        {
            var multiplier = Random.Next(1, 100);

            var multipliedArr = previousTask.Result
                .Select(i => i * multiplier)
                .ToArray();

            PrintCollection($"Array multiplied with {multiplier}:", multipliedArr);
            return multipliedArr;
        }

        static int[] SortArray(Task<int[]> previousTask)
        {
            Array.Sort(previousTask.Result);

            PrintCollection("Sorted Array:", previousTask.Result);
            return previousTask.Result;
        }

        static double CalculateAverage(Task<int[]> previousTask)
        {
            var average = previousTask.Result.Average();
            Console.WriteLine("Average value: " + average);
            return average;
        }

        static void PrintCollection(string message, int[] values)
        {
            Console.WriteLine(message);
            Console.WriteLine(string.Join(", ", values));
        }
    }
}
