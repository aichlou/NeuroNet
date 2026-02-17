using NeuroNet.Core;

namespace NeuroNet.CLI;

class SaveCLI
{
    public static string SaveNetworkToFile(List<List<Neuron>> network, string status, string? currentnnName = null)
    {
        bool Error = false;
        do
        {
            Error = false;
            string nnName = "";
            string saveResponse;
            switch (status) {
                case "new":
                    try {
                        Console.WriteLine("How do you name the Neural Network?");
                        do {
                            nnName = Console.ReadLine()!;
                            if(string.IsNullOrEmpty(nnName)) Console.WriteLine("Please name your Network Properly");
                            else if (Extras.IsReturn(nnName))
                            {
                                Console.WriteLine("Exiting Save Process...");
                                return Program.returnString;
                            }
                        }
                        while (string.IsNullOrWhiteSpace(nnName));
                        var invalidChars = Path.GetInvalidFileNameChars();
                        foreach (var c in invalidChars)
                        {
                            nnName = nnName.Replace(c, '_');
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("An error occurred while reading the Neural Network name. Neural Network not saved.");
                        return "Error";
                    }
                    break;
                case "overwrite":
                    Console.WriteLine("Do you want to save changes to a file? (y/n)");
                    saveResponse = Console.ReadLine() ?? string.Empty;
                    if(saveResponse.ToLower() == "y") {
                        nnName = currentnnName ?? throw new Exception("No current Network loaded");
                    }
                    else
                    {
                        if (Extras.IsReturn(saveResponse))
                        {
                            Console.WriteLine("This isn't implemented yet"); //TODO: Implement this
                            Console.WriteLine("If you are seeing this, pleasese report this issue on GitHub, because this should never happen");
                        }
                        Console.WriteLine("Neural Network not saved.");
                        return "NoName";
                    }
                    break;
                default:
                    Console.WriteLine("Something went wrong. Please report an Issue on GitHub and restart the program");
                    throw new Exception();
            }
            nnName = SaveNetworkToFileY(network, status, nnName);
            if (nnName == "Error") Error = true;
            else return nnName;
        } while (Error);
        return "Error";
    }

    public static string SaveNetworkToFileY(List<List<Neuron>> network, string status, string nnName)
    {
        string Message = Save.SaveNetwork(nnName, network, status);
        switch(Message)
        {
            case "done":
                Console.WriteLine("Neural Network successfully saved as " + nnName);
                break;
            case "already existing":
                Console.WriteLine("Neural Network already exists.");
                Console.WriteLine("Do you want to overwrite the existing Neural Network? (y/n)");
                string UserResponse = Console.ReadLine() ?? string.Empty;
                if (UserResponse.ToLower() == "y")
                {
                    Save.SaveNetwork(nnName, network, "overwrite");
                    Console.WriteLine("Neural Network successfully overwritten as " + nnName);
                }
                else
                {
                    return "Error";
                }
                break;
            default:
                Console.WriteLine("An error occurred. Please report the issue on GitHub.");
                break;
        }
        return nnName;
    }
}