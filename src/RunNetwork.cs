namespace NeuroNet;

public static class RunNetwork
{
    public static void RunNeuralNetwork(List<List<Neuron>> network, List<double> inputData)
    {
        bool allWeightsZero = false;;
        for (int i = 0; i < network.Count; i++)
        {
            for (int j = 0; j < network[i].Count; j++)
            {
                if(network[i][j].weights.All(x => x == 0))
                {
                    allWeightsZero = true;
                    break;
                }
                if(network[i][j].bias == 0)
                {
                    allWeightsZero = true;
                    break;
                }
            }
            if(allWeightsZero)
            {
                break;
            }
        }
        if(allWeightsZero)
        {
            Console.WriteLine("Warning: Some neurons have all weights set to zero. Randomizing weights.");
            Random rand = new Random();
            for (int i = 0; i < network.Count; i++)
            {
                for (int j = 0; j < network[i].Count; j++)
                {
                    network[i][j].RandomizeWeights(rand);
                }
            }
        }
        for (int i = 0; i < network.Count; i++)
        {
            for (int j = 0; j < network[i].Count; j++)
            {
                Console.WriteLine("Processing Neuron " + (j + 1) + " in Layer " + (i + 1));
                if (i == 0)
                {
                    network[i][j].Fire([inputData.ToArray()[j]]);
                }
                else
                {
                    double[] inputs = network[i - 1].Select(neuron => neuron.value).ToArray();
                    network[i][j].Fire(inputs);
                }
            }
        }
        Console.WriteLine("Neural Network Output:");
        for (int j = 0; j < network[network.Count - 1].Count; j++)
        {
            Console.WriteLine($"Neuron {j + 1}: {network[network.Count - 1][j].value}");
        }
    }
}