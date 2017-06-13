using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IstorijosTestas
{
    class Program
    {

        public int agresyvumasZaidime(int playersHealth, int playersMana, int JoannaHealth, int JoannaMana, 
            int JoshHealth, int JoshMana, int aggression)
        {
            int agresyvumas;
            // kuo didesni sie rodikliai, tuo didesnis agresyvumas
            agresyvumas = Convert.ToInt32(Math.Floor(playersHealth * 0.5 + playersMana * 0.5 + 
                JoannaHealth * 0.25 + aggression * 2));
            // kuo didesni sie rodikliai, tuo mazesnis agresyvumas
            agresyvumas = agresyvumas - Convert.ToInt32(Math.Floor(JoannaMana * 0.5 + 
                JoshHealth * 0.1 + JoshMana * 0.25));
            return agresyvumas;
        }

        static void Main(string[] args)
        {
            // Agresyvumo testo klausimai
            Test a = new Test();
            a.AskAll();

            string vardas = "";
            Console.Write("Write your name and press ENTER: ");
            vardas = Console.ReadLine();
            Story test = new Story(vardas);               // gauti zaidejo varda is pacio zaidejo
            StoryAndPointer pointer = new StoryAndPointer();
            bool hasOptions = false;                // jei turi pasirinkimu, tai juos ir rodysim, gaudysim tikslu input
            StoryAndPointer[] choices = new StoryAndPointer[10];

            Console.WriteLine("Now please read the story and choose the best fit answers.");
            Console.WriteLine("It\'s a text-based game, so don\'t worry. Every answer is a good answer.");
            Console.WriteLine("Just try to survive.");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();

            while (!test.thisIsTheEnd())
            {
                hasOptions = false;
                
                if (test.nextIsTheEndOfOption(pointer))
                {
                    if (test.nextHasOptions(pointer))
                    {
                        choices = test.lastTextAndOptions(pointer);
                        hasOptions = true;
                    }   
                    else
                    {
                        pointer = test.lastTextWithoutOptions(pointer);
                        
                    }
                }
                else
                {
                    pointer = test.nextStory(pointer);
                }
                //parasyti i console teksta
                if(!hasOptions)
                {
                    Console.WriteLine(pointer.getStory());
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine(choices[0].getStory());
                    Console.WriteLine();
                    for(int i = 1; i < choices.Length; i++)
                    {
                        if(!choices[i].getStory().Equals(""))
                        Console.WriteLine("\t[" +i + "] : " + choices[i].getStory());
                        
                    }
                    Console.WriteLine("---------------------------------------");
                    string answer = Console.ReadLine();
                    int index = -1;
                    while(!Int32.TryParse(answer, out index) || index >= choices.Length)
                    {
                        Console.WriteLine("WRONG INPUT");
                        answer = Console.ReadLine();
                    }
                    pointer.setPointer(test.getStoryIndex(choices[index].getNextStage()));
                }
                //jei yra options: juos parasyti 
                //pagauti is consoles input
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("\tHealth: " + test.myHealth);
                Console.WriteLine("\tMana: " + test.myMana);
                Console.WriteLine("---------------------------------------");
                Console.WriteLine();
            }
            Program p = new Program();
            int agresvumas = p.agresyvumasZaidime(test.myHealth, test.myMana,
                test.JoannaHealth, test.JoannaMana, test.JoshHealth,
                test.JoshMana, test.aggresiveness);
            Console.WriteLine("Player\'s health: " + test.myHealth);
            Console.WriteLine("Player\'s mana: " + test.myMana);
            Console.WriteLine("Joanna\'s health: " + test.JoannaHealth);
            Console.WriteLine("Joanna\'s mana: " + test.JoannaMana);
            Console.WriteLine("Josh\'s health: " + test.JoshHealth);
            Console.WriteLine("Josh\'s mana: " + test.JoshMana);
            Console.WriteLine("Aggresiveness in game: " + test.aggresiveness);
            Console.WriteLine("Total calculated aggresiveness: " + agresvumas);
            Console.ReadLine();

            //CIA IDETI AGRESYVUMA PRIE DATA

            //File.AppendAllText("outputResults.txt", test.myHealth + "," + test.myMana 
            //    + "," + test.JoannaHealth + "," + test.JoannaMana 
            //    + "," + test.JoshHealth + "," + test.JoshMana 
            //    + "," + test.aggresiveness +  Environment.NewLine);
        }
    }
}
