using NeuroNet.Core;

namespace NeuroNet.CLI;

class EditCLI
{
    public static List<List<Neuron>> RandomizeIfNeeded(List<List<Neuron>> network, string currentnnName)
    {
        bool allWeightsZero = Edit.allWeightsZero(network);
        if (allWeightsZero) {
            network = Edit.RandomizeWeights(network);
            Console.WriteLine("The weights were randomized since they were all null");
            SaveCLI.SaveNetworkToFile(network, "overwrite", currentnnName);
        }
        return network;
    }
}