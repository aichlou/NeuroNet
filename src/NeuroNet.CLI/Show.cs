namespace NeuroNet.CLI;

using System.Xml.Schema;
using NeuroNet.Core;

static class ShowCLI
{
    public static void ShowNetwork(List<List<Neuron>> network)
    {
        ArgumentNullException.ThrowIfNull(network);
        Console.WriteLine("Showing Network:");
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
            string weights = string.Join(", ", layer[j].weights.Select(w => w.ToString("F2")));
            Console.WriteLine($"  Neuron {j + 1}: Weights: [{weights}]");
        }
    }
}