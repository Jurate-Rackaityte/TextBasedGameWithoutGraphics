using AForge.Neuro;
using AForge.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machinelearning_test
{
    public class Network : INetwork
    {
        ActivationNetwork network;
        ISupervisedLearning teacher;

        double[][] input;
        double[][] output;

        double min;
        double max;

        double learningRate = 0.009;

        int iterations =3000;

        public Network(int intputCount, int neuronCount, int outputCount)  
        {
            network = new ActivationNetwork(
                        new SigmoidFunction(),
                        intputCount, neuronCount, outputCount);
        }
        public void initiateRBPNTeacher()
        {
            if (teacher == null)
            {
                teacher = new ResilientBackpropagationLearning(network);
            }
            else { Console.WriteLine(" teacher allready initiated "); }
        }
        public void initiateBPNTeacher()
        {
            if (teacher == null) {
                teacher = new BackPropagationLearning(network) { LearningRate = this.learningRate };
            } else { Console.WriteLine(" teacher allready initiated "); }
        }


        public double getPrediction(double[] input)
        {
            return Math.Round(ConvertRange(0, 1, min, max, network.Compute(convertPredictionArray(input, min, max))[0]));
        }

        public void setData(double[][] input, double[][] output)
        {
            this.max = getmax(input, output);
            this.min = getMin(input, output); // jeigu bus pastovios max min values irasyt ranka.

            this.input = convertArray(input, min, max);
            for (int i = 0; i < this.input.Length; i++)
            {
                for (int j = 0; j < this.input[i].Length; j++)
                {
                    Console.WriteLine(this.input[i][j]);
                }
            }
            this.output = convertArray(output, min, max);
        }

        public void teachNetwork()
        {
            int iteration = 0;
            while(iteration < iterations)
            {
                double error = teacher.RunEpoch(input, output);
                Console.Clear();
                Console.Write(" error: " + error);
                iteration++;
            }
            //network.
            //return error // jei bus reikalingas.
        }


        private double ConvertRange(
    double originalStart, double originalEnd, // original range
    double newStart, double newEnd, // desired range
    double value) // value to convert
        {
            double scale = (newEnd - newStart) / (originalEnd - originalStart);
            return (newStart + ((value - originalStart) * scale));
        }

        private double getmax(double[][] input, double[][] output)
        {
            var inList = input.ToList();
            inList.AddRange(output);
            var array = inList.ToArray();

            double max = double.MinValue;

            foreach (double[] item in array)
            {
                foreach (double value in item)
                {
                    if (value > max)
                    {
                        max = value;
                    }
                }
            }
            return max;
        }

        private double getMin(double[][] input, double[][] output)
        {
            var inList = input.ToList();
            inList.AddRange(output);
            var array = inList.ToArray();

            double min = double.MaxValue;

            foreach (double[] item in array)
            {
                foreach (double value in item)
                {
                    if (value < min)
                    {
                        min = value;
                    }
                }
            }
            return min;
        }

        public double[] convertPredictionArray(double[] array, double min, double max)
        {
            double[] newArray = new double[array.Length];

            var count = 0;
            foreach (double value in array)
            {
                newArray[count] = ConvertRange(min, max, 0, 1, value);

                //Console.Out.WriteLine(" was " + value + " now: " + newArray[counterI][counterJ]);
                count++;
            }
            return newArray;
        }

        private double[][] convertArray(double[][] array, double min, double max)
        {
            double[][] newArray = new double[array.Length][];

            var counterI = 0;

            foreach (double[] item in array)
            {
                var counterJ = 0;
                newArray[counterI] = new double[item.Length];
                foreach (double value in item)
                {
                    newArray[counterI][counterJ] = ConvertRange(min, max, 0, 1, value);

                    //Console.Out.WriteLine(" was " + value + " now: " + newArray[counterI][counterJ]);
                    counterJ++;
                }
                counterI++;
            }
            return newArray;
        }


    }
}
