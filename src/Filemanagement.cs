namespace NeuroNet;

using System.Data;
using System.Text.Json;

public static class Filemanagement
{
    static string baseDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    static string appDataPath = Path.Combine(baseDataPath, "NeuroNet");
    public static void SaveNetworkToFile(string nnName, string jsonData)
    {
        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }
        string filePath = Path.Combine(appDataPath, nnName + ".nn");
        File.WriteAllText(filePath, jsonData);
        Console.WriteLine("Neural Network saved as " + nnName);
    }
    public static void ListSavedNetworks()
    {
        if (!Directory.Exists(appDataPath))
        {
            Console.WriteLine("No saved Neural Networks found.");
            return;
        }
        string[] files = Directory.GetFiles(appDataPath, "*.nn");
        if (files.Length == 0)
        {
            Console.WriteLine("No saved Neural Networks found.");
            return;
        }
        Console.WriteLine("Saved Neural Networks:");
        int index = 1;
        foreach (string file in files)
        {
            Console.WriteLine(index + ". " + Path.GetFileNameWithoutExtension(file));
            index++;
        }
    }
    public static string LoadNetworkFromFile(string nnName)
    {
        if (int.TryParse(nnName, out _))
        {
            string[] files = Directory.GetFiles(appDataPath, "*.nn");
            int fileIndex = int.Parse(nnName) - 1;
            if (fileIndex < 0 || fileIndex >= files.Length)
            {
                Console.WriteLine("Invalid Neural Network selection.");
                return string.Empty;
            }
            nnName = Path.GetFileNameWithoutExtension(files[fileIndex]);
        }
        else
        {
            nnName = nnName.Trim();
        }
        if (string.IsNullOrEmpty(nnName))
        {
            Console.WriteLine("Neural Network name cannot be empty.");
            return string.Empty;
        }
        if (!Directory.Exists(appDataPath))
        {
            Console.WriteLine("No saved Neural Networks found.");
            return string.Empty;
        }
        else
        {
            nnName = nnName.Trim();
        }
        string filePath = Path.Combine(appDataPath, nnName + ".nn");
        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }
        else
        {
            Console.WriteLine("Neural Network file not found: " + nnName);
            return string.Empty;
        }
    }
    public static void SaveNetwork(string nnName, List<List<Neuron>> network, string status)
    {
        if (status == "new" && File.Exists(Path.Combine(appDataPath, nnName + ".nn")))
        {
            Console.WriteLine("Neural Network already exists. Do you want to overwrite it? (y/n)");
            string overwriteChoice = Console.ReadLine() ?? "n";
            if (overwriteChoice.ToLower() != "y")
            {
                Console.WriteLine("Neural Network not saved.");
                return;
            }
        }
        if (status == "overwrite" && File.Exists(Path.Combine(appDataPath, nnName + ".nn")))
        {
            Console.WriteLine("Overwriting existing Neural Network: " + nnName);
        }
        if (status == "overwrite" && !File.Exists(Path.Combine(appDataPath, nnName + ".nn")))
        {
            Console.WriteLine("Neural Network does not exist. Saving as new Neural Network: " + nnName);
            GitHubReportIssue.ReportToGitHub(
                "Attempted to overwrite a non-existing Neural Network.",
                "",
                "",
                "The user tried to overwrite a Neural Network that does not exist.",
                true
            );
        }
        List<List<NeuronDto>> dtoNetwork = new List<List<NeuronDto>>();
        foreach(var layer in network)
        {
            List<NeuronDto> dtoLayer = new List<NeuronDto>();
            foreach(var neuron in layer)
            {
                dtoLayer.Add(neuron.ToDto());
                //Console.WriteLine($"Debug: Created Neuron DTO");
            }
            dtoNetwork.Add(dtoLayer);
        }
        string contentJson = JsonSerializer.Serialize(dtoNetwork);
        Console.WriteLine($"Debug: JSON Original: {JsonSerializer.Serialize(network)}");
        Console.WriteLine($"Debug: Content JSON: {contentJson}");
        string versionInfo =
                JsonSerializer
                    .Deserialize<Dictionary<string, string>>(
                        File.ReadAllText("../../../AppVersion.json")
                    )?["version"]
                ?? "Unknown Version";
        var metadata = new
        {
            Name = nnName,
            Type = "Default",
            Version = 0.1,
            Description = "A Neural Network created with NeuroNet.",
            GitHub = "https://github.com/aichlou/NeuroNet",
            VersionOfProramm = versionInfo,
            CreatedAt = DateTime.UtcNow,
        };

        string metadataJson = JsonSerializer.Serialize(metadata);
        string combinedJson = "{ \"Metadata\": " + metadataJson + ", \"Network\": " + contentJson + " }";
        Filemanagement.SaveNetworkToFile(nnName, combinedJson);
    }
}