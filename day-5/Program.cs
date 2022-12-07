using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace day_5_solution
{
    class MoveProcedure
    {
        public int Count { get; }
        public int SourceIndex { get; }
        public int DestinationIndex { get; }
      
        public MoveProcedure (string plaintextProcedure)
        {
            string[] temp = plaintextProcedure.Split(" ");

            Count = int.Parse(temp[1]);
            SourceIndex = int.Parse(temp[3]) - 1;
            DestinationIndex = int.Parse(temp[5]) - 1;
        }
    }
    class Cargo
    {
        Stack<char>[] Stacks;
        
        public Cargo(string fileLocation)
        {
            List<string> cargoLayout = extractCargoLayoutFromFile(fileLocation);

            int stackCount = (cargoLayout[0].Length + 1) / 4;
            initializeFields(stackCount);

            populateStacksWith(cargoLayout);
        }
        
        public string GetTopCrates()
        {
            string topCrates = "";                  

            foreach (Stack<char> stack in Stacks)
            {                
                topCrates += stack.Peek();
            }

            return topCrates;
        }

        public void MoveCratesOneByOne (MoveProcedure moveProcedure )
        {            
            for (int i = 0; i < moveProcedure.Count ; i++)
            {
                Stacks[moveProcedure.DestinationIndex].Push(Stacks[moveProcedure.SourceIndex].Pop());
            }
        }

        public void MoveMultipleCratesAtOnce (MoveProcedure moveProcedure)
        {
            Stack<char> buffer = new Stack<char>();
            
            for (int i = 0; i < moveProcedure.Count; i++)
            {
                buffer.Push(Stacks[moveProcedure.SourceIndex].Pop());
            }

            for (int i = 0; i < moveProcedure.Count; i++)
            {
                Stacks[moveProcedure.DestinationIndex].Push(buffer.Pop());
            }            
        }

        private List<string> extractCargoLayoutFromFile(string fileLocation)
        {
            IEnumerator<string> lines = File.ReadLines(fileLocation).GetEnumerator();
            List<string> cargoLayout = new List<string>();

            while (lines.MoveNext() && lines.Current.Length != 0)
            {
                cargoLayout.Add(lines.Current);
            }

            //remove last line with unnecessary data 
            cargoLayout.RemoveAt(cargoLayout.Count - 1);

            return cargoLayout;
        }

        private void initializeFields(int stackCount)
        {
            Stacks = new Stack<char>[stackCount];

            for (int i = 0; i < stackCount; i++)
            {
                Stacks[i] = new Stack<char>();
            }
        }

        
        private void populateStacksWith(List<string> cargoLayout)
        {
            for (int i = cargoLayout.Count - 1; i >= 0; i--)
            {
                for (int j = 1; j < cargoLayout[i].Length; j += 4)
                {
                    if (char.IsLetter(cargoLayout[i][j]))
                    {
                        Stacks[j/4].Push(cargoLayout[i][j]);
                    }
                }
            }
        }
    }
    

    class Program
    {
        static void Main(string[] args)
        {
            Cargo cargo = new Cargo(@"..\..\..\..\input");

            IEnumerator<string> lines = File.ReadLines(@"..\..\..\..\input").GetEnumerator();

            //move line iterator to start of movement procedures
            while (lines.MoveNext() && lines.Current.Length != 0) ;

            while (lines.MoveNext())
            {
                cargo.MoveMultipleCratesAtOnce(new MoveProcedure(lines.Current));
            }

            Console.WriteLine(cargo.GetTopCrates());
        }
    }
}
