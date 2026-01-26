using NeuroNet.Core;

namespace NeuroNet.CLI;

class SaveCLI
{
    public static string SaveNetworkToFile(List<List<Neuron>> network, string status, string? currentnnName = null)
    {
        string nnName = "";
        string saveResponse;
        switch (status) {
            case "new":
                Console.WriteLine("Do you want to save this Neural Network to a file? (y/n)");
                saveResponse = Console.ReadLine() ?? string.Empty;
                if(saveResponse.ToLower() == "y") {
                    Console.WriteLine("How do you name the Neural Network?");
                    try {
                    do
                    {
                        nnName = Console.ReadLine()!;
                        if(string.IsNullOrEmpty(nnName)) Console.WriteLine("Please name your Network Properly");
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
                        return "NoName";
                    }
                }
                else
                {
                Console.WriteLine("Neural Network not saved.");
                return "NoName";
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
                    Console.WriteLine("Neural Network not saved.");
                    return "NoName";
                }
                break;
            default:
                Console.WriteLine("Something went wrong. Please report an Issue on GitHub and restart the program");
                throw new Exception();
        }
        string Message = Save.SaveNetwork(nnName, network, status, Console.WriteLine, () => Console.ReadLine() ?? string.Empty);
        switch(Message)
        {
            case "done":
                Console.WriteLine("Neural Network successfully saved as " + nnName);
                break;
            case "already existing":
                break;
            default:
                Console.WriteLine("An error occured. Please Report the Issue on GitHub.");
                
                break;
        }
        return nnName;
    }
}