using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machinelearning_test
{
     public class ProcessData
    {
        String file;
        public double[][] input;
        public double[][] output;

        List<INetwork> networks;
        public ProcessData(String file)
        {
            this.file = file;
            this.input = new double[0][];
            this.output = new double[0][];
            this.networks = new List<INetwork>();
        }
        public void processData()
        {
            FileActions.readDataFile(this.file, ref input, ref output);
            InitializeNetworks();
            setUpNetworks();
            trainNetworks();   
        }

        public void processDataNoOutput( double[][] output)
        {
            FileActions.readDataFile(this.file, ref input);
            this.output = output;
            InitializeNetworks();
            setUpNetworks();
            trainNetworks();
        }

        public double getPrediction(double[] input)
        {
            var votedResults = vote(input);
            if (isEqual(votedResults))
            {
                return getEqual(votedResults);
            }else
            {
                return getCalculatedVote(getMinDistance(votedResults), votedResults);
            }

            return 0;
        }

        private double getMinDistance(List<double> list)
        {
            var minimal = Double.MaxValue;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (i != j && Math.Abs(list[i] - list[j]) < minimal)
                    {
                        minimal = Math.Abs(list[i] - list[j]);
                    }
                }
            }

            return minimal;
        }

        private double getCalculatedVote(double distance, List<double> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (i != j && Math.Abs(list[i] - list[j]) == distance)
                    {
                        return (list[i] + list[j]) / 2;
                    }
                }
            }
            return 0;
        }


        private double getEqual(List<double> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (i != j && list[i] == list[j])
                    {
                        return list[i];
                    }
                }
            }
            return 0;
        }

        private Boolean isEqual(List<double> votes)
        {
            for (int i =0; i<votes.Count; i++)
            {
                for (int j = 0; j < votes.Count; j++)
                {
                    if(i !=j && votes[i] == votes[j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private List<double> vote(double[] input)
        {
            var predictions = new List<double>();

            foreach (INetwork network in networks)
            {
                var prediction = network.getPrediction(input);
                predictions.Add(prediction);

            }
            return predictions;
        }
        private void InitializeNetworks()
        {
            Network network1 = new Network(input[0].Length, 8, 1);
            network1.initiateBPNTeacher();

            Network network2 = new Network(input[0].Length, 8, 1);
            network2.initiateRBPNTeacher();


            AccordNetwork network3 = new AccordNetwork(input[0].Length, 8, 1);
            network3.initiateParallelRBL();

            //Regression network4 = new Regression(2);
            //network4.setData(input, output);
            //network4.initialiseSMORegressionTeacher();

            networks.Add(network1);
            networks.Add(network2);
            networks.Add(network3);
            // networks.Add(network4);
        }

        private void setUpNetworks()
        {
            foreach(INetwork network in networks)
            {
                network.setData(input, output);
            }
        }

        private void trainNetworks()
        {
            foreach (INetwork network in networks)
            {
                network.teachNetwork();
            }
        }

    }
}
