using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IstorijosTestas
{
    class Back_propagationcs
    {

        static int numInput = 4;       //jei nepridesim lyties: 4
        static int numHidden = 5;      //PAEKSPERIMENTUOTI kuris skaicius geriausiai tinka
        static public int numOutput = 9;      //tureti omeny, jog gali keistis
        int numRows = 1000;     //PALA, KA SITAI REISKIA?

        // veliau padaryti su 
        // Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName
        // kad ir kitiems automatiskai rastu failu vieta
        string trainInputFolderPath = @"C:\Users\Jurate\Documents\GitHub\TextBasedGameWithoutGraphics\train data\input";
        string trainOutputFolderPath = @"C:\Users\Jurate\Documents\GitHub\TextBasedGameWithoutGraphics\train data\output";

        string testInputFolderPath = @"C:\Users\Jurate\Documents\GitHub\TextBasedGameWithoutGraphics\test data\input";
        string testOutputFolderPath = @"C:\Users\Jurate\Documents\GitHub\TextBasedGameWithoutGraphics\test data\output";

<<<<<<< HEAD
        int[][] trainData;       //VELIAU: PADARYTI TESTINIU DUOMENU NUSKAITYMA IS FAILO
        int[][] testData;        //VELIAU: PADARYTI TESTINIU DUOMENU NUSKAITYMA IS FAILO
		
        NeuralNetwork nn = new NeuralNetwork(numInput, numHidden, numOutput);
=======
        //NeuralNetwork nn = new NeuralNetwork(numInput, numHidden, numOutput);
>>>>>>> no message

        // uzpildom trainData ir testData masyvus
        nn.readFiles(trainInputFolderPath, trainOutputFolderPath, out trainData, numInput, numOutput);

        int maxEpochs = 1000;       //PAEKSPERIMENTUOTI kuris skaicius geriausiai tinka
        double learnRate = 0.05;    //PAEKSPERIMENTUOTI kuris skaicius geriausiai tinka
        double momentum = 0.01;     //PAEKSPERIMENTUOTI kuris skaicius geriausiai tinka
                                    // The momentum rate helps prevent training from getting stuck with local, non-optimal 
                                    // weight values and also prevents oscillation where training never converges to stable values.

        double[] weights = nn.TrainBackProp(trainData, maxEpochs, learnRate, momentum);

        double trainAcc = nn.Accuracy(trainData);

        double testAcc = nn.Accuracy(testData);

    }

}
