//using System;
//using System.IO;

//namespace IstorijosTestas
//{
//    class NeuralNetwork
//    {
//        private int numInput;           //3-4
//        private int numHidden;          //eksperimentuoti ir ziureti, kiek pasleptu nodes geriausia naudoti
//        private int numOutput;          //~9

//        private double[] inputs;
//        private double[][] ihWeights;
//        private double[] hBiases;
//        private double[] hOutputs;

//        private double[][] hoWeights;
//        private double[] oBiases;
//        private double[] outputs;

//        private Random rnd;
//        konstruktorius(suteikiami atsitiktiniai svoriai)
//        public NeuralNetwork(int numInput, int numHidden,
//          int numOutput)
//        {
//            this.numInput = numInput;
//            this.numHidden = numHidden;
//            this.numOutput = numOutput;

//            this.inputs = new double[numInput];

//            this.ihWeights = MakeMatrix(numInput, numHidden, 0.0);
//            this.hBiases = new double[numHidden];
//            this.hOutputs = new double[numHidden];

//            this.hoWeights = MakeMatrix(numHidden, numOutput, 0.0);
//            this.oBiases = new double[numOutput];
//            this.outputs = new double[numOutput];

//            //inicializuojame atsitiktinius svorius
//            this.rnd = new Random(0);
//            this.InitializeWeights(); // all weights and biases
//        }

//        //helper for constructor, Train
//        //padaroma ir grazinama rows x cols dydzio matrica, kurios visi kintamieji turi reiksme v
//        private static double[][] MakeMatrix(int rows,
//          int cols, double v)
//        {
//            double[][] result = new double[rows][];
//            for (int r = 0; r < result.Length; ++r)
//                result[r] = new double[cols];
//            for (int i = 0; i < rows; ++i)
//                for (int j = 0; j < cols; ++j)
//                    result[i][j] = v;
//            return result;
//        }

//        //konstruktoriui (helper for constructor)
//        //inicializuojamos pirmosios pradines atsitiktines neurono svoriu reiksmes
//        private void InitializeWeights()
//        {
//            // initialize weights and biases to small random values

//            // suskaiciuojama, kiek reikes/yra svoriu (pagal formule)
//            int numWeights = (numInput * numHidden) +
//              (numHidden * numOutput) + numHidden + numOutput;

//            // inicializuojama pradiniu svoriu matrica
//            double[] initialWeights = new double[numWeights];

//            // pradiniu svoriu matrica uzpildoma atsitiktiniais mazais skaiciais
//            for (int i = 0; i < initialWeights.Length; ++i)
//                initialWeights[i] = (0.001 - 0.0001) * rnd.NextDouble() + 0.0001;

//            // neurono svoriai nustatomi i pradinius atsitiktinius svorius
//            this.SetWeights(initialWeights);
//        }

//        // copy serialized weights and biases in weights[] array
//        // to i-h weights, i-h biases, h-o weights, h-o biases
//        // masyvu kopijavimas
//        public void SetWeights(double[] weights)
//        {
//            // pagal formule suskaiciuojama, kiek yra svoriu
//            int numWeights = (numInput * numHidden) +
//                (numHidden * numOutput) + numHidden + numOutput;
//            // jei duotas masyvas turi kitoki svoriu skaiciu, nei kad suskaiciuotasis, vadinasi, buvo
//            //  negalim kopijuoti svoriu
//            if (weights.Length != numWeights)
//                throw new Exception("Bad weights array in SetWeights");

//            int k = 0; // points into weights param

//            for (int i = 0; i < numInput; ++i)
//                for (int j = 0; j < numHidden; ++j)
//                    ihWeights[i][j] = weights[k++];

//            for (int i = 0; i < numHidden; ++i)
//                hBiases[i] = weights[k++];

//            for (int i = 0; i < numHidden; ++i)
//                for (int j = 0; j < numOutput; ++j)
//                    hoWeights[i][j] = weights[k++];

//            for (int i = 0; i < numOutput; ++i)
//                oBiases[i] = weights[k++];
//        }

//        // grazina neurone esancius svorius
//        public double[] GetWeights()
//        {
//            // pagal formule apskaiciuojamas neurono svoriu skaicius
//            int numWeights = (numInput * numHidden) +
//                (numHidden * numOutput) + numHidden + numOutput;

//            double[] result = new double[numWeights];
//            int k = 0;

//            for (int i = 0; i < ihWeights.Length; ++i)
//                for (int j = 0; j < ihWeights[0].Length; ++j)
//                    result[k++] = ihWeights[i][j];

//            for (int i = 0; i < hBiases.Length; ++i)
//                result[k++] = hBiases[i];

//            for (int i = 0; i < hoWeights.Length; ++i)
//                for (int j = 0; j < hoWeights[0].Length; ++j)
//                    result[k++] = hoWeights[i][j];

//            for (int i = 0; i < oBiases.Length; ++i)
//                result[k++] = oBiases[i];

//            return result;
//        }

//        //PASIZIURETI IR ISSIAISKINTI
//        public double[] ComputeOutputs(double[] xValues)
//        {
//            double[] hSums = new double[numHidden]; // hidden nodes sums scratch array
//            double[] oSums = new double[numOutput]; // output nodes sums

//            for (int i = 0; i < xValues.Length; ++i) // copy x-values to inputs
//                this.inputs[i] = xValues[i];
//            // note: no need to copy x-values unless you implement a ToString.
//            // more efficient is to simply use the xValues[] directly.

//            for (int j = 0; j < numHidden; ++j)  // compute i-h sum of weights * inputs
//                for (int i = 0; i < numInput; ++i)
//                    hSums[j] += this.inputs[i] * this.ihWeights[i][j]; // note +=

//            for (int i = 0; i < numHidden; ++i)  // add biases to hidden sums
//                hSums[i] += this.hBiases[i];

//            for (int i = 0; i < numHidden; ++i)   // apply activation
//                this.hOutputs[i] = HyperTan(hSums[i]); // hard-coded

//            for (int j = 0; j < numOutput; ++j)   // compute h-o sum of weights * hOutputs
//                for (int i = 0; i < numHidden; ++i)
//                    oSums[j] += hOutputs[i] * hoWeights[i][j];

//            for (int i = 0; i < numOutput; ++i)  // add biases to output sums
//                oSums[i] += oBiases[i];

//            double[] softOut = Softmax(oSums); // all outputs at once for efficiency
//            Array.Copy(softOut, outputs, softOut.Length);

//            double[] retResult = new double[numOutput]; // could define a GetOutputs 
//            Array.Copy(this.outputs, retResult, retResult.Length);

//            return retResult;
//        }

//        // PAZIURETI, KA SITAS DARO IR AR MUMS JO IS VISO REIKIA
//        private static double HyperTan(double x)
//        {
//            if (x < -20.0) return -1.0; // approximation is correct to 30 decimals
//            else if (x > 20.0) return 1.0;
//            else return Math.Tanh(x);
//        }

//        // PAZIURETI, KA DARO IR AR REIKIA
//        private static double[] Softmax(double[] oSums)
//        {
//            // does all output nodes at once so scale
//            // doesn't have to be re-computed each time

//            double sum = 0.0;
//            for (int i = 0; i < oSums.Length; ++i)
//                sum += Math.Exp(oSums[i]);

//            double[] result = new double[oSums.Length];
//            for (int i = 0; i < oSums.Length; ++i)
//                result[i] = Math.Exp(oSums[i]) / sum;

//            return result; // now scaled so that xi sum to 1.0
//        }

//        // train using back-prop algorithm
//        // PAZIURETI IR PATIKRINTI
//        public double[] TrainBackProp(int[][] trainData,
//          int maxEpochs, double learnRate,
//          double momentum)
//        {
//            // train using back-prop
//            // back-prop specific arrays
//            double[][] hoGrads = MakeMatrix(numHidden, numOutput, 0.0); // hidden-to-output weight gradients
//            double[] obGrads = new double[numOutput];                   // output bias gradients

//            double[][] ihGrads = MakeMatrix(numInput, numHidden, 0.0);  // input-to-hidden weight gradients
//            double[] hbGrads = new double[numHidden];                   // hidden bias gradients

//            double[] oSignals = new double[numOutput];                  // local gradient output signals - gradients w/o associated input terms
//            double[] hSignals = new double[numHidden];                  // local gradient hidden node signals

//            // back-prop momentum specific arrays 
//            double[][] ihPrevWeightsDelta = MakeMatrix(numInput, numHidden, 0.0);
//            double[] hPrevBiasesDelta = new double[numHidden];
//            double[][] hoPrevWeightsDelta = MakeMatrix(numHidden, numOutput, 0.0);
//            double[] oPrevBiasesDelta = new double[numOutput];

//            int epoch = 0;
//            double[] xValues = new double[numInput]; // inputs
//            double[] tValues = new double[numOutput]; // target values
//            double derivative = 0.0;
//            double errorSignal = 0.0;

//            int[] sequence = new int[trainData.Length];
//            for (int i = 0; i < sequence.Length; ++i)
//                sequence[i] = i;

//            int errInterval = maxEpochs / 10; // interval to check error
//            while (epoch < maxEpochs)
//            {
//                ++epoch;

//                if (epoch % errInterval == 0 && epoch < maxEpochs)
//                {
//                    double trainErr = Error(trainData);
//                    Console.WriteLine("epoch = " + epoch + "  error = " +
//                      trainErr.ToString("F4"));
//                    //Console.ReadLine();
//                }

//                Shuffle(sequence); // visit each training data in random order
//                for (int ii = 0; ii < trainData.Length; ++ii)
//                {
//                    int idx = sequence[ii];
//                    Array.Copy(trainData[idx], xValues, numInput);
//                    Array.Copy(trainData[idx], numInput, tValues, 0, numOutput);
//                    ComputeOutputs(xValues); // copy xValues in, compute outputs 

//                    // indices: i = inputs, j = hiddens, k = outputs

//                    // 1. compute output node signals (assumes softmax)
//                    for (int k = 0; k < numOutput; ++k)
//                    {
//                        errorSignal = tValues[k] - outputs[k];  // Wikipedia uses (o-t)
//                        derivative = (1 - outputs[k]) * outputs[k]; // for softmax
//                        oSignals[k] = errorSignal * derivative;
//                    }

//                    // 2. compute hidden-to-output weight gradients using output signals
//                    for (int j = 0; j < numHidden; ++j)
//                        for (int k = 0; k < numOutput; ++k)
//                            hoGrads[j][k] = oSignals[k] * hOutputs[j];

//                    // 2b. compute output bias gradients using output signals
//                    for (int k = 0; k < numOutput; ++k)
//                        obGrads[k] = oSignals[k] * 1.0; // dummy assoc. input value

//                    // 3. compute hidden node signals
//                    for (int j = 0; j < numHidden; ++j)
//                    {
//                        derivative = (1 + hOutputs[j]) * (1 - hOutputs[j]); // for tanh
//                        double sum = 0.0; // need sums of output signals times hidden-to-output weights
//                        for (int k = 0; k < numOutput; ++k)
//                        {
//                            sum += oSignals[k] * hoWeights[j][k]; // represents error signal
//                        }
//                        hSignals[j] = derivative * sum;
//                    }

//                    // 4. compute input-hidden weight gradients
//                    for (int i = 0; i < numInput; ++i)
//                        for (int j = 0; j < numHidden; ++j)
//                            ihGrads[i][j] = hSignals[j] * inputs[i];

//                    // 4b. compute hidden node bias gradients
//                    for (int j = 0; j < numHidden; ++j)
//                        hbGrads[j] = hSignals[j] * 1.0; // dummy 1.0 input

//                    // == update weights and biases

//                    // update input-to-hidden weights
//                    for (int i = 0; i < numInput; ++i)
//                    {
//                        for (int j = 0; j < numHidden; ++j)
//                        {
//                            double delta = ihGrads[i][j] * learnRate;
//                            ihWeights[i][j] += delta; // would be -= if (o-t)
//                            ihWeights[i][j] += ihPrevWeightsDelta[i][j] * momentum;
//                            ihPrevWeightsDelta[i][j] = delta; // save for next time
//                        }
//                    }

//                    // update hidden biases
//                    for (int j = 0; j < numHidden; ++j)
//                    {
//                        double delta = hbGrads[j] * learnRate;
//                        hBiases[j] += delta;
//                        hBiases[j] += hPrevBiasesDelta[j] * momentum;
//                        hPrevBiasesDelta[j] = delta;
//                    }

//                    // update hidden-to-output weights
//                    for (int j = 0; j < numHidden; ++j)
//                    {
//                        for (int k = 0; k < numOutput; ++k)
//                        {
//                            double delta = hoGrads[j][k] * learnRate;
//                            hoWeights[j][k] += delta;
//                            hoWeights[j][k] += hoPrevWeightsDelta[j][k] * momentum;
//                            hoPrevWeightsDelta[j][k] = delta;
//                        }
//                    }

//                    // update output node biases
//                    for (int k = 0; k < numOutput; ++k)
//                    {
//                        double delta = obGrads[k] * learnRate;
//                        oBiases[k] += delta;
//                        oBiases[k] += oPrevBiasesDelta[k] * momentum;
//                        oPrevBiasesDelta[k] = delta;
//                    }

//                } // each training item

//            } // while
//            double[] bestWts = GetWeights();
//            return bestWts;
//        }

//        // ismaiso masyva
//        // PAZIURETI, AR REIKIA
//        private void Shuffle(int[] sequence) // instance method
//        {
//            for (int i = 0; i < sequence.Length; ++i)
//            {
//                int r = this.rnd.Next(i, sequence.Length);
//                int tmp = sequence[r];
//                sequence[r] = sequence[i];
//                sequence[i] = tmp;
//            }
//        }

//        // MeanSquaredError
//        private double Error(int[][] trainData)
//        {
//            // average squared error per training item
//            double sumSquaredError = 0.0;
//            double[] xValues = new double[numInput]; // first numInput values in trainData
//            double[] tValues = new double[numOutput]; // last numOutput values

//            // walk thru each training case. looks like (6.9 3.2 5.7 2.3) (0 0 1)
//            for (int i = 0; i < trainData.Length; ++i)
//            {
//                Array.Copy(trainData[i], xValues, numInput);
//                Array.Copy(trainData[i], numInput, tValues, 0, numOutput); // get target values
//                double[] yValues = this.ComputeOutputs(xValues); // outputs using current weights
//                for (int j = 0; j < numOutput; ++j)
//                {
//                    double err = tValues[j] - yValues[j];
//                    sumSquaredError += err * err;
//                }
//            }
//            return sumSquaredError / trainData.Length;
//        }

//        public double Accuracy(int[][] testData)
//        {
//            // percentage correct using winner-takes all
//            int numCorrect = 0;
//            int numWrong = 0;
//            double[] xValues = new double[numInput]; // inputs
//            double[] tValues = new double[numOutput]; // targets
//            double[] yValues; // computed Y

//            for (int i = 0; i < testData.Length; ++i)
//            {
//                Array.Copy(testData[i], xValues, numInput); // get x-values
//                Array.Copy(testData[i], numInput, tValues, 0, numOutput); // get t-values
//                yValues = this.ComputeOutputs(xValues);
//                int maxIndex = MaxIndex(yValues); // which cell in yValues has largest value?
//                int tMaxIndex = MaxIndex(tValues);

//                if (maxIndex == tMaxIndex)
//                    ++numCorrect;
//                else
//                    ++numWrong;
//            }
//            return (numCorrect * 1.0) / (numCorrect + numWrong);
//        }
//        private static int MaxIndex(double[] vector)
//        {
//            // index of largest value
//            int bigIndex = 0;
//            double biggestVal = vector[0];
//            for (int i = 0; i < vector.Length; ++i)
//            {
//                if (vector[i] > biggestVal)
//                {
//                    biggestVal = vector[i];
//                    bigIndex = i;
//                }
//            }
//            return bigIndex;
//        }

//        // pagalbine klase train ir/ar test duomenu paemimui ir uzpildymui i masyva
//        public void readFiles(string inputFolderPath, string outputFolderPath,
//           out int[][] rezArray, int numOfInputs, int numOfOutputs)
//        {
//            // rezultatu masyvo indeksai
//            int i = 0;
//            int j = 0;

//            // rezultatu masyvo dydis
//            int x = 0;
//            int y = 0;

//            // suskaiciuojame rezultatu masyvo dydi
//            foreach (string file in Directory.EnumerateFiles(inputFolderPath, "*.txt"))
//            {
//                x++;
//            }
//            foreach (string file in Directory.EnumerateFiles(outputFolderPath, "*.txt"))
//            {
//                y++;
//            }

//            // rezultatu masyvo inicializacija
//            rezArray = new int[x][];
//            for (int ii = 0; ii < x; ii++)
//            {
//                rezArray[ii] = new int[y];
//            }

//            // input ir output rezultatus pradziai sudesiu i atskirus masyvus
//            int[][] inputArray = new int[numOfInputs][];
//            for (int ii = 0; ii < x; ii++)
//                inputArray[ii] = new int[x];

//            int[][] outputArray = new int[numOfOutputs][];
//            for (int ii = 0; ii < y; ii++)
//                outputArray[ii] = new int[y];

//            // input paemimas ir sudejimas i masyva
//            foreach (string file in Directory.EnumerateFiles(inputFolderPath, "*.txt"))
//            {
//                string line;
//                StreamReader f = new StreamReader(file);
//                while ((line = f.ReadLine()) != null)
//                {
//                    int temp = int.Parse(line);
//                    inputArray[i++][j] = temp;
//                    //rezArray[i++][j] = temp;
//                }
//                f.Close();
//                i = 0; j++;
//                //string contents = File.ReadAllText(file);
//            }
//            i = 0; j = 0;
//            foreach (string file in Directory.EnumerateFiles(outputFolderPath, "*.txt"))
//            {
//                string line;
//                StreamReader f = new StreamReader(file);
//                while ((line = f.ReadLine()) != null)
//                {
//                    int temp = int.Parse(line);
//                    outputArray[i++][j] = temp;
//                    //rezArray[i++][j] = temp;
//                }
//                f.Close();
//                i = 0; j++;
//                //string contents = File.ReadAllText(file);
//                // apdoroti teksta
//                // sudeti rezultatus i masyva
//            }

//            // Okay, dabar belieka viska sudeti i rezultatu masyva... ir issiaiskinti, 
//            //  kaip turetu atrodyti tas rezultatu masyvas...
//        }

//    }
//}

////        public NeuralNetwork(int numInput, int numHidden,
////          int numOutput)
////        { . . }

////        private static double[][] MakeMatrix(int rows,
////          int cols, double v)
////        { . . }

////        private void InitializeWeights() { . . }

////        public void SetWeights(double[] weights) { . . }
////        public double[] GetWeights() { . . }

////        public double[] ComputeOutputs(double[] xValues) { . . }
////        private static double HyperTan(double x) { . . }
////        private static double[] Softmax(double[] oSums) { . . }

////        public double[] Train(double[][] trainData,
////          int maxEpochs, double learnRate,
////          double momentum)
////        {

////        }

////        private void Shuffle(int[] sequence) { . . }
////        private double Error(double[][] trainData) { . . }

////        public double Accuracy(double[][] testData) { . . }
////        private static int MaxIndex(double[] vector) { . . }
////    }
////}

