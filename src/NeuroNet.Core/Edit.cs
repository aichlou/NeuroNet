namespace NeuroNet.Core;

public class Edit
{
    public static void RandomizeIfNeeded(List<List<Neuron>> network, Action<string>? Message)
    {
        bool allWeightsZero = true;
        for (int i = 0; i < network.Count; i++)
        {
            for (int j = 0; j < network[i].Count; j++)
            {
                if(network[i][j].weights.All(x => x != 0))
                {
                    allWeightsZero = false;
                    break;
                }
                if(network[i][j].bias != 0)
                {
                    allWeightsZero = false;
                    break;
                }
            }
        }
        if(allWeightsZero)
        {
            Message?.Invoke("Warning: All neurons have all weights set to zero. Randomizing weights.");
            Random rand = new Random();
            for (int i = 0; i < network.Count; i++)
            {
                for (int j = 0; j < network[i].Count; j++)
                {
                    network[i][j].RandomizeWeights(rand);
                }
            }
            Message?.Invoke("Weights randomized.");
        }
    }
}