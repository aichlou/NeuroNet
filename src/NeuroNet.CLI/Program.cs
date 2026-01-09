using System.Net.NetworkInformation;
using NeuroNet.Core;
namespace NeuroNet.CLI;

internal class Program
{
    public static void Main(string[] args)
    {
        int UserOutput;
        bool Error;
        List<List<Neuron>>? LoadedNetwork = null;
        string currentnnName = "MyNeuralNetwork";
        do
        {
            Error = false;
            Console.Clear();
            Console.WriteLine("NeuroNet");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1. Create a NeuralNetwork");
            Console.WriteLine("2. Load a NeuralNetwork");
            if (!int.TryParse(Console.ReadLine() ?? string.Empty, out UserOutput))
            {
                Console.WriteLine("Please type in a valid number");
                Error = true;
            }
            switch (UserOutput)
            {
                case 1:
                    LoadedNetwork = CreateCLI.CreatingProcess() ?? throw new Exception("Loaded Network cannot be null");
                    currentnnName = SaveCLI.SaveNetworkToFile(LoadedNetwork, "new");
                    break;
                case 2:
                    var result = LoadCLI.LoadNeuralNetwork();
                    if (result.HasError)
                    {
                        Error = true;
                        Extras.PressKey();
                    }
                    else
                    {
                        LoadedNetwork = (result.Value ?? throw new Exception("Network and network name cannot be null")).Value1 ?? throw new Exception("Network cannot be null");
                        currentnnName = result.Value.Value2 ?? throw new Exception("Network name cannot be null");
                    }
                    break;
                default:
                    Console.WriteLine("Please type in one of the shown options");
                    Error = true;
                    break;
            }
        } while (Error);
        if (LoadedNetwork is null) throw new InvalidOperationException("LoadedNetwork must not be null here");

        Extras.PressKey();
        LoadedNetwork = EditCLI.RandomizeIfNeeded(LoadedNetwork, currentnnName);
        UserOutput = 0;
        do {
            Error = false;
            Console.WriteLine();
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Run the Neural Network");
            Console.WriteLine("2. Let the Neural Network learn");
            if (!int.TryParse(Console.ReadLine(), out UserOutput))
            {
                Console.WriteLine("Please type in a valid number");
                Error = true;
            }
            else
            {
                LoadedNetwork = LoadedNetwork?? throw new Exception("Loaded Netwok cannot be null");
                switch(UserOutput)
                {
                    case 1:
                        double[]? output = RunCLI.Run_Network(LoadedNetwork).Value  ?? throw new Exception("Network Output cannot be null"); //No Error handeling
                        for(int i = 0; i < output.Length; i++)
                        {
                            Console.WriteLine($"Neuron {i + 1}: {output[i]}");
                        }
                        break;
                    case 2:
                        Console.WriteLine("This feature is in the working process...");
                        Console.WriteLine("CAREFUL: This feature isn't working yet");
                        try {
                            Learn.UserDialoge(LoadedNetwork);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"You Exited the Learning Process with the Exception {e.Data}");
                        }
                        Console.WriteLine("You Exited the Learning Process Sucessfully");
                        break;
                    default:
                        Console.WriteLine("Please insert one of the shown Options");
                        Error = true;
                        break;
                }
            }
            Console.WriteLine("Exiting Program...");
            Extras.PressKey();
        }
        while (Error);
    }
}
