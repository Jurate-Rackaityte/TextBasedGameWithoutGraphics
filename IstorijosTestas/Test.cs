using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IstorijosTestas
{
    class Test
    {
        String[] Questions = 
            {"Once in a while I can't control the urge to strike another person.",
            "Given enough provocation, I may hit another person.",
            "If somebody hits me, I hit back.",
            "I get into fights a little more than the average person",
            "If I have to resort to violence to protect my rights, I will.",
            "There are people who pushed me so far that we came to blows.",
            "I can think of no good reason for ever hitting a person.",
            "I have threatened people I know.",
            "I have become so mad that I have broken things.",
            "I tell my friends openly when I disagree with them.",
            " I often find myself disagreeing with people.",
            "When people annoy me, I may tell them what I think of them.",
            "I can't help getting into arguments when people disagree with me.",
            "My friends say that I'm somewhat argumentative.",
            "I flare up quickly but get over it quickly.",
            "When frustrated, I let my irritation show.",
            "I sometimes feel like a powder keg ready to explode",
            "I am an even-tempered person.",
            "Some of my friends think I'm a hothead",
            "Sometimes I fly off the handle for no good reason.",
            "I have trouble controlling my temper.",
            "I am sometimes eaten up with jealousy.",
            "At times I feel I have gotten a raw deal out of life.",
            "Other people always seem to get the breaks.",
            "I wonder why sometimes I feel so bitter about things",
            "I know that \"friends\" talk about me behind my back.",
            "I am suspicious of overly friendly strangers.",
            "I sometimes feel that people are laughing at me behind me back.",
            "When people are especially nice, I wonder what they want."};

        double physical = 0;
        double verbal = 0;
        double anger = 0;
        double hostility = 0;
        void Question(string q, ref double points)
        {
            Console.Write(q + "\n" + "extremely uncharacteristic of me (1) to extremely characteristic of me (7)" + "\n");
            string answ = Console.ReadLine();
            switch (answ)
            {
                case "1":
                    points += 1;
                    break;
                case "2":
                    points += 2;
                    break;
                case "3":
                    points += 3;
                    break;
                case "4":
                    points += 4;
                    break;
                case "5":
                    points += 5;
                    break;
                case "6":
                    points += 6;
                    break;
                case "7":
                    points += 7;
                    break;
                default:
                    Console.WriteLine("Netinkamas parinkimas");
                    Question(q, ref points);
                    break;
            }
        }  
        void writeToFile()
        {
            File.AppendAllText("testsResults.txt", physical+","+verbal+","+anger+","+hostility+Environment.NewLine);
        }
        public void AskAll()
        {
            for(int i = 0; i < Questions.Length; i++)
            {
                if(i <= 8)
                    Question(Questions[i], ref physical);
                else if(i <= 13)
                    Question(Questions[i], ref verbal);
                else if (i <= 20)
                    Question(Questions[i], ref anger);
                else if (i <= 28)
                    Question(Questions[i], ref hostility);
            }
            //Console.WriteLine("Apklausos agresyvumas " + getAggressiveness());
            writeToFile();
        }
    }
}
