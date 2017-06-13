using System;
using System.Collections.Generic;
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
    }
}
