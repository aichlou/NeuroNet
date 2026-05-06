using System.Security.Cryptography;
using NeuroNet.Core;
using Spectre.Console;
using static NeuroNet.CLI.InputCLI;

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
            Console.WriteLine("2. Edit weights, neurons and layers manually");
            Console.WriteLine("3. Edit name of the network");
            Console.WriteLine("4. Return to main menu");
            var Result = InputCLI.ConfirmInputReturn(["1", "2", "3", "4"]);
            switch (Result.Value1)
            {
                case TriState.Return:
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
                case TriState.False:
                    Console.WriteLine("Please insert a valid entry");
                    break;
                case TriState.True:
                    switch (Result.Value2)
                    {
                        case "1":
                            bool RangeError = false;
                            do {
                                Console.WriteLine("Do you want to use the default(-1 to 1) Range or a customized? (d/c)");
                                var DefOrCu = ConfirmInputReturn(["d", "D"]);
                                switch(DefOrCu.Value1)
                                {
                                    case TriState.Return:
                                        Console.WriteLine("Returning...");
                                        Error = true;
                                        break;
                                    case TriState.True:
                                        network = Edit.RandomizeWeights(network);
                                        break;
                                    case TriState.False:
                                        if ((DefOrCu.Value2 ?? "").ToLower() == "c") {
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
                                                    //Console.WriteLine($"The Lower Border is {LowerBorder}");
                                                    double UpperBorder = double.Parse(RangeInput
                                                        .SkipWhile(c => c != ',')
                                                        .Skip(1)
                                                        .ToArray() ?? throw new Exception("Upper border could not be resolved"));
                                                    //Console.WriteLine($"The upper Border is {UpperBorder}");
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
                                            Console.WriteLine("This isn't a valid Input. Please try again...");
                                            RangeError = true;
                                        }
                                        break;
                                }
                            } while (RangeError);
                            SaveCLI.SaveNetworkToFileY(network, "overwrite", currentnnName);
                            ShowCLI.ShowNetwork(network);
                            Console.WriteLine("Weights randomized and saved to file");
                            break;
                        case "2":
                            Console.WriteLine("You can manually change weights, neurons or layers in this menu");
                            //Console.WriteLine("I personally would wait until the v.0.4.0 - Graphical Update comes and edit then");
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
                                Console.WriteLine($"Debug, vor Saven");
                                ShowCLI.ShowNetwork(network);
                                SaveCLI.SaveNetworkToFileY(network, "overwrite", currentnnName);
                            } 
                            break;
                        case "3":
                            string result = ChangeName(currentnnName);
                            if (result == "Error") Error = true;
                            else currentnnName = result;
                            break;
                        case "4":
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
                        var UserInput = InputCLI.ConfirmAndReturn();
                        if (UserInput == TriState.Return)
                        {
                            //Todo: I dont want to do this
                            Console.WriteLine("This isn't implemented so you will be redirected to the main menu...");
                        }
                        else if (UserInput == TriState.True)
                        {
                            //Console.WriteLine("Really? You want to stay in my shi**y menu?");
                            Console.WriteLine("On the way to the Edit menu...");
                            Error = true;
                        }
                        else if (UserInput == TriState.False)
                        {
                            Console.WriteLine("You will be redirected to the main menu...");
                        }
                    }
                    break;
            }
        } while (Error);

        return new MultipleValues<TwoValues<List<List<Neuron>>, string?>>
        {
            HasError = false,
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
            Console.WriteLine("Enter the number of the layer you want to edit:");
            var LayerState = InputCLI.ConfirmInputReturn([], $"numbers0,{network.Count + 10}");
            switch(LayerState.Value1)
            {
                case TriState.Return: // Retuning to Edit Menu
                    Console.WriteLine("Returning to Edit Menu...");
                    return new MultipleValues<List<List<Neuron>>>
                    {
                        HasError = true,
                        Value = network,
                        ErrorMessage = Program.returnString,
                    };
                case TriState.True: //Valid layer (Selected or Add)
                    int layerNumber = int.Parse(LayerState.Value2 ?? "0"); //Layer inputted by the user
                    if (layerNumber <= network.Count) //Existing Layer
                    {
                        Console.WriteLine($"You selected Layer {layerNumber}:");
                        ShowCLI.ShowLayer(network[layerNumber - 1]);
                        bool NeuronError = false;
                        do {
                            NeuronError = false;
                            Console.WriteLine($"This layer has {network[layerNumber - 1].Count} neurons.");
                            Console.WriteLine("Enter the number of the number you want to edit"); //Todo: Add e for editing the whole layer for things like delete, change position in network, name,etc
                            var NeuronState = InputCLI.ConfirmInputReturn(["e"], $"numbers1,{network[layerNumber - 1].Count - 1}");
                            switch (NeuronState.Value1)
                            {
                                case TriState.Return:
                                    Console.WriteLine("Returning to Layer Selection...");
                                    Error = true;
                                    break;
                                case TriState.False: //Neuron does not exist
                                    if (!int.TryParse(NeuronState.Value2, out int WeightNumber2)) {
                                        Console.WriteLine("Please type in a valid number");
                                        NeuronError = true;
                                    }
                                    else if (WeightNumber2 > network[layerNumber - 1].Count + 9 || WeightNumber2 <= 0) {
                                        Console.WriteLine("This is no valid neuron. Please type in a valid neuron.");
                                    }
                                    else 
                                    {
                                        double[] weights = network[layerNumber - 1][WeightNumber2 - 1].GetWeights();
                                        Console.WriteLine($"The neuron {WeightNumber2} is not a valid Layer because the layer is contains just {weights.Length}");
                                        Console.WriteLine($"Do you want to create {WeightNumber2 - weights.Length} neurons?(y/n)");
                                        var state = InputCLI.ConfirmAndReturn();
                                        if (state == InputCLI.TriState.True) //Create neurons
                                        {
                                            
                                        }
                                        else if (state == InputCLI.TriState.False) {} //Don't create neurons
                                        else if (state == InputCLI.TriState.Return) {} //Return
                                    }
                                    break;
                                case TriState.True:
                                    if(int.TryParse(NeuronState.Value2, out int neuronNumber))
                                    {
                                        Neuron selectedNeuron = network[layerNumber - 1][neuronNumber - 1];
                                        bool WeightsError = false;
                                        do {
                                            WeightsError = false;
                                            double[] weights = selectedNeuron.GetWeights();
                                            Console.WriteLine($"Current weights: {string.Join(", ", weights.Select(w => w.ToString("F3")))}");
                                            Console.WriteLine("Which weight do you want to change? (Type in 'all' if you want to change all)");
                                            var WeightState = InputCLI.ConfirmInputReturn(["all", "a"], $"numbers1,{weights.Length}"); //Todo: Check if begin from 0 is valid
                                            switch (WeightState.Value1)
                                            {
                                                case TriState.Return:
                                                    Console.WriteLine("Redirecting to Neuron Choice...");
                                                    NeuronError = true;
                                                    break;
                                                case TriState.False:
                                                    Console.WriteLine("Please type in a valid number or 'all'");
                                                    WeightsError = true;
                                                    break;
                                                case TriState.True:
                                                    if (WeightState.Value2 == "all" || WeightState.Value2 == "a")
                                                    {
                                                        bool AllWeightsError = false;
                                                        do {
                                                            AllWeightsError = false;
                                                            Console.WriteLine("Please type in the new weights seperated by commas"); //Todo: Maybe implement configuration for inputCLI for this
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
                                                                    double[] doubleWeights = StringWeights.Select(w => double.Parse(w)).ToArray();
                                                                    if (doubleWeights.Length == selectedNeuron.GetWeights().Length)
                                                                    {
                                                                        selectedNeuron.SetWeights(doubleWeights);
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
                                                        int WeightNumber = int.Parse(WeightState.Value2 ?? "");
                                                        bool WeightValueError = false;
                                                        do {
                                                            WeightValueError = false;
                                                            Console.WriteLine($"To what do you want to change the {WeightNumber}. weight?");
                                                            var NewWeightState = InputCLI.ConfirmInputReturn(null, "numbersd");
                                                            switch (NewWeightState.Value1)
                                                            {
                                                                case TriState.Return:
                                                                    Console.WriteLine("Returning to which weight to choose...");
                                                                    WeightsError = true;
                                                                    break;
                                                                case TriState.False:
                                                                    Console.WriteLine("Please type in a valid number.");
                                                                    WeightValueError = true;
                                                                    break;
                                                                case TriState.True:
                                                                    double NewWeightValue = double.Parse(NewWeightState.Value2 ?? "");
                                                                    double[] neuronweights = selectedNeuron.GetWeights();
                                                                    double oldValue = neuronweights[WeightNumber - 1];
                                                                    neuronweights[WeightNumber - 1] = NewWeightValue;
                                                                    selectedNeuron.SetWeights(neuronweights);
                                                                    Console.WriteLine($"The weight {WeightNumber} of neuron {neuronNumber} in layer {layerNumber} has been changed from {oldValue} to {NewWeightValue}");
                                                                    break;
                                                            }
                                                        } while (WeightValueError);
                                                    }
                                                    break;
                                            }
                                        } while (WeightsError);
                                    }
                                    else
                                    {
                                        Console.WriteLine("The edit feature isn't currently availble");
                                        Console.WriteLine("Please try again");
                                        NeuronError = true;
                                    }
                                    break;
                            }
                        } while (NeuronError);
                    }
                    else
                    {
                        Console.WriteLine($"The Layer {layerNumber} is not a valid Layer because the network is just {network.Count} layers large");
                        Console.WriteLine(layerNumber - network.Count == 1 ? $"Do you want to add {layerNumber - network.Count} layer?(y/n)" : $"Do you want to add {layerNumber - network.Count} layers?(y/n)");
                        var AddLayersState = InputCLI.ConfirmAndReturn();
                        switch(AddLayersState)
                        {
                            case TriState.Return:
                                Error = true;
                                break;
                            case TriState.True:
                                int[] NewLayers = new int[layerNumber - network.Count];
                                NewLayers = NeuronsInLayer(NewLayers, 0);
                                Console.WriteLine("Creating new layers...");
                                //Console.WriteLine($"Debug: New Layers: {string.Join(",", NewLayers)}");
                                network = Edit.AddLayers(network, NewLayers);
                                break;
                            case TriState.False:
                                Console.WriteLine($"Then please select a valid layer (1-{network.Count})");
                                Error = true;
                                break;
                        }
                    }
                    break;
                case TriState.False:
                    if (int.TryParse(LayerState.Value2, out int layerNumberValue)) {
                        if (layerNumberValue < 0)
                        {
                            Console.WriteLine("Please insert a valid, positive number");
                        }
                        else
                        {
                            Console.WriteLine("This layer does not exist.");
                            Console.WriteLine("You also cannot add more than 10 Layers at once.");
                            Console.WriteLine("Please insert a smaller number");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please insert a valid number");
                    }
                    Error = true;
                    break;
            }
        } while (Error);
        Console.WriteLine("Operation succesfully completed.");

        return new MultipleValues<List<List<Neuron>>>
        {
            Value = network,
            HasError = false,
        };
    }

    static public MultipleValues<List<List<Neuron>>> NumberLayerNeurons (List<List<Neuron>> network) //TODO
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
            //Console.WriteLine($"This is in the variable currentnnName: {currentnnName}");
            Console.WriteLine($"Changing the name from {currentnnName} to {newName}");
            string status = Edit.CloneFile(currentnnName, newName);
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
                        Console.WriteLine("Old network file does not exist.");
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

    public static int[] NeuronsInLayer(int[] NewLayers, int LayerID)
    {
        bool NeuronError = false;
        do {
            NeuronError = false;
            Console.WriteLine("How many neurons do you want in the layer?");
            var NeuronAmountState = InputCLI.ConfirmInputReturn([], "numbers1,10000");
            switch (NeuronAmountState.Value1)
            {
                case TriState.Return:
                    Console.WriteLine("Returning to boolean question...");
                    //Todo
                    break;
                case TriState.False:
                    if (int.Parse(NeuronAmountState.Value2 ?? "0") < 10000) {
                        Console.WriteLine("Please type in a valid number");
                    }
                    else
                    {
                        Console.WriteLine("There can not be more then 10000 neurons in one Layer. Please type in a smaller value");
                    }
                    NeuronError = true;
                    break;
                case TriState.True:
                    NewLayers[LayerID] = int.Parse(NeuronAmountState.Value2 ?? "0");
                    break;
            }
        } while (NeuronError);
        if (NewLayers.Length > LayerID + 1) {
            return NeuronsInLayer(NewLayers, LayerID + 1);
        }
        else
        {
            return NewLayers;
        }
    }
}