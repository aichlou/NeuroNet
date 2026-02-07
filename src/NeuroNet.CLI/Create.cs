using System.Net.NetworkInformation;
using NeuroNet.Core;

namespace NeuroNet.CLI;

public class CreateCLI
{
    public static TwoValues<List<List<Neuron>>, string?> CreatingProcess()
    {
        bool Error = false;
        int layers;
        int[] networkData;
        do
        {
            Error = false;
            MultipleValues<int> layerCountResult = LayerCount();
            layers = layerCountResult.Value;
            if (layerCountResult.HasError)
            {
                return new TwoValues<List<List<Neuron>>, string?> 
                {
                    Value1 = null,
                    Value2 = layerCountResult.ErrorMessage
                };
            }


            networkData = new int[layers];
            NeuronCountForLayer(networkData);
            if (networkData[0] == 0) //Network Creation was exited with the Return-Keyword
            {
                Error = true;
            }
        } while (Error);
        /*for (int i = 0; i < layers; i++)
        {
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

            Console.WriteLine("How many neurons would you like in layer " + (i + 1) + layerType + "?");
            do {
                Error = false;
                string UserOutput = Console.ReadLine() ?? string.Empty;
                int neuronCount = int.TryParse(UserOutput, out int parsedNeuronCount) ? parsedNeuronCount : 0;
                if(neuronCount <= 0)
                {
                    if(Extras.isReturn(UserOutput))
                    {
                        
                    }
                    Console.WriteLine("Invalid neuron count, defaulting to 1 neuron? (y/n)");

                    string response = Console.ReadLine() ?? string.Empty;
                    if(response.ToLower() == "y") {
                        Console.WriteLine("Confirmed");
                        neuronCount = 1; 
                    }
                    else {
                        Console.WriteLine("Not Confirmed");
                        Console.WriteLine("Please enter a valid number for the neurons in layer " + (i + 1) );
                        Error = true;
                    }
                }
                else if(neuronCount > 100000)
                {
                    Console.WriteLine("The maximum number of neurons per layer is 100000. Defaulting to 1000000? (y/n)");
                    string response = Console.ReadLine() ?? string.Empty;
                    if(response.ToLower() == "y") {
                        Console.WriteLine("Confirmed");
                        neuronCount = 1; 
                    }
                    else {
                        Console.WriteLine("Not Confirmed");
                        Console.WriteLine("Please enter a valid number for the neurons in layer " + (i + 1) );
                        Error = true;
                    }
                }
                else
                {
                    networkData[i] = neuronCount;
                }
            } while (Error);
        } */
        Console.WriteLine("Creating Neural Network...");
        List<List<Neuron>> network;
        try {
            network = Create.CreateNeuralNetwork(networkData);
        }
        catch(Exception ex)
        {
            Console.WriteLine("An error occurred while creating the Neural Network: " + ex.Message);
            //Todo: Error handling with GitHub Issue Reporting
            Console.WriteLine("Please try again.");
            Console.WriteLine();
            return CreatingProcess(); //When the user types the return Keyword in this Process the program will return to the main menu, but is that what the program should do?
        }
        Console.WriteLine("Neural Network created with " + layers + " layers.");
        return new TwoValues<List<List<Neuron>>, string?> 
        {
            Value1 = network,
            Value2 = null
        };
    }

    public static MultipleValues<int> LayerCount()
    {
        bool Error;
        int layers = 0;
        do
        {
            Error = false;
            Console.WriteLine("How many layers would you like your Neural Network to have? (Including input and output layers)");
            string UserOutput = Console.ReadLine() ?? string.Empty;
            layers = int.TryParse(UserOutput, out int parsedLayers) ? parsedLayers : 0;
            if (layers == 0)
            {
                if(Extras.isReturn(UserOutput))
                {
                    Console.WriteLine("Exiting Neural Network Creation...");
                    var result = new MultipleValues<int>
                    {
                        HasError = true,
                        ErrorMessage = Program.returnString
                    };
                    return result;
                }
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
        return new MultipleValues<int>{
            Value = layers,
            HasError = false,
            ErrorMessage = null
        };
    }
    public static int[] NeuronCountForLayer(int[] networkData)
    {
        int layer = 0;
        int layers = networkData.Length;
        for(int i = 0; i < networkData.Length; i++)
        {
            if (networkData[i] == 0)
            {
                layer = i;
                break;
            }
            else if (i == networkData.Length - 1)
            {
                return networkData;
            }
        }
        string layerType;
        if (layer == 0)
        {
            layerType = " (Input Layer)";
        }
        else if (layer == layers - 1)
        {
            layerType = " (Output Layer)";
        }
        else
        {
            layerType = " (Hidden Layer)";
        }

        Console.WriteLine("How many neurons would you like in layer " + (layer + 1) + layerType + "?");
        bool Error;
        do {
            Error = false;
            string UserOutput = Console.ReadLine() ?? string.Empty;
            int neuronCount = int.TryParse(UserOutput, out int parsedNeuronCount) ? parsedNeuronCount : 0;
            if(neuronCount <= 0)
            {
                if(Extras.isReturn(UserOutput))
                {
                    if (layer != 0) {
                        networkData[layer -1] = 0;
                    }
                    else
                    {
                        Console.WriteLine("Exiting Neural Network Creation...");
                        return networkData; //TODO HIER MEITER MACHEN
                    }
                    Console.WriteLine("Go Back One Layer...");
                    //Todo: Print last network state
                    NeuronCountForLayer(networkData);
                    return networkData;
                }
                Console.WriteLine("Invalid neuron count, defaulting to 1 neuron? (y/n)");

                string response = Console.ReadLine() ?? string.Empty;
                if(response.ToLower() == "y") {
                    Console.WriteLine("Confirmed");
                    neuronCount = 1; 
                }
                else {
                    Console.WriteLine("Not Confirmed");
                    Console.WriteLine("Please enter a valid number for the neurons in layer " + (layer + 1) );
                    Error = true;
                }
            }
            else if(neuronCount > 100000)
            {
                Console.WriteLine("The maximum number of neurons per layer is 100000. Defaulting to 1000000? (y/n)");
                string response = Console.ReadLine() ?? string.Empty;
                if(response.ToLower() == "y") {
                    Console.WriteLine("Confirmed");
                    neuronCount = 1; 
                }
                else {
                    Console.WriteLine("Not Confirmed");
                    Console.WriteLine("Please enter a valid number for the neurons in layer " + (layer + 1) );
                    Error = true;
                }
            }
            else
            {
                networkData[layer] = neuronCount;
            }
        } while (Error);
        NeuronCountForLayer(networkData);
        return networkData;
    }
}