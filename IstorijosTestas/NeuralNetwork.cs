using System;

namespace IstorijosTestas
{
    class NeuralNetwork
    {
        private int numInput;           //3-4
        private int numHidden;          //eksperimentuoti ir ziureti, kiek pasleptu nodes geriausia naudoti
        private int numOutput;          //~9

        private double[] inputs;
        private double[][] ihWeights;
        private double[] hBiases;
        private double[] hOutputs;

        private double[][] hoWeights;
        private double[] oBiases;
        private double[] outputs;

        private Random rnd;

        public NeuralNetwork(int numInput, int numHidden,
          int numOutput)
        { . . }

        private static double[][] MakeMatrix(int rows,
          int cols, double v)
        { . . }

        private void InitializeWeights() { . . }

        public void SetWeights(double[] weights) { . . }
        public double[] GetWeights() { . . }

        public double[] ComputeOutputs(double[] xValues) { . . }
        private static double HyperTan(double x) { . . }
        private static double[] Softmax(double[] oSums) { . . }

        public double[] Train(double[][] trainData,
          int maxEpochs, double learnRate,
          double momentum)
        {

        }

        private void Shuffle(int[] sequence) { . . }
        private double Error(double[][] trainData) { . . }

        public double Accuracy(double[][] testData) { . . }
        private static int MaxIndex(double[] vector) { . . }
    }
}
