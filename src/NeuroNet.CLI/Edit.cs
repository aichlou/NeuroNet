using NeuroNet.Core;
using Spectre.Console;

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

    public static MultipleValues<TwoValues<List<List<Neuron>>, string?>> Edit_Network(List<List<Neuron>> network, string currentnnName)
    {
        bool Error = false;
        do {
            Error = false;
            Console.WriteLine("Edit neural network");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Randomize all weights");
            Console.WriteLine("2. Edit weights manually");
            Console.WriteLine("3. Edit number of layers & neurons");
            Console.WriteLine("4. Edit name of the network");
            Console.WriteLine("5. Return to main menu");
            string UserOutputString = Console.ReadLine() ?? string.Empty;
            if (!int.TryParse(UserOutputString, out int UserOutput))
            {
                if (Extras.IsReturn(UserOutputString))
                {
                    Console.WriteLine("Returning to Main Menu...");
                    return new MultipleValues<TwoValues<List<List<Neuron>>, string?>>
                    {
                        HasError = true,
                        Value = new TwoValues<List<List<Neuron>>, string?>
                        {
                            Value1 = network,
                            Value2 = currentnnName,
                        },
                        ErrorMessage = Program.returnString,
                    };
                }
                Console.WriteLine("Please insert a valid number");
                Error = true;
            }
            else
            {
                network = network ?? throw new Exception("Network cannot be null");
                switch (UserOutput)
                {
                    case 1:
                        if (network == null)
                        {
                            Console.WriteLine("Cannot randomize: network is null.");
                            Error = true;
                            break;
                        }
                        bool RangeError = false;
                        do {
                            Console.WriteLine("Do you want to use the default(-1 to 1) Range or a customized? (d/c)");
                            string input = Console.ReadLine() ?? "";
                            if (input.Trim() == "") {
                                Console.WriteLine("Please trype somethinf in...");
                                RangeError = false;
                            }
                            else if (Extras.IsReturn(input))
                            {
                                Error = true;
                            }
                            else if (input.ToLower() == "d")
                            {
                                network = Edit.RandomizeWeights(network);
                            }
                            else if (input.ToLower() == "c")
                            {
                                bool CustomError;
                                do {
                                    CustomError = false;
                                    Console.WriteLine("Please type in the Range in the [lower, upper]-Format");
                                    try {
                                        string RangeInput = Console.ReadLine() ?? "";
                                        if (Extras.IsReturn(RangeInput)) RangeError = true;
                                        double LowerBorder = double.Parse(RangeInput
                                            .Reverse()
                                            .SkipWhile(c => c != ',')
                                            .Skip(1)
                                            .Reverse()
                                            .ToArray() ?? throw new Exception("Lower border could not be resolved"));
                                        double UpperBorder = double.Parse(RangeInput
                                            .SkipWhile(c => c != ',')
                                            .Skip(1)
                                            .ToArray() ?? throw new Exception("Upper border could bot be resolved"));
                                        if (LowerBorder >= UpperBorder)
                                        {
                                            Console.WriteLine("The Lower Border should be smaller than the Upper Border");
                                            Console.WriteLine("Please try again");
                                            CustomError = true;
                                        }
                                        else
                                        {
                                            network = Edit.RandomizeWeights(network, LowerBorder, UpperBorder);
                                        }
                                    }
                                    catch (Exception c)
                                    {
                                        Console.WriteLine($"Something went wrong: {c.Message}");
                                        Console.WriteLine("Please try again");
                                        CustomError = true;
                                    }
                                } while (CustomError);
                            }
                            else
                            {
                                Console.WriteLine("This is no valid Input. Please try again.");
                                RangeError = false;
                            }
                        } while (RangeError);
                        SaveCLI.SaveNetworkToFileY(network, "overwrite", currentnnName);
                        ShowCLI.ShowNetwork(network);
                        Console.WriteLine("Weights randomized and saved to file");
                        break;
                    case 2:
                        Console.WriteLine("Me (the developer) don't recommend to change the Weights here...");
                        Console.WriteLine("I personally would wait until the v.0.4.0 - Graphical Update comes and edit the weights then...");
                        Console.WriteLine("But I wish you good Luck");
                        Console.WriteLine();
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
                            Console.WriteLine("Please repeat the process");
                            Error = true;
                            break;
                        }
                        network = EditWeightsResult.Value ?? throw new Exception("Unexpected null value for network after editing weights.");
                        Error = EditWeightsResult.HasError;
                        if (!Error)
                        {
                            SaveCLI.SaveNetworkToFileY(network, "overwrite", currentnnName);
                        } 
                        break;
                    case 3:
                        network = network ?? throw new Exception("Network cannt be null");
                        MultipleValues<List<List<Neuron>>> EditNetworkResult = EditCLI.NumberLayerNeurons(network);
                        Error = EditNetworkResult.HasError;
                        if (Error)
                        {
                            if (EditNetworkResult.ErrorMessage == Program.returnString)
                            {
                                Console.WriteLine("Returning to Edit Menu...");
                                Error = true;
                                break;
                            }
                            Console.WriteLine("Error editing weights: " + EditNetworkResult.ErrorMessage);
                            Console.WriteLine("Please repeat the process");
                            Error = true;
                            break;
                        }
                        network = EditNetworkResult.Value ?? throw new Exception("Unexpected null value for network after editing weights.");
                        SaveCLI.SaveNetworkToFileY(network, "overwrite", currentnnName);
                        break;
                    case 4:
                        string result = ChangeName(currentnnName);
                        if (result == "Error") Error = true;
                        break;
                    case 5:
                        return new MultipleValues<TwoValues<List<List<Neuron>>, string?>>
                        {
                            HasError = true,
                            Value = new TwoValues<List<List<Neuron>>, string?>
                            {
                                Value1 = network,
                                Value2 = currentnnName,
                            },
                        };
                    default:
                        Console.WriteLine("Please insert one of the shown Options");
                        Error = true;
                        break;
                }
                if (!Error) {
                    Console.WriteLine("Do you want to Edit further? (y/n)");
                    string UserInupt = Console.ReadLine() ?? "";
                    if (Extras.IsReturn(UserInupt))
                    {
                        //Todo: I dont want to do this
                        Console.WriteLine("This isn't implemented yet so you will be redirected to the main menu...");
                    }
                    else if (UserInupt.ToLower() == "y")
                    {
                        //Console.WriteLine("Really? You want to stay in my shi**y menu?");
                        Console.WriteLine("On the way to the Edit menu...");
                        Error = true;
                    }
                    else if (UserInupt.ToLower() == "n")
                    {
                        Console.WriteLine("You will be redirected to the main menu...");
                    }
                    else
                    {
                        Console.WriteLine("Okay... I guess you don't want to stay here...");
                    }
                }
            }
        } while (Error);

    return new MultipleValues<TwoValues<List<List<Neuron>>, string?>>
    {
        HasError = true, //Why true?
        Value = new TwoValues<List<List<Neuron>>, string?>
        {
            Value1 = network,
            Value2 = currentnnName,
        },
        ErrorMessage = Program.returnString,
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
        ShowCLI.ShowNetwork(network);
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
                Console.WriteLine($"You selected Layer {layerNumber}:");
                ShowCLI.ShowLayer(network[layerNumber - 1]);
                bool NeuronError = false;
                do {
                    NeuronError = false;
                    Console.WriteLine($"This layer has {network[layerNumber - 1].Count} neurons.");
                    Console.WriteLine("Enter the neuron number of the layer you want to edit:");
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
                        bool WeightsError = false;
                        do {
                            WeightsError = false;
                            double[] weights = selectedNeuron.GetWeights();
                            Console.WriteLine($"Current weights: {string.Join(", ", weights.Select(w => w.ToString("F3")))}");
                            Console.WriteLine("Which weight do you want to change? (Type in 'all' if you want to change all)");
                            string UserOutput = Console.ReadLine() ?? "";
                            if (Extras.IsReturn(UserOutput))
                            {
                                Console.WriteLine("Redirecting to Neuron Choice...");
                                NeuronError = true;
                            }
                            else
                            {
                                if (UserOutput.ToLower() == "all")
                                {   
                                    bool AllWeightsError = false;
                                    do {
                                        AllWeightsError = false;
                                        Console.WriteLine("Please type in the new weights seperated by commas");
                                        string NewWeights = Console.ReadLine() ?? "";
                                        if (NewWeights == "")
                                        {
                                            Console.WriteLine("Please type in something");
                                            AllWeightsError = true;
                                        }
                                        else if (Extras.IsReturn(NewWeights))
                                        {
                                            Console.WriteLine("Returning to Neuron Choice...");
                                            NeuronError = true;
                                        }
                                        else
                                        {
                                            string[] StringWeights = NewWeights.Split(',');

                                            if (StringWeights.All(w => double.TryParse(w, out _)))
                                            {
                                                double[] doubleWeigths = StringWeights.Select(w => double.Parse(w)).ToArray();
                                                if (doubleWeigths.Length == selectedNeuron.GetWeights().Length)
                                                {
                                                    selectedNeuron.SetWeights(doubleWeigths);
                                                    Console.WriteLine("Succesfully changed Neurons Weights");
                                                }
                                                else
                                                {
                                                    Console.WriteLine($"Please insert {selectedNeuron.GetWeights().Length} weights");
                                                    AllWeightsError = true;
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Please give only valid weights (No Text), seperated by commas");
                                                AllWeightsError = true;
                                            }
                                        }
                                    } while (AllWeightsError);
                                }
                                else
                                {
                                    if (int.TryParse(UserOutput, out int WeightNumber))
                                    {
                                        if (WeightNumber <= weights.Length)
                                        {
                                            bool WeightValueError = false;
                                            do {
                                                WeightValueError = false;
                                                Console.WriteLine($"To what do you want to change the {WeightNumber}. weight?");
                                                string NewWeightString = Console.ReadLine() ?? "";
                                                if (Extras.IsReturn(NewWeightString))
                                                {
                                                    Console.WriteLine("Returning to which weight to choose...");
                                                    WeightsError = true;
                                                }
                                                else if (double.TryParse(NewWeightString, out double NewWeightValue))
                                                {
                                                    double[] neuronweights = selectedNeuron.GetWeights();
                                                    neuronweights[WeightNumber - 1] = NewWeightValue;
                                                    selectedNeuron.weights = neuronweights;
                                                    Console.WriteLine($"The weight {WeightNumber} of neuron {neuronNumber} in the layer {layerNumber} have now been changed from {selectedNeuron.GetWeights()[WeightNumber - 1]} to {NewWeightValue}");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Please type in a valid number.");
                                                    WeightValueError = true;
                                                }
                                            } while (WeightValueError);
                                        }
                                        else
                                        {
                                            Console.WriteLine("This neuron doesn't exist, please choose one, who exists");
                                            WeightsError = true;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Please type in a valid number or 'all'");
                                        WeightsError = true;
                                    }
                                }
                            }
                        } while (WeightsError);
                    }
                } while (NeuronError);
            }
        } while (Error);
        Console.WriteLine("Operation succesfully completed.");

        return new MultipleValues<List<List<Neuron>>>
        {
            Value = network,
            HasError = false,
        };
    }

    static public MultipleValues<int[,]> NavigateThrowNetwork()
    {
        return new MultipleValues<int[,]>
        {
            HasError = true
        };
    }


    static public MultipleValues<List<List<Neuron>>> NumberLayerNeurons (List<List<Neuron>> network)
    {
        ShowCLI.ShowNetwork(network);


        return new MultipleValues<List<List<Neuron>>>
        {
            Value = network,
            HasError = true
        };
    }

    static public string ChangeName(string currentnnName)
    {
        
        Console.WriteLine($"The current Name of the Network is {currentnnName}");
        Console.WriteLine("To what do you want to change the name your network?");
        string newName = Console.ReadLine() ?? "";
        if (Extras.IsReturn(newName) || newName == "Error")
        {
            Console.WriteLine("Returning to Edit Menu");
            return "Error";
        }
        if (newName == "")
        {
            Console.WriteLine("Please type something in");
            return ChangeName(currentnnName);
        }
        else
        {
            Console.WriteLine($"This is in the variable currentnnName: {currentnnName}");
            string status = Edit.CloneFile(currentnnName, newName, currentnnName);
            Console.WriteLine($"Cloning target is: {currentnnName}");
            Console.WriteLine($"Status: {status}");
            if (status.Contains("Error")) {
                Console.WriteLine("Something went wrong");
                return "Error";
            }
            else if (status == "done") {
                string Message = Save.DeleteFile(currentnnName);
                switch (Message)
                {
                    case "done":
                        break;
                    case "file not found":
                        Console.WriteLine("File does not exist.");
                        break;
                    default:
                        Console.WriteLine("An error occurred. Please report the issue on GitHub.");
                        break;
                }
                return newName;
            }
            else throw new Exception("Something went wrong, Please try again");
        }
    }
}