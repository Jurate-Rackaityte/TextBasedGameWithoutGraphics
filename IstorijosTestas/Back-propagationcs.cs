using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IstorijosTestas
{
    class Back_propagationcs
    {
        int numInput = 4;       //jei nepridesim lyties: 4
        int numHidden = 5;      //PAEKSPERIMENTUOTI kuris skaicius geriausiai tinka
        public int numOutput = 9;      //tureti omeny, jog gali keistis
        int numRows = 1000;     //PALA, KA SITAI REISKIA?

        double[][] trainData;       //VELIAU: PADARYTI TESTINIU DUOMENU NUSKAITYMA IS FAILO
        double[][] testData;        //VELIAU: PADARYTI TESTINIU DUOMENU NUSKAITYMA IS FAILO

        NeuralNetwork nn = new NeuralNetwork(numInput, numHidden, numOutput);

        int maxEpochs = 1000;       //PAEKSPERIMENTUOTI kuris skaicius geriausiai tinka

    }
}
