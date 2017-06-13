using IstorijosTestas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machinelearning_test
{
    public static class FileActions
    {

        public static void readDataFile(String file,ref double[][] input,ref double[][] output)
        {
            var inputNew = new List<double[]>();
            var outputNew = new List<double[]>();

            string[] lines = System.IO.File.ReadAllLines(file);
            foreach(String line in lines)
            {
                double[] input1 = new double[4];
                double[] output1 = new double[1];
                var numbers = line.Split(' ');
                
                for(int i = 0; i < 4; i++)
                {

                    input1[i] = Double.Parse(numbers[i]);
                }
                output1[0] = Double.Parse(numbers[4]);

                inputNew.Add(input1);
                outputNew.Add(output1);
            }

            input = inputNew.ToArray();
            output = outputNew.ToArray();

        }

        public static void readDataFile(String file, ref double[][] input)
        {
            var inputNew = new List<double[]>();

            string[] lines = System.IO.File.ReadAllLines(file);
            foreach (String line in lines)
            {
                double[] input1 = new double[4];
                var numbers = line.Split(' ');

                for (int i = 0; i < 4; i++)
                {
                    input1[i] = Double.Parse(numbers[i]);
                }

                inputNew.Add(input1);
            }

            input = inputNew.ToArray();
        }

        public static void writeToFile(String fileName, double[] data1, double[] data2, double[] data3)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(fileName);

            for(int i=0; i< data1.Length; i++)
            {
                String line = data1[i] + " " + data2[i] + " " + data3[i];
                file.WriteLine(line);
            }

            file.Close();
        }


        public static void AppendResult(String fileName, Test test, int agresyvymas)
        {
            String line = "\n"+ test.physical + " " + test.verbal + " " + test.anger + " " + test.hostility + " " + agresyvymas;
            File.AppendAllText(fileName, line);
        }

    }
}
