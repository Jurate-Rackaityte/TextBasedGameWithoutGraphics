using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machinelearning_test
{
    public class Regression : INetwork
    {
        double[][] input;
        double[] output;

        KernelSupportVectorMachine machine;
        SequentialMinimalOptimizationRegression teacher;

        public Regression(int inputCount)
        {
            machine = new Accord.MachineLearning.VectorMachines.KernelSupportVectorMachine(new Polynomial(2), inputCount);
        }

        public void initialiseSMORegressionTeacher()
        {

            teacher = new SequentialMinimalOptimizationRegression(machine, input, output);

        }

        public double getPrediction(double[] input)
        {
            return machine.Score(input);
        }

        public void setData(double[][] input, double[][] output)
        {
            this.input = input;
            this.output = convertArray(output);

        }

        public void teachNetwork()
        {
            teacher.Run();

        }


        public static double[] convertArray(double[][] array)
        {
            double[] newArray = new double[array.Length];
            var count = 0;
            foreach (double[] item in array)
            {
                newArray[count] = item[0];
                    count++;
            }
            return newArray;
        }

    }
}
