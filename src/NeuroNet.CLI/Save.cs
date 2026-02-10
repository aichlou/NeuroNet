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
                bool Error = false;
                do {
                    Console.WriteLine("Do you want to save this Neural Network to a file? (y/n)");
                    saveResponse = Console.ReadLine() ?? string.Empty;
                    if(saveResponse.ToLower() == "y") {
                        try {
                            Console.WriteLine("How do you name the Neural Network?");
                            do
                            {
                                nnName = Console.ReadLine()!;
                                if(string.IsNullOrEmpty(nnName)) Console.WriteLine("Please name your Network Properly");
                                else if (Extras.isReturn(nnName))
                                {
                                    Error = true;
                                    nnName="Temp";
                                }
                                else if (nnName == "ReturnToMainMenu") 
                                {
                                    Console.WriteLine("The name 'ReturnToMainMenu' is reserved. Please choose another name.");
                                    nnName = ""; //Will cause the loop to continue and ask for a new name
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
                            return "NoName";
                        }
                    }
                    else
                    {
                        if(Extras.isReturn(saveResponse))
                        {
                            Console.WriteLine("We're sorry. You cannot return to the Last Layer Network Creeation Step, because the network was created fully.");
                            Console.WriteLine("Do you want to return to the Main Menu? (y/n)");
                            string returnResponse = Console.ReadLine() ?? string.Empty;
                            if(returnResponse.ToLower() == "y")
                            {
                                return "ReturnToMainMenu";
                            }
                            else
                            {
                                return SaveNetworkToFile(network, "new", currentnnName);
                            }
                        }
                        Console.WriteLine("Neural Network not saved.");
                        return "NoName";
                    }
                } while (Error);
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