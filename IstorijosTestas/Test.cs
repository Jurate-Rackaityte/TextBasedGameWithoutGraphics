using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IstorijosTestas
{
    public class Test
    {
        String[] Questions = 
            {"Once in a while I can't control the urge to strike another person.\n",
            "Given enough provocation, I may hit another person.\n",
            "If somebody hits me, I hit back.\n",
            "I get into fights a little more than the average person\n",
            "If I have to resort to violence to protect my rights, I will.\n",
            "There are people who pushed me so far that we came to blows.\n",
            "I can think of no good reason for ever hitting a person.\n",
            "I have threatened people I know.\n",
            "I have become so mad that I have broken things.\n",
            "I tell my friends openly when I disagree with them.\n",
            " I often find myself disagreeing with people.\n",
            "When people annoy me, I may tell them what I think of them.\n",
            "I can't help getting into arguments when people disagree with me.\n",
            "My friends say that I'm somewhat argumentative.\n",
            "I flare up quickly but get over it quickly.\n",
            "When frustrated, I let my irritation show.\n",
            "I sometimes feel like a powder keg ready to explode\n",
            "I am an even-tempered person.\n",
            "Some of my friends think I'm a hothead\n",
            "Sometimes I fly off the handle for no good reason.\n",
            "I have trouble controlling my temper.\n",
            "I am sometimes eaten up with jealousy.\n",
            "At times I feel I have gotten a raw deal out of life.\n",
            "Other people always seem to get the breaks.\n",
            "I wonder why sometimes I feel so bitter about things\n",
            "I know that \"friends\" talk about me behind my back.\n",
            "I am suspicious of overly friendly strangers.\n",
            "I sometimes feel that people are laughing at me behind me back.\n",
            "When people are especially nice, I wonder what they want.\n"};

        public double physical = 0;
        public double verbal = 0;
        public double anger = 0;
        public double hostility = 0;
        void Question(string q, ref double points)
        {
            Console.Write(q + "\n" + "1 - extremely uncharacteristic of me\n2 - uncharacteristic of me\n"+
                "3 - somewhat characteristic of me\n4 - I don't know\n5 - somewhat characteristic of me\n" +
                "6 - characteristic of me\n7 - extremely characteristic of me" + "\n");
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
                    Console.WriteLine("Invalid answer");
                    Question(q, ref points);
                    break;
            }
            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
        }  
        void writeToFile()
        {
            File.AppendAllText("testsResults.txt", physical+","+verbal+","+anger+","+hostility+Environment.NewLine);
        }
        public void AskAll()
        {
            Console.WriteLine("Please answer the following questions:\n\n");
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
