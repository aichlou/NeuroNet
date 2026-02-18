using NeuroNet.Core;

namespace NeuroNet.CLI;

class EditCLI
{
    public static List<List<Neuron>> RandomizeIfNeeded(List<List<Neuron>> network, string currentnnName)
    {
        if (network == null)
        {
            Console.WriteLine("Network is null; cannot randomize weights.");
            return new List<List<Neuron>>();
        }
        bool allWeightsZero = Edit.allWeightsZero(network);
        if (allWeightsZero) {
            network = Edit.RandomizeWeights(network);
            Console.WriteLine("The weights were randomized since they were all null");
            SaveCLI.SaveNetworkToFileY(network, "overwrite", currentnnName);
        }
        return network;
    }

    public static MultipleValues<List<List<Neuron>>> Edit_Network(List<List<Neuron>> network, string currentnnName)
    {
        bool Error = false;
        do {
            Console.WriteLine("Edit Neural Network");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Randomize Weights");
            Console.WriteLine("2. Edit Weights Manually");
            Console.WriteLine("3. Edit Number of Layers & Neurons");
            Console.WriteLine("4. Edit Name of the Network");
            //Console.WriteLine("4. Edit Neuron Types");
            string UserOutputString = Console.ReadLine() ?? string.Empty;
            if (!int.TryParse(UserOutputString, out int UserOutput))
            {
                if (Extras.IsReturn(UserOutputString))
                {
                    Console.WriteLine("Returning to Main Menu...");
                    return new MultipleValues<List<List<Neuron>>>
                    {
                        HasError = true,
                        Value = network,
                        ErrorMessage = Program.returnString,
                    };
                }
                Console.WriteLine("Please insert a valid number");
                Error = true;
            }
            else
            {
                switch (UserOutput)
                {
                    case 1:
                        if (network == null)
                        {
                            Console.WriteLine("Cannot randomize: network is null.");
                            Error = true;
                            break;
                        }
                        network = Edit.RandomizeWeights(network);
                        SaveCLI.SaveNetworkToFileY(network, "overwrite", currentnnName);
                        Console.WriteLine("Weights randomized and saved to file");
                        break;
                    case 2:
                        MultipleValues<List<List<Neuron>>> EditWeightsResult = EditCLI.EditWeightsManually(network);
                        if (EditWeightsResult.HasError)
                        {
                            if (EditWeightsResult.ErrorMessage == Program.returnString)
                            {
                                Console.WriteLine("Returning to Edit Menu...");
                                Error = true;
                                break;
                            }
                            Console.WriteLine("Error editing weights: " + EditWeightsResult.ErrorMessage);
                            Error = true;
                            break;
                        }
                        network = EditWeightsResult.Value ?? throw new Exception("Unexpected null value for network after editing weights.");
                        Error = EditWeightsResult.HasError;
                        if (!Error)
                        {
                            SaveCLI.SaveNetworkToFileY(network, "overwrite", currentnnName);
                            Console.WriteLine("Weights edited and saved to file");
                        } 
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    default:
                        Console.WriteLine("Please insert one of the shown Options");
                        Error = true;
                        break;
                }
            }
        } while (Error);



        return new MultipleValues<List<List<Neuron>>>
        {
          Value = network,
          HasError = false,
        };
    }

    public static MultipleValues<List<List<Neuron>>> EditWeightsManually(List<List<Neuron>>? network)
    {
        if (network == null)
        {
            Console.WriteLine("Cannot edit weights: network is null.");
            return new MultipleValues<List<List<Neuron>>>
            {
                HasError = true,
                ErrorMessage = "Network is null."
            };
        }
        ShowCLI.ShowWeights(network);
        bool Error;
        do {
            Error = false;
            Console.WriteLine("Enter the layer number of the weight you want to edit:");
            string? layerInput = Console.ReadLine();
            if (!int.TryParse(layerInput, out int layerNumber) || layerNumber < 1 || layerNumber > network.Count)
            {
                if (Extras.IsReturn(layerInput))
                {
                    Console.WriteLine("Returning to Edit Menu...");
                    return new MultipleValues<List<List<Neuron>>>
                    {
                        HasError = true,
                        Value = network,
                        ErrorMessage = Program.returnString,
                    };
                }
                Console.WriteLine("Invalid layer number.");
                Error = true;
            }
            else
            {
                Console.WriteLine($"You selected Layer {layerNumber}");
                bool NeuronError = false;
                do {
                    Console.WriteLine($"This layer has {network[layerNumber - 1].Count} neurons.");
                    Console.WriteLine("Enter the neuron number of the weight you want to edit:");
                    string? neuronInput = Console.ReadLine();
                    if (!int.TryParse(neuronInput, out int neuronNumber) || neuronNumber < 1 || neuronNumber > network[layerNumber - 1].Count)
                    {
                        if (Extras.IsReturn(neuronInput))
                        {
                            Console.WriteLine("Returning to Layer Selection...");
                            Error = true;
                        }
                        else {
                            Console.WriteLine("Invalid neuron number.");
                            NeuronError = true;
                        }
                    }
                    else
                    {
                        Neuron selectedNeuron = network[layerNumber - 1][neuronNumber - 1];
                        double[] weights = selectedNeuron.GetWeights();
                        Console.WriteLine($"Current weights: {string.Join(", ", weights.Select(w => w.ToString("F2")))}");
                        Console.WriteLine("Which wight do you want to change?");
                        string UserOutput = Console.ReadLine() ?? "";
                        //Todo: Code further but Im unhappy with this Code
                    }
                } while (NeuronError);
            }
        } while (Error);


        return new MultipleValues<List<List<Neuron>>>
        {
            Value = network,
            HasError = false,
        };
    }
    }