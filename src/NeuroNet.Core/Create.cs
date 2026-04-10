namespace NeuroNet.Core;

public class Create {
    public static List<List<Neuron>> CreateNeuralNetwork(int[] networkData) //Creates a Neural Network based on user input
    {
        //string baseDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //string appDataPath = Path.Combine(baseDataPath, "NeuroNet");
        int layers = networkData.Length - 1;
        if (networkData[0] <= 0)
            throw new ArgumentOutOfRangeException(nameof(networkData), "Input layer size cannot be zero or negative");  
        List<List<Neuron>> network = new List<List<Neuron>>();
        for (int i = 0; i < layers; i++) 
        {
            int neuronCount = networkData[i + 1];
            if (neuronCount <= 0) throw new ArgumentOutOfRangeException(nameof(networkData), $"Number of Neurons in Layer {i + 1} cannot be zero or negative");
            network.Add(new List<Neuron>());

            for (int j = 0; j < neuronCount; j++)
            {
                if(i == 0)
                {
                    network[i].Add(new Neuron(0, new double[networkData[0]]));
                }
                else {
                    network[i].Add(new Neuron(0, new double[network[i - 1].Count]));
                }
            }
        }
        return network;
    }
    public static int[] NeuronClusterToArray(List<List<Neuron>> network)
    {
        try {
            int[] networkData = new int[network.Count + 1];
            networkData[0] = network[0][0].GetWeights().Length;
            for (int i = 0; i < network.Count; i++)
            {
                networkData[i + 1] = network[i].Count;
            }
            return networkData;
        }
        catch(Exception)
        {
            return new int[0];
        }
    }
}