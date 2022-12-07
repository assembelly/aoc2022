using System;
using System.IO;
using System.Collections.Generic;

namespace day_1_puzzle_1
{
    class Program
    {
        static void Main(string[] args)
        {
            SortedSet<int> max_calorie_count = new SortedSet<int>();
            //initalise values 
            max_calorie_count.Add(int.MinValue);
            max_calorie_count.Add(int.MinValue + 1);
            max_calorie_count.Add(int.MinValue + 2);

            int current_elf_calorie_count;

            foreach (string elf in System.IO.File.ReadAllText(@".\day-1-input").Split("\n\n"))
            {
                current_elf_calorie_count = 0;

                foreach (string item in elf.Split("\n"))
                {
                    current_elf_calorie_count += int.Parse(item);
                }

                if (current_elf_calorie_count > max_calorie_count.Min)
                {
                    max_calorie_count.Remove(max_calorie_count.Min);
                    max_calorie_count.Add(current_elf_calorie_count);
                }
            }


            int total_calorie_count_top_elves = 0;

            foreach (int calorie_count in max_calorie_count)
            {
                total_calorie_count_top_elves += calorie_count;
            }

            Console.WriteLine(total_calorie_count_top_elves);
        }
    }
}
