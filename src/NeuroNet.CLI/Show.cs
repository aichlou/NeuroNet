namespace NeuroNet.CLI;

using System.Xml.Schema;
using NeuroNet.Core;

static class ShowCLI
{
    public static void ShowNetwork(List<List<Neuron>> network)
    {
        ArgumentNullException.ThrowIfNull(network);
        Console.WriteLine("Showing Network:");
        int[] WeightLength = network[0].Select(x => x.GetWeights().Length).ToArray();
        Console.WriteLine($"Layer 0 (Input layer (cannot edit)): {WeightLength.All(x => x == WeightLength[0]) ? WeightLength[0] : WeightLength[1]}");
        for (int i = 0; i < network.Count; i++)
        {
            Console.WriteLine($"Layer {i + 1}:");
            ShowLayer(network[i]);
        }
    }

    public static void ShowLayer(List<Neuron> layer)
    {
        for (int j = 0; j < layer.Count; j++)
        {
            string weights = string.Join(", ", layer[j].GetWeights().Select(w => w.ToString("F2")));
            Console.WriteLine($"  Neuron {j + 1}: Weights: [{weights}]");
        }
    }
}