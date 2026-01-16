namespace NeuroNet.Core;

public class Create {
    public static List<List<Neuron>> CreateNeuralNetwork(int[] networkData) //Creates a Neural Network based on user input
    {
        string baseDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appDataPath = Path.Combine(baseDataPath, "NeuroNet");
        int layers = networkData.Length;  
        List<List<Neuron>> network = new List<List<Neuron>>();
        for (int i = 0; i < layers; i++) 
        {
            int neuronCount = networkData[i];
            network.Add(new List<Neuron>());

            for (int j = 0; j < neuronCount; j++)
            {
                if(i == 0)
                {
                    network[i].Add(new InputNeuron());
                }
                else {
                    network[i].Add(new SigmoidNeuron(0, new double[network[i - 1].Count]));
                }
            }
        }
        return network;
    }
}