using System.Reflection.Metadata.Ecma335;
using static NeuroNet.Core.Edit;
namespace NeuroNet.Core;

public class Run
{
    public static double[] RunNeuralNetwork(List<List<Neuron>> network, List<double> inputData)
    {
        
        if (network == null) throw new ArgumentNullException(nameof(network));
        if (inputData == null) throw new ArgumentNullException(nameof(inputData));
        if (network.Count == 0) throw new ArgumentException("Network cannot be empty.", nameof(network));
        if (network[0].Count != inputData.Count) throw new ArgumentException($"Input data count ({inputData.Count}) must match first layer neuron count ({network[0].Count}).");
           
        var lastLayer = network[network.Count - 1];
        double[] networkoutput = new double[lastLayer.Count];
        double[,] outputs = new double[network.Count(), 10000];
        for (int i = 0; i < network.Count; i++)
        {
            double[] output = new double[network[i].Count()];
            for (int j = 0; j < network[i].Count; j++)
            {
                if (i == 0)
                {
                    outputs[i, j] = network[i][j].Fire(inputData.ToArray());
                }
                else
                {
                    double[] inputs = new double[network[i-1].Count()];
                    for(int k = 0; k < inputs.Length; k++)
                    {
                        inputs[k] = outputs[i - 1, k];
                    }
                    outputs[i,j] = network[i][j].Fire(inputs);
                    /*double[] inputs = network[i - 1].Select(neuron => neuron.value).ToArray();
                    network[i][j].Fire(inputs); */
                }
            }
        }
        for (int i = 0; i < lastLayer.Count; i++)
        {
            networkoutput[i] = outputs[network.Count() - 1, i];
        }
        return networkoutput;
    }
}