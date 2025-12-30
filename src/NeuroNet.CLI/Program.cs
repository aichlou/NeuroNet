using NeuroNet.Core;
namespace NeuroNet.CLI;

internal class Program
{
    static void Main(string[] args)
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
                    var OutputCreate = Create.CreateNeuralNetwork(() => Console.ReadLine() ?? string.Empty, Console.WriteLine);
                    LoadedNetwork = OutputCreate.Value1;
                    currentnnName = OutputCreate.Value2 ?? "MyNeuralNetwork";
                    break;
                case 2:
                    var result = Load.LoadNeuralNetwork(Console.WriteLine, () => Console.ReadLine() ?? string.Empty);
                    if (result.HasError)
                    {
                        Error = true;
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

        Extras.PressKey();
        bool wasRandomized = Edit.RandomizeIfNeeded(LoadedNetwork!, (message) => Console.WriteLine(message));
        if (wasRandomized) {
            Console.WriteLine("Do you want to save randomized weights and biases? (y/n)");
            if((Console.ReadLine() ?? string.Empty).ToLower() == "y")
            {
                Save.SaveNetwork(currentnnName, LoadedNetwork!, "overwrite", Console.WriteLine); // Hardcoded name for testing
            }
            else
            {
                Console.WriteLine("Neural Network changes not saved.");
                Console.WriteLine("If you do something with the Network in this session, you will do it with the randomized weights and biases, but they won't be saved.");
                Console.WriteLine("Continuing without saving...");
                Extras.PressKey();
            }
        }

        Console.WriteLine("Do you want to run the Neural Network now? (y/n)");
        if((Console.ReadLine() ?? string.Empty).ToLower() == "y")
        {
            double[] output = Run.Run_Network(LoadedNetwork).Value ?? throw new Exception("Loaded Network cannot be null");
            for(int i = 0; i <= output.Length; i++)
            {
                Console.Write("Debug: " + output[i]);
            }
            Console.WriteLine();
            
        }
        Console.WriteLine("Exiting Program...");
        Extras.PressKey();
    }
}
