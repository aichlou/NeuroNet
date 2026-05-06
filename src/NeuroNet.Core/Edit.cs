using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NeuroNet.Core;

public class Edit
{
    public static bool allWeightsZero(List<List<Neuron>> network)
    {
        for (int i = 0; i < network.Count; i++)
        {
            for (int j = 0; j < network[i].Count; j++)
            {
                if(network[i][j].GetWeights().All(x => x != 0))
                {
                    return false;
                }
                if(network[i][j].GetBias() != 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static List<List<Neuron>> RandomizeWeights(List<List<Neuron>> network, double minValue = -1, double maxValue = 1)
    {
        if (minValue > maxValue) throw new ArgumentException($"minVlaue ({minValue}) must be less than or equal to maxValue ({maxValue})");
        Random rand = new Random();
        for (int i = 0; i < network.Count; i++)
        {
            foreach (Neuron neuron in network[i])
            {
                neuron.RandomizeWeights(rand, minValue, maxValue);
            }
        }
        return network;
    }

    public static List<List<Neuron>> adjustweights(List<List<Neuron>> network)
    {
        for(int i=0; i < network.Count; i++)
        {
            for(int j=0; j < network[i].Count; j++)
            {
                if(i == 0)
                {
                    network[i][j].SetWeights(new double[1]);
                }
                else
                {
                    network[i][j].SetWeights(new double[network[i - 1].Count]);
                }
            }
        }
        return network;
    }
    public static string CloneFile(string FirstFile, string newFileName)
    {
        var dataMulti = Load.ContentOf(FirstFile);
        if (dataMulti.HasError) return $"Error: {dataMulti.ErrorMessage}";
        if (dataMulti.Value == null) { return "Error: Value is null";}
        try
        {
            var jsonNode = JsonNode.Parse(dataMulti.Value);
            if (jsonNode?["Metadata"] is JsonObject metadata)
            {
                metadata["Name"] = newFileName;
            }
            string content = jsonNode?.ToJsonString() ?? "";
            Save.SaveNetworkToFile(newFileName, content);
            return "done";
        }
        catch (JsonException ex)
        {
            return $"Error: Invalid JSON format - {ex.Message}";
        }
    }

    public static List<List<Neuron>> AddLayers(List<List<Neuron>> network, int[] LayerCount)
    {
        Console.WriteLine($"Add {LayerCount.Length} Layers");
        for(int i = 0; i < LayerCount.Count(); i++)
        {
            //Console.WriteLine($"Add Layer {i + 1} with {LayerCount[i]} Neurons");
            List<Neuron> Layer = new List<Neuron>(LayerCount[i]);
            Random rand = new Random();
            double[] emptyWeights = new double[network[network.Count - 1].Count];
            for (int j = 0; j < LayerCount[i]; j++) {
                //Console.WriteLine("New Neuron");
                Neuron neuron = new Neuron(0, emptyWeights);
                neuron.RandomizeWeights(rand);
                Layer.Add(neuron);
            }
            network.Add(Layer);
        }
        return network;
    }

    public static List<List<Neuron>> AddNeurons(List<List<Neuron>> network, int layer, int amount)
    {
        Random rand = new Random();
        double[] emptyWeights = new double[network[layer - 1].Count];
        Neuron[] newNeurons = new Neuron[amount];
        for(int i = 0; i < newNeurons.Length; i++)
        {
            Neuron newNeuron = new Neuron(0, emptyWeights);
            newNeuron.RandomizeWeights(rand);
            newNeurons[i] = newNeuron;
        }
        network[layer].AddRange(newNeurons);
        if (layer != network.Count - 1)
        {
            foreach(Neuron neuron in network[layer + 1])
            {
                double[] Weights = neuron.GetWeights();
                double average = Weights.Average();
                double[] addWeights = Enumerable.Repeat(average, amount).ToArray();
                double[] newWeights = Weights.Concat(addWeights).ToArray();
                neuron.SetWeights(newWeights);
            }
        }
        return network;
    }
}