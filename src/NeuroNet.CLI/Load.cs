namespace NeuroNet.CLI;
using NeuroNet.Core;

class LoadCLI {
    public static MultipleValues<TwoValues<List<List<Neuron>>, string?>> LoadNeuralNetwork()
    {
        Console.WriteLine("Loading Neural Network");
        string[]? networks = Load.ListSavedNetworks();
        if (networks == Array.Empty<string>() || networks == null)
        {
            Console.WriteLine("The Directory where the Networks should be located does not exist.");
            Console.WriteLine("Do you want to load the File from another Directory?");
            Console.WriteLine("If yes, its not implemented yet, so just create a Network and the Directroy will be created automatically.");
            Extras.PressKey();
            return new MultipleValues<TwoValues<List<List<Neuron>>, string?>>
            {
                Value = new TwoValues<List<List<Neuron>>, string?> { Value1 = null},
                HasError = true,
                ErrorMessage = "Failed to load Neural Network."
            };
        }
        if (networks.Length == 0)
        {
            Console.WriteLine("There is no Network located in the Directory");
            return new MultipleValues<TwoValues<List<List<Neuron>>, string?>>
            {
                Value = new TwoValues<List<List<Neuron>>, string?> { Value1 = null},
                HasError = true,
                ErrorMessage = "Failed to load Neural Network."
            };
        }
        Console.WriteLine("The Networks in your Directroy:");
        for(int i = 0; i < networks.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {networks[i]}");            
        }
        Console.WriteLine("Please type in the name of the Neural Network you would like to load:");
        string? nnName = Console.ReadLine();
        nnName = Load.NameOf(nnName);
        string networkData = Load.ContentOf(nnName);
        if (string.IsNullOrEmpty(networkData))
        {
            Console.WriteLine("Failed to load Neural Network.");
            return new MultipleValues<TwoValues<List<List<Neuron>>, string?>>
            {
                Value = new TwoValues<List<List<Neuron>>, string?> { Value1 = null},
                HasError = true,
                ErrorMessage = "Failed to load Neural Network."
            };
        }
        List<List<Neuron>>? network = Load.JsonToList(networkData);

        if (network == null)
        {
            Console.WriteLine("Failed to convert Neural Network DTO to Neurons.");
            return new MultipleValues<TwoValues<List<List<Neuron>>, string?>>
            {
                Value = null!,
                HasError = true,
                ErrorMessage = "Conversion Error."
            };
        }
        if (network.Count == 0)
        {
            Console.WriteLine("The Network Data of your File is empty. Please select another file");
            return new MultipleValues<TwoValues<List<List<Neuron>>, string?>>
            {
                Value = null!,
                HasError = true,
                ErrorMessage = "File is empty"
            };
        }
        network = Edit.adjustweights(network);
        Console.WriteLine("Neural Network Structure:");
        foreach (var layer in network!)
        {
            Console.WriteLine("Layer with " + layer.Count + " neurons.");
        }
         return new MultipleValues<TwoValues<List<List<Neuron>>, string?>>
        {
            Value = new TwoValues<List<List<Neuron>>, string?> { Value1 = network, Value2 = nnName},
            HasError = false,
            ErrorMessage = string.Empty
        };
    }
}