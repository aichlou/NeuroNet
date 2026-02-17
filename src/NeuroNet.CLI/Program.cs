using System.Net.NetworkInformation;
using NeuroNet.Core;
namespace NeuroNet.CLI;

internal class Program
{
    public const string returnString = "return";
    public static void Main(string[] args)
    {
        bool again = false;
        string? Way = null;
        TwoValues<List<List<Neuron>>, string?> CreationResult = default!;
        do {
            int UserOutput;
            bool Error;
            List<List<Neuron>>? LoadedNetwork = null;
            string currentnnName = "MyNeuralNetwork";
            do
            {
                Error = false;
                if (!again) {
                    Console.Clear();
                    Console.WriteLine("NeuroNet");
                    Console.WriteLine("What would you like to do?");
                    Console.WriteLine("1. Create a NeuralNetwork");
                    Console.WriteLine("2. Load a NeuralNetwork");
                    string UserOutputString = Console.ReadLine() ?? string.Empty;
                    if (!int.TryParse(UserOutputString, out UserOutput))
                    {
                        if (Extras.IsReturn(UserOutputString))
                        {
                            Console.WriteLine("Exiting Program...");
                            Error = true;
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Please type in a valid number");
                            Extras.PressKey();
                            Error = true;
                        }
                    }
                }
                else
                {
                    switch (Way)
                    {
                        case "Create":
                            UserOutput = 1;
                            break;
                        case "Load":
                            UserOutput = 2;
                            break;
                        default:
                            Console.WriteLine("This should never happen, but if it does, just restart the program & Report on GitHub.");
                            return;
                    }
                }
                switch (UserOutput)
                {
                    case 1:
                        Way = "Create";
                        bool repeat;
                        if (again) repeat = true;
                        else repeat = false;
                        do {
                            if (repeat)
                            {
                                CreationResult = CreateCLI.CreatingProcess(CreationResult);
                            }
                            else {
                                CreationResult = CreateCLI.CreatingProcess();
                            }
                            repeat = false;
                            if (CreationResult.Value2 == returnString)
                            {
                                Console.WriteLine("Exiting Neural Network Creation...");
                                Error = true;
                            }
                            else {
                                LoadedNetwork = CreationResult.Value1 ?? throw new Exception("Loaded Network cannot be null");
                                currentnnName = SaveCLI.SaveNetworkToFile(LoadedNetwork, "new");
                                if (currentnnName.ToLower() == returnString.ToLower()) repeat = true;
                            }
                        }
                        while(repeat);
                        break;
                    case 2:
                        Way = "Load";
                        var result = LoadCLI.LoadNeuralNetwork();
                        if (result.HasError)
                        {
                            Error = true;
                            if (result.ErrorMessage != "User exited load process.") Extras.PressKey();
                        }
                        else
                        {
                            LoadedNetwork = (result.Value ?? throw new Exception("Network and network name cannot be null")).Value1 ?? throw new Exception("Network cannot be null");
                            currentnnName = result.Value.Value2 ?? throw new Exception("Network name cannot be null");
                        }
                        break;
                    default:
                        if(!Error) {
                            Console.WriteLine("Please type in one of the shown options");
                            Extras.PressKey();
                            Error = true;
                        }
                        break;
                }
                again = false;
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
                string UserOutputString = Console.ReadLine() ?? string.Empty;
                if (!int.TryParse(UserOutputString, out UserOutput))
                {
                    if (Extras.IsReturn(UserOutputString)) again = true;
                    else {
                        Console.WriteLine("Please type in a valid number");
                        Error = true;
                    }
                }
                else
                {
                    LoadedNetwork = LoadedNetwork?? throw new Exception("Loaded Netwok cannot be null");
                    switch(UserOutput)
                    {
                        case 1:
                            var runResult = RunCLI.Run_Network(LoadedNetwork);
                            if (runResult.HasError)
                            {
                                if (runResult.ErrorMessage == "User exited run process.") Error = true;
                                else Error = true;
                            }
                            else {
                            double[]? output = runResult.Value  ?? throw new Exception("Network Output cannot be null"); //No Error handeling
                            for(int i = 0; i < output.Length; i++)
                            {
                                Console.WriteLine($"Neuron {i + 1}: {output[i]}");
                            }
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
            }
            while (Error);
        } while (again);
        Console.WriteLine("Exiting Program...");
        Extras.PressKey();
    }
}
