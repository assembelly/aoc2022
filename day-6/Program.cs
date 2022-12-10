using System;
using System.IO;
using System.Collections.Generic;

namespace day_6_solution
{
    class Program
    {
        static void Main(string[] args)
        {
            string datastream = File.ReadAllText(@"..\..\..\..\input");
            int sequenceLengt = 14;
                        
            string startingWindow = datastream.Substring(0, sequenceLengt);
            Queue<char> slidingWindow = new Queue<char>(startingWindow);   
                                 
            int uniqueCharCount = new HashSet<char>(slidingWindow).Count;

            int index = sequenceLengt;

            while (uniqueCharCount < sequenceLengt)
            {
                slidingWindow.Dequeue();
                slidingWindow.Enqueue(datastream[index]);

                uniqueCharCount = new HashSet<char>(slidingWindow).Count;

                index++;
            }

            Console.WriteLine(index);
        }
    }
}
