using System;
using System.IO;

namespace day_4_solution
{
    class Assignment
    {
        public int MinSectionID { get; }
        public int MaxSectionID { get; }

        //requires string format "N-M". Example: "2-4" 
        public Assignment(string sectionRange)
        {
            MinSectionID = int.Parse(sectionRange.Split("-")[0]);
            MaxSectionID = int.Parse(sectionRange.Split("-")[1]);
        }

        public bool IsSubSetOf(Assignment Y)
        {
            return MinSectionID >= Y.MinSectionID && MaxSectionID <= Y.MaxSectionID;           
        }

        public bool IsOverlapingWith(Assignment Y)
        {
            return IsSectionIDInAssignment(Y.MinSectionID) || IsSectionIDInAssignment(Y.MaxSectionID) || IsSubSetOf(Y);          
        }

        private bool IsSectionIDInAssignment (int sectionID)
        {
            return MinSectionID <= sectionID && sectionID <= MaxSectionID;
        }
    }

    class AssignmentPair
    {
        Assignment X { get; }
        Assignment Y { get; }

        //requires string format "N-M,X-Y". Example: "2-4,6-8"
        public AssignmentPair (string assigmentPair)
        {
            X = new Assignment(assigmentPair.Split(",")[0]);
            Y = new Assignment(assigmentPair.Split(",")[1]);
        }

        public bool IsOneSubsetOfOther()
        {
            return X.IsSubSetOf(Y) || Y.IsSubSetOf(X);
        }

        public bool IsOverlaping()
        {
            return X.IsOverlapingWith(Y);
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {            
            int overlapCount = 0;

            foreach (string line in File.ReadLines(@"..\..\..\..\input"))
            {
                AssignmentPair assignmentPair = new AssignmentPair(line);
                                
                if (assignmentPair.IsOverlaping())
                {
                    overlapCount++;
                }
            }
                        
            Console.WriteLine(overlapCount);
        }

    }
}
