using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machinelearning_test
{
    public interface INetwork
    {
         void setData(double[][] input, double[][] output);
         void teachNetwork();
         double getPrediction(double[] input);
    }
}
