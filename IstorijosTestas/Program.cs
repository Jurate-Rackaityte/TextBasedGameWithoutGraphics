using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IstorijosTestas
{
    class Program
    {
        static void Main(string[] args)
        {
            Story test = new Story("Eglute");
            StoryAndPointer pointer = new StoryAndPointer();
            bool hasOptions = false;                // jei turi pasirinkimu, tai juos ir rodysim, gaudysim tikslu input
            StoryAndPointer[] choices = new StoryAndPointer[10];

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
                        Console.WriteLine("[" +i + "] : " + choices[i].getStory());
                        
                    }
                    string answer = Console.ReadLine();
                    int index = -1;
                    while(!Int32.TryParse(answer, out index) || index >= choices.Length)
                    {
                        Console.WriteLine("BLOGAS INPUT");
                        answer = Console.ReadLine();
                    }
                    pointer.setPointer(test.getStoryIndex(choices[index].getNextStage()));
                }
                //jei yra options: juos parasyti 
                //pagauti is consoles input
                Console.WriteLine(test.myHealth);
                Console.WriteLine(test.myMana);
            }
            Console.WriteLine(test.myHealth);
            Console.WriteLine(test.myMana);
            Console.WriteLine(test.JoannaHealth);
            Console.WriteLine(test.JoannaMana);
            Console.WriteLine(test.JoshHealth);
            Console.WriteLine(test.JoshMana);
            Console.WriteLine(test.aggresiveness);
            Console.ReadLine();
        }
    }
}
