using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Machinelearning_test;

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
                JoannaHealth * 0.25 + aggression * 5));
            // kuo didesni sie rodikliai, tuo mazesnis agresyvumas
            agresyvumas = agresyvumas - Convert.ToInt32(Math.Floor(JoannaMana * 0.5 +
                JoshHealth * 0.1 + JoshMana * 0.25));
            return agresyvumas;
        }

        static void Main(string[] args)
        {
            String fileResults = @"C:\Users\Simas\Documents\Intelektika\Projekt\data.txt";


            Console.Write("if you want o play a game: select[1] \nif you want to analyze game data: select [2]\n");
            var pasirinkimas  = Int32.Parse( Console.ReadLine());
            if (pasirinkimas == 1)
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
                    if (!hasOptions)
                    {
                        Console.WriteLine(pointer.getStory());
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine(choices[0].getStory());
                        Console.WriteLine();
                        for (int i = 1; i < choices.Length; i++)
                        {
                            if (!choices[i].getStory().Equals(""))
                                Console.WriteLine("\t[" + i + "] : " + choices[i].getStory());

                        }
                        Console.WriteLine("---------------------------------------");
                        string answer = Console.ReadLine();
                        int index = -1;
                        while (!Int32.TryParse(answer, out index) || index >= choices.Length)
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


                FileActions.AppendResult(fileResults, a, agresvumas);

                Console.ReadLine();
            }

            if(pasirinkimas == 2)
            {

                String failas1 = @"C:\Users\Simas\Documents\Intelektika\Projekt\data1training.txt";
                ProcessData processData1 = new ProcessData(failas1);
                processData1.processData();


                String failas2 = @"C:\Users\Simas\Documents\Intelektika\Projekt\data1training.txt";
                ProcessData processData2 = new ProcessData(failas2);
                double[][] testingInput1 = new double[0][];
                double[][] testingOutput1 = new double[0][];

                FileActions.readDataFile(@"C: \Users\Simas\Documents\Intelektika\Projekt\data1testing.txt", ref testingInput1, ref testingOutput1);


                processData2.processDataNoOutput(processData1.output);

                double[][] testingInput2 = new double[0][];
                double[][] testingOutput2 = testingOutput1;
                FileActions.readDataFile(@"C: \Users\Simas\Documents\Intelektika\Projekt\data1testing.txt", ref testingInput2);


                double[] predictedTest1 = new double[testingOutput1.Length];
                double[] predictedTest2 = new double[testingOutput1.Length];

                for (int i = 0; i < testingInput1.Length; i++)
                {
                    for (int j = 0; j < testingOutput1[i].Length; j++)
                    {
                        Console.Write("\nShould be: " + testingOutput1[i][j]);
                    }

                    predictedTest1[i] = processData1.getPrediction(testingInput1[i]);
                    predictedTest2[i] = processData2.getPrediction(testingInput2[i]);
                    Console.Write(" prediction of test1: " + predictedTest1[i]);
                    Console.Write(" prediction of test2: " + predictedTest2[i]);
                }

                String resultFile = "dataResultFile.txt";

                FileActions.writeToFile(resultFile, predictedTest1, predictedTest2, Regression.convertArray(testingOutput1));
                var testT = new Test();
                testT.anger = 2;
                testT.hostility = 10;
                testT.physical = 15;
                testT.verbal = 20;
            
                FileActions.AppendResult(fileResults, testT, 999);
            }

            Console.ReadKey();

            //CIA IDETI AGRESYVUMA PRIE DATA

            //File.AppendAllText("outputResults.txt", test.myHealth + "," + test.myMana 
            //    + "," + test.JoannaHealth + "," + test.JoannaMana 
            //    + "," + test.JoshHealth + "," + test.JoshMana 
            //    + "," + test.aggresiveness +  Environment.NewLine);
        }
    }
}
