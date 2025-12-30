using System.Reflection.Metadata.Ecma335;
using static NeuroNet.Core.Edit;
namespace NeuroNet.Core;

public class Run
{
    public static double[] RunNeuralNetwork(List<List<Neuron>> network, List<double> inputData, Action<string>? Message)
    {
        
        if (network == null) throw new ArgumentNullException(nameof(network));
        if (inputData == null) throw new ArgumentNullException(nameof(inputData));
        if (network.Count == 0) throw new ArgumentException("Network cannot be empty.", nameof(network));
        if (network[0].Count != inputData.Count)
            throw new ArgumentException($"Input data count ({inputData.Count}) must match first layer neuron count ({network[0].Count}).");
        Message?.Invoke("Feeding input data into the Neural Network...");    
        var lastLayer = network[network.Count - 1];
        for (int i = 0; i < network.Count; i++)
        {
            for (int j = 0; j < network[i].Count; j++)
            {
                //Message?.Invoke("Processing Neuron " + (j + 1) + " in Layer " + (i + 1));
                if (i == 0)
                {
                    network[i][j].Fire([inputData[j]]);
                }
                else
                {
                    double[] inputs = network[i - 1].Select(neuron => neuron.value).ToArray();
                    network[i][j].Fire(inputs);
                }
            }
        }
        double[] output = new double[lastLayer.Count];
        Message?.Invoke("Neural Network Output:");
        for (int j = 0; j < lastLayer.Count; j++)
        {
            output[j] = lastLayer[j].value;
            //Message?.Invoke($"Output {j}: {output[j]}");
            Message?.Invoke($"Neuron {j + 1}: {output[j]}");
        }
        return output;
    }
}