namespace NeuroNet.Core;

public class Create {
public static TwoValues<List<List<Neuron>>, string?> CreateNeuralNetwork(Func<string> readInput, Action<string>? Message = null) //Creates a Neural Network based on user input
    {
        string baseDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appDataPath = Path.Combine(baseDataPath, "NeuroNet");
        bool Error;
        int layers = 0;
        do
        {
            Error = false;
            Message?.Invoke("Creating Neural Network...");
            Message?.Invoke("How many layers would you like your Neural Network to have? (Including input and output layers)");
            layers = int.TryParse(readInput?.Invoke() ?? string.Empty, out int parsedLayers) ? parsedLayers : 0;
            if (layers == 0)
            {
                Message?.Invoke("Please enter a valid number for the layers.");
                Error = true;
            }
            else if (layers < 2)
            {
                Message?.Invoke("A Neural Network must have at least 2 layers (input and output layers).");
                Error = true;
            }
            else if (layers > 100)
            {
                Message?.Invoke("The maximum number of layers is 100.");
                Error = true;
            }
        } while (Error);
        
        List<List<Neuron>> network = new List<List<Neuron>>();
        Message?.Invoke("Creating Neural Network...");
        for (int i = 0; i < layers; i++) 
        {
            network.Add(new List<Neuron>());
            string layerType;
            if (i == 0)
            {
                layerType = " (Input Layer)";
            }
            else if (i == layers - 1)
            {
                layerType = " (Output Layer)";
            }
            else
            {
                layerType = " (Hidden Layer)";
            }
            Message?.Invoke("How many neurons would you like in layer " + (i + 1) + layerType + "?");
            int neuronCount = 0;
            do {
                Error = false;
                neuronCount = int.TryParse(readInput?.Invoke() ?? string.Empty, out int parsedNeuronCount) ? parsedNeuronCount : 0;
                if(neuronCount <= 0)
                {
                    Message?.Invoke("Invalid neuron count, defaulting to 1 neuron? (y/n)");
                    string response = readInput?.Invoke() ?? string.Empty;
                    if(response.ToLower() == "y") {
                        Message?.Invoke("Confirmed");
                        neuronCount = 1; 
                    }
                    else {
                        Message?.Invoke("Not Confirmed");
                        Message?.Invoke("Please enter a valid number for the neurons in layer " + (i + 1) + "?");
                        Error = true;
                    }
                }
                else if(neuronCount > 100000)
                {
                    Message?.Invoke("The maximum number of neurons per layer is 100000. Please enter a valid number.");
                    Error = true;
                }
            } while (Error);
            for (int j = 0; j < neuronCount; j++)
            {
                if(i == 0)
                {
                    network[i].Add(new Neuron(0, [0]));
                }
                else {
                    network[i].Add(new Neuron(0, new double[network[i - 1].Count]));
                }
            }
        }
        Message?.Invoke("Neural Network created with " + layers + " layers.");
        Message?.Invoke("Do you want to save this Neural Network? (y/n)");
        string saveResponse = readInput?.Invoke() ?? string.Empty;
        string? nnName = null;
        if(saveResponse.ToLower() == "y")
        {
            Message?.Invoke("How do you name the Neural Network?");
            nnName = readInput?.Invoke() ?? "MyNeuralNetwork";
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var c in invalidChars)
            {
                nnName = nnName.Replace(c, '_');
            }
            Save.SaveNetwork(nnName, network, "new", Message);
            Message?.Invoke("Neural Network saved as " + nnName);
        }
        else
        {
            Message?.Invoke("Neural Network not saved.");
        }
        return new TwoValues<List<List<Neuron>>, string?> { Value1 = network, Value2 = nnName };
    }
}