using System.Data;
using System.Text.Json;

namespace NeuroNet;

public static class Main
{

public static List<List<Neuron>> CreateNeuralNetwork()
    {
        string baseDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appDataPath = Path.Combine(baseDataPath, "NeuroNet");
        bool Error;
        int layers = 0;
        do
        {
            Error = false;
            Console.WriteLine("How many layers would you like your Neural Network to have? (Including input and output layers)");
            layers = int.TryParse(Console.ReadLine(), out int parsedLayers) ? parsedLayers : 0;
            if (layers == 0)
            {
                Console.WriteLine("Please enter a valid number for the layers.");
                Error = true;
            }
            else if (layers < 2)
            {
                Console.WriteLine("A Neural Network must have at least 2 layers (input and output layers).");
                Error = true;
            }
            else if (layers > 100)
            {
                Console.WriteLine("The maximum number of layers is 100.");
                Error = true;
            }
        } while (Error);
        
        List<List<Neuron>> network = new List<List<Neuron>>();
        Console.WriteLine("Creating Neural Network...");
        for (int i = 0; i < layers; i++) 
        {
            network.Add(new List<Neuron>());
            Console.Write("How many neurons would you like in layer " + (i + 1) + "?");
            if (i == 0)
            {
                Console.WriteLine(" (Input Layer)");
            }
            else if (i == layers - 1)
            {
                Console.WriteLine(" (Output Layer)");
            }
            else
            {
                Console.WriteLine();
            }
            int neuronCount = 0;
            do {
                Error = false;
                neuronCount = int.TryParse(Console.ReadLine(), out int parsedNeuronCount) ? parsedNeuronCount : 0;
                if(neuronCount <= 0)
                {
                    Console.WriteLine("Invalid neuron count, defaulting to 1 neuron? (y/n)");
                    string response = Console.ReadLine() ?? string.Empty;
                    if(response.ToLower() != "y") {
                        Console.WriteLine("Confirmed");
                        neuronCount = 1; 
                    }
                    else {
                        Console.WriteLine("Not Confirmed");
                        Console.WriteLine("Please enter a valid number for the neurons in layer " + (i + 1) + "?");
                        Error = true;
                    }
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
        Console.WriteLine("Neural Network created with " + layers + " layers.");
        Console.WriteLine("Do you want to save this Neural Network? (y/n)");
        string saveResponse = Console.ReadLine() ?? string.Empty;
        if(saveResponse.ToLower() == "y")
        {
            Console.WriteLine("How do you name the Neural Network?");
            string nnName = Console.ReadLine() ?? "MyNeuralNetwork";
            List<List<NeuronDto>> dtoNetwork = new List<List<NeuronDto>>();
            foreach(var layer in network)
            {
                List<NeuronDto> dtoLayer = new List<NeuronDto>();
                foreach(var neuron in layer)
                {
                    dtoLayer.Add(neuron.ToDto());
                }
            }
            string contentJson = JsonSerializer.Serialize(dtoNetwork);
            string versionInfo =
                    JsonSerializer
                        .Deserialize<Dictionary<string, string>>(
                            File.ReadAllText("../../../AppVersion.json")
                        )?["version"]
                    ?? "Unknown Version";
            var metadata = new
            {
                Name = nnName,
                Type = "Default",
                Version = 0.1,
                Description = "A Neural Network created with NeuroNet.",
                GitHub = "https://github.com/aichlou/NeuroNet",
                VersionOfProramm = versionInfo,
                CreatedAt = DateTime.UtcNow,
            };

            string metadataJson = JsonSerializer.Serialize(metadata);
            string combinedJson = "{ \"Metadata\": " + metadataJson + ", \"Network\": " + contentJson + " }";
            Filemanagement.SaveNetworkToFile(nnName, combinedJson);
        }
        else
        {
            Console.WriteLine("Neural Network not saved.");
        }
        return network;
    }
}