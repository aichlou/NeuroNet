using System.Text.Json;

namespace NeuroNet.Core;

public class Save
{
    static string baseDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    static string appDataPath = Path.Combine(baseDataPath, "NeuroNet");

    public static void SaveNetworkToFile(string nnName, string jsonData, Action<string>? Message = null)
    {
        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }
        string filePath = Path.Combine(appDataPath, nnName + ".nn");
        File.WriteAllText(filePath, jsonData);
    }

    public static void SaveNetwork(string nnName, List<List<Neuron>> network, string status, Action<string>? Message = null, Func<string>? readInput = null)
    {
        if (status == "new" && File.Exists(Path.Combine(appDataPath, nnName + ".nn")))
        {
            Message?.Invoke("Neural Network already exists. Do you want to overwrite it? (y/n)");
            string overwriteChoice = readInput?.Invoke() ?? "n";
            if (overwriteChoice.ToLower() != "y")
            {
                Message?.Invoke("Neural Network not saved.");
                return;
            }
        }
        if (status == "overwrite" && File.Exists(Path.Combine(appDataPath, nnName + ".nn")))
        {
            Message?.Invoke("Overwriting existing Neural Network: " + nnName);
        }
        if (status == "overwrite" && !File.Exists(Path.Combine(appDataPath, nnName + ".nn")))
        {
            Message?.Invoke("Neural Network does not exist. Saving as new Neural Network: " + nnName);
            GitHubReportIssue.ReportToGitHub(
                "Attempted to overwrite a non-existing Neural Network.",
                "",
                "",
                "The user tried to overwrite a Neural Network that does not exist.",
                true,
                Message, 
                readInput
            );
        }
        List<List<NeuronDto>> dtoNetwork = new List<List<NeuronDto>>();
        foreach(var layer in network)
        {
            List<NeuronDto> dtoLayer = new List<NeuronDto>();
            foreach(var neuron in layer)
            {
                dtoLayer.Add(neuron.ToDto());
            }
            dtoNetwork.Add(dtoLayer);
        }
        string contentJson = JsonSerializer.Serialize(dtoNetwork);
        string versionInfo = "Unknown Version";
        try
        {
            // Look for AppVersion.json in common locations
            string? appVersionPath = null;
            var searchPaths = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "AppVersion.json"),
                Path.Combine(AppContext.BaseDirectory, "../AppVersion.json"),
                Path.Combine(AppContext.BaseDirectory, "../../../../AppVersion.json")
            };

            foreach (var path in searchPaths)
            {
                if (File.Exists(path))
                {
                    appVersionPath = path;
                    break;
                }
            }

            if (appVersionPath != null)
            {
                var versionData = JsonSerializer.Deserialize<Dictionary<string, string>>(
                    File.ReadAllText(appVersionPath));
                versionInfo = versionData?["version"] ?? "Unknown Version";
            }
        }
        catch (Exception)
        {
            // Fall back to Unknown Version if reading fails
            versionInfo = "Unknown Version";
        }
        var metadata = new
        {
            Name = nnName,
            Type = "Default",
            Version = 0.1,
            Description = "A Neural Network created with NeuroNet.",
            GitHub = "https://github.com/aichlou/NeuroNet",
            VersionOfProgram = versionInfo,
            CreatedAt = DateTime.UtcNow,
        };

        /*string metadataJson = JsonSerializer.Serialize(metadata);
        string combinedJson = "{ \"Metadata\": " + metadataJson + ", \"Network\": " + contentJson + " }";
        SaveNetworkToFile(nnName, combinedJson); */

        var combinedData = new
        {
            Metadata = metadata,
            Network = dtoNetwork
        };
        string combinedJson = JsonSerializer.Serialize(combinedData);
        SaveNetworkToFile(nnName, combinedJson, Message);
    }
}