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
            Error = false;
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
                        Console.WriteLine("Me (the developer) don't recommend to change the Weights here...");
                        Console.WriteLine("I personally would wait until the v.0.5.0 - Graphical Update comes and edit the weights then...");
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
                        Console.WriteLine("This isn't implemented yet");
                        //Todo add Number of Layers or Neurons
                        break;
                    case 4:
                        Console.WriteLine("This isn't implemented yet");
                        //Todo: Change Name of the network
                        break;
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
                ShowCLI.ShowLayer(network[layerNumber]);
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
                            Console.WriteLine($"Current weights: {string.Join(", ", weights.Select(w => w.ToString("F2")))}"); //Implement that the number is shown automatically
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
                                        if (NewWeights == "") //I thing thats a bit useless but it's not bad either
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
                                                    //Todo: The program should show the previous entry. The user should be able to edit this entry
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
                                                    Console.WriteLine("Because the dev team want's to give the power to the user (Wrong), you can give the weigth every value you want to");
                                                    Console.WriteLine("Thats acually wrong but its 1 o'clock and I am not motivated to code limitations Lucky for you:)");
                                                    bool stupidError = false;
                                                    do {
                                                        stupidError = false;
                                                        Console.WriteLine($"The weight {WeightNumber} of neuron {neuronNumber} in the layer {layerNumber} will now be changed from {selectedNeuron.GetWeights()[WeightNumber - 1]} to {NewWeightValue}");
                                                        Console.WriteLine("Please press Enter to Confirm or type in 'Return' to reject");
                                                        string Banana = Console.ReadLine() ?? "";
                                                        if (Extras.IsReturn(Banana))
                                                        {
                                                            Console.WriteLine("Return to weight value selection...");
                                                        }
                                                        else if (Banana == "")
                                                        {
                                                            Console.WriteLine("Confirmed");
                                                            double[] neuronweights = selectedNeuron.GetWeights();
                                                            neuronweights[WeightNumber - 1] = NewWeightValue;
                                                            selectedNeuron.weights = neuronweights;
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Why do you do that???");
                                                            Console.WriteLine("Do you think I have time to code for you?");
                                                            Console.WriteLine("NO! I have not! (But I do it anyways because... idk... why do i make this??)");
                                                            Console.WriteLine("Forget about this");
                                                            Console.WriteLine("Try it again...");
                                                            stupidError = true;
                                                        }
                                                    } while (stupidError);
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
}