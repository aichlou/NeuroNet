namespace NeuroNet.CLI;

using NeuroNet.Core;

class ShowCLI
{
    public static void ShowWeights(List<List<Neuron>> network)
    {
        Console.WriteLine("Showing Weights:");
        for (int i = 0; i < network.Count; i++)
        {
            Console.WriteLine($"Layer {i + 1}:");
            for (int j = 0; j < network[i].Count; j++)
            {
                string weights = string.Join(", ", network[i][j].weights.Select(w => w.ToString("F2")));
                Console.WriteLine($"  Neuron {j + 1}: Weights: [{weights}]");
            }
        }
    }
}