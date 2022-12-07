using System;
using System.Collections.Generic;
using System.IO;

//TRANSLATION TABEL
/* Shape    Opponent    Player
 * 
 * Rock         A         X
 * Paper        B         Y
 * Scissor      C         Z
 */

namespace day_2_solution
{
    class Program
    {

        static public string ConvertEncodingSymbolToShapeName (string symbol)
        {
            switch (symbol)
            {
                case "A":
                case "X":
                    return "rock";                    

                case "B":
                case "Y":
                    return "papper";                    

                case "C":
                case "Z":
                    return "scissor";

                default:
                    throw new Exception("invalid symbol");                    
            }            
        }

         static public string ConvertEncodingSymbolToDesiredOutcome (string symbol)
        {
            switch (symbol)
            {
                case "X":
                    return "lose";

                case "Y":
                    return "draw";

                case "Z":
                    return "win";

                default:
                    throw new Exception("invalid symbol");
            }
        } 
             
        static void Main(string[] args)
        {
            Dictionary<string, int> shapeScore = new Dictionary<string, int>()
            {
                { "rock",       1 },
                { "papper",     2 },
                { "scissor",    3 }
            };

            //Order of strings (shapes) in tupple: (opponent, player)
            Dictionary<(string, string), int> outcomeScore = new Dictionary<(string, string), int>()
            {                
                //lose
                { ("rock", "scissor"),      0 },
                { ("papper", "rock"),       0 },
                { ("scissor", "papper"),    0 },

                //draw
                { ("rock", "rock"),         3 },
                { ("papper", "papper"),     3 },
                { ("scissor", "scissor"),   3 },

                //win
                { ("rock", "papper"),       6 },
                { ("papper", "scissor"),    6 },
                { ("scissor", "rock"),      6 }
            };

            Dictionary<(string, string), string> shapePicker = new Dictionary<(string, string), string>()
            {
                { ("rock", "lose"), "scissor" },
                { ("rock", "draw"), "rock" },
                { ("rock", "win"), "papper" },

                { ("papper", "lose"), "rock" },
                { ("papper", "draw"), "papper" },
                { ("papper", "win"), "scissor" },

                { ("scissor", "lose"), "papper" },
                { ("scissor", "draw"), "scissor" },
                { ("scissor", "win"), "rock" }
            };

            int totalScore = 0;            
            int value;
            string opponentShape, playerShape, desiredOutcome;

            foreach (string round in System.IO.File.ReadAllLines(@".\day-2-input"))
            {                
                string[] roundSymbols = round.Split(" ");
                
                opponentShape = ConvertEncodingSymbolToShapeName(roundSymbols[0]);
                desiredOutcome = ConvertEncodingSymbolToDesiredOutcome(roundSymbols[1]);

                shapePicker.TryGetValue((opponentShape, desiredOutcome), out playerShape);

                totalScore += shapeScore.TryGetValue(playerShape, out value) ? value : 0;
                totalScore += outcomeScore.TryGetValue((opponentShape, playerShape), out value) ? value : 0;
            }

            Console.WriteLine(totalScore);
        }
    }
}
