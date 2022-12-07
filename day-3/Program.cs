using System;
using System.IO;
using System.Collections.Generic;


namespace day_3_solution
{
    class Program
    {
        static char GetMisplacedItemType (string rucksack)
        {
            char[] misplacedItemType = new char[1];

            int compartmentLength = (rucksack.Length / 2);

            string firstComparment = rucksack.Substring(0, compartmentLength);
            string secondCompartment = rucksack.Substring(compartmentLength);

            HashSet<char> firstCompartmentTypes = new HashSet<char>(firstComparment);
            HashSet<char> secondCompartmentTypes = new HashSet<char>(secondCompartment);

            //firstCompartmentTypes contains only the overlaping item type after the following line
            firstCompartmentTypes.IntersectWith(secondCompartment);            
            firstCompartmentTypes.CopyTo(misplacedItemType);

            return misplacedItemType[0];
        }

        static char GetGroupBadge(string[] groupRucksacks)
        {
            char[] groupBadge = new char[1];

            HashSet<char> firstRucksackItemTypes = new HashSet<char>(groupRucksacks[0]);
            HashSet<char> secondRucksackItemTypes = new HashSet<char>(groupRucksacks[1]);
            HashSet<char> thirdRucksackItemTypes = new HashSet<char>(groupRucksacks[2]);

            firstRucksackItemTypes.IntersectWith(secondRucksackItemTypes);
            firstRucksackItemTypes.IntersectWith(thirdRucksackItemTypes);

            firstRucksackItemTypes.CopyTo(groupBadge);

            return groupBadge[0];
        }

        static int GetPriorityValue (char itemType)
        {
            int priorityValue;

            if (Char.IsLower(itemType))
            {
                priorityValue = (int)itemType - 96;
            }
            else
            {
                priorityValue = (int)itemType - 38;
            }

            return priorityValue;
        }
        static void Main(string[] args)
        {
            int sumOfGroupBadgesItemType = 0;

            var rucksacks = File.ReadAllText(@"..\..\..\..\input").Split("\n");

            for (int i = 0; i < rucksacks.Length - 2; i += 3)
            {
                string[] group = new string[3] { rucksacks[i], rucksacks[i + 1], rucksacks[i + 2] };

                sumOfGroupBadgesItemType += GetPriorityValue(GetGroupBadge(group));
            }
            

            Console.WriteLine(sumOfGroupBadgesItemType);
        }
    }
}
