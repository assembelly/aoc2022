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
            int index = 0;
            int sequenceLength = 14;

            for (; new HashSet<char>(datastream.Substring(index, sequenceLength)).Count < sequenceLength; index++) ;            

            Console.WriteLine(index + sequenceLength);
        }
    }
}
