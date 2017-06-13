
//using Encog.MathUtil.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;


namespace Machinelearning_test
{
    public static class OutliersCalculator
    {
        private static int outlierMark = -666;

        public static void code()
        {
            String failas = @"C:\Users\Simas\Documents\Intelektika\Projekt\data.txt";
            var data = new double[] { 8,7,5,6,4,1,2,3,9 };
            double[][] testin = new double[0][];//= new double[][] {new double[] {8,2 }, new double[] { 3,4 }, new double[] {5,6}, new double[] { 4,2}, new double[] {7,5}, new double[] {15,10 } };
            double[][] testout = new double[0][];//= new double[][] {  new double[] {1 }, new double[] { 2}, new double[] {15 }, new double[] {3 }, new double[] { 4}, new double[] { 5} };
            FileActions.readDataFile(failas, ref testin, ref testout);


           cleanData(ref testin, ref testout);
           
            for(int i = 0; i<testin.Length; i++)
            {
                Console.Write("\n ");
                for (int j = 0; j< testin[i].Length; j++)
                {
                    Console.Write(" " + testin[i][j]);
                }
                for (int j = 0; j < testout[i].Length; j++)
                {
                    Console.Write(" " + testout[i][j]);
                }
            }
            
        }


        public static void cleanData(ref double[][] input,ref double[][] output) 
        {
            var list = getOutliersIndexes(input,output);
            deleteOutliers(ref input,ref output, list);
        }

        public static void deleteOutliers(ref double[][] input, ref double[][] output, List<List<int>> outliersIndexes)
        {
            foreach (List<int> list in outliersIndexes)
            {
                foreach(int item in list)
                {

                    for (int i = 0; i < input[0].Length; i++)
                    {
                        input[item][i] = outlierMark;
                    }

                    for (int i = 0; i < output[0].Length; i++)
                    {
                        output[item][i] = outlierMark;
                    }

                }
            }


            input = deleteMarked(input);
            output = deleteMarked(output);

        }

        private static double[][] deleteMarked(double[][] data)
        {
            var list = data.ToList();

            for (int i=0; i<list.Count; i++)
            {
                for(int j = 0; j< list[i].Length; j++)
                {
                    if (data[i][j] == outlierMark)
                    {
                        list.RemoveAt(i);
                        Console.WriteLine("elements deleted" );
                        break;
                    }
                }
            }
            return  list.ToArray();

        }

        private static List<List<int>> getOutliersIndexes(double[][] input, double[][] output)
        {
            var outliers = new List<List<int>>();
            for (int i = 0; i < input[0].Length; i++)
            {
                outliers.Add(findOutliersIndexes(createArayOfIndex(input, i)));
            }

            for (int i = 0; i < output[0].Length; i++)
            {
                outliers.Add(findOutliersIndexes(createArayOfIndex(output, i)));
            }
            return outliers;
        }

        private static List<int> findOutliersIndexes(double[] data)
        {
            var LowerLimit = findOulierFirstLimit(data);
            var UpperLimit = findOutlierSecondLimit(data);
            var oulietsIndexes = new List<int>();
            for(int i= 0; i< data.Length; i++)
            {
                Console.WriteLine(" data: " +data[i]);
                if(data[i]> UpperLimit || data[i]<LowerLimit)
                {
                    oulietsIndexes.Add(i);
                }
            }
            return oulietsIndexes;
        }

        private static double[] createArayOfIndex(double[][] data, int index)
        {
            var newArray = new double[data.Length];
            var count = 0;
            foreach(double[] item in data)
            {
                newArray[count] = item[index];
                    count++;
            }

            return newArray;
        }
       

        private static double findOulierFirstLimit(double[] data)
        {
            var Q1 = Statistics.LowerQuartile(data);
            var Q3 = Statistics.UpperQuartile(data);
            var IQR = Q3 - Q1;
            return (Q1 - 1.5 * IQR);
        }

        private static double findOutlierSecondLimit(double[] data)
        {
            var Q1 = Statistics.LowerQuartile(data);
            var Q3 = Statistics.UpperQuartile(data);
            var IQR = Q3 - Q1;
            return (Q3 + 1.5 * IQR);
        }


    }
}
