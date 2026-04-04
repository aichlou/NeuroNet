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
                if(network[i][j].weights.All(x => x != 0))
                {
                    return false;
                }
                if(network[i][j].bias != 0)
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
            for (int j = 0; j < network[i].Count; j++)
            {
                network[i][j].RandomizeWeights(rand, minValue, maxValue);
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
    public static string CloneFile(string FirstFile, string newFileName, string currentnnName)
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

    public static List<List<Neuron>> AddLayers (List<List<Neuron>> network, int[] LayerCount)
    {
        for(int i = 0; i < LayerCount.Count(); i++)
        {
            List<Neuron> Layer = new List<Neuron>(LayerCount[i]);
            double[] emptyWeights = new double[network[network.Count - 1].Count];
            foreach (Neuron neuron in Layer) {
                neuron.SetWeights(emptyWeights);
                Random rand = new Random();
                neuron.RandomizeWeights(rand);
            }
            network.Add(Layer);
        }
        return network;
    }
}