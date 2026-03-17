namespace NeuroNet.Core;

public class Create {
    public static List<List<Neuron>> CreateNeuralNetwork(int[] networkData) //Creates a Neural Network based on user input
    {
        string baseDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appDataPath = Path.Combine(baseDataPath, "NeuroNet");
        int layers = networkData.Length - 1;  
        List<List<Neuron>> network = new List<List<Neuron>>();
        for (int i = 0; i < layers; i++) 
        {
            int neuronCount = networkData[i + 1];
            if (neuronCount <= 0) throw new Exception("Number of Neurons in one Layer cannot be zero or lower");
            network.Add(new List<Neuron>());

            for (int j = 0; j < neuronCount; j++)
            {
                if(i == 0)
                {
                    network[i].Add(new Neuron(0, new double[networkData[0]])); //Mumpitz?
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
            networkData[0] = network[0][0].weights.Length;
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