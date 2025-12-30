
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text.Json;
namespace NeuroNet.Core;

public class Load {

    static readonly string baseDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    static readonly string appDataPath = Path.Combine(baseDataPath, "NeuroNet");

    public static MultipleValues<TwoValues<List<List<Neuron>>, string?>> LoadNeuralNetwork(Action<string>? Message = null, Func<string>? readInput = null)
    {
        Message?.Invoke("Loading Neural Network...");
        ListSavedNetworks(Message);
        Message?.Invoke("Please type in the name of the Neural Network you would like to load:");
        string? nnName = readInput?.Invoke();
        nnName = NameOf(nnName, Message);
        string networkData = ContentOf(nnName, Message);
        if (string.IsNullOrEmpty(networkData))
        {
            Message?.Invoke("Failed to load Neural Network.");
            return new MultipleValues<TwoValues<List<List<Neuron>>, string?>>
            {
                Value = new TwoValues<List<List<Neuron>>, string?> { Value1 = null},
                HasError = true,
                ErrorMessage = "Failed to load Neural Network."
            };
        }
        List<List<NeuronDto>>? networkDto;
        try
        {
            var file = JsonSerializer.Deserialize<FileDto>(networkData) ?? throw new Exception("Deserialized file is null.");
            networkDto = file.Network;
        }
        catch (Exception e)
        {
            Message?.Invoke("An Error occurred while deserializing the Neural Network.");
            GitHubReportIssue.ReportToGitHub("Failed to Load Neural Network", e.Message, e.StackTrace ?? "No stack trace available.", "Deserialization Error in LoadNeuralNetwork", true, Message, readInput);
            return new MultipleValues<TwoValues<List<List<Neuron>>, string?>>
            {
                Value = new TwoValues<List<List<Neuron>>, string?> {Value1 = null!},
                HasError = true,
                ErrorMessage = "Deserialization Error."
            };
        }
        List<List<Neuron>>? network = networkDto?.Select(layer => layer.Select(neuronDto => neuronDto.ToNeuron()).ToList()).ToList();

        if (network == null)
        {
            Message?.Invoke("Failed to convert Neural Network DTO to Neurons.");
            return new MultipleValues<TwoValues<List<List<Neuron>>, string?>>
            {
                Value = null!,
                HasError = true,
                ErrorMessage = "Conversion Error."
            };
        }
        for(int i=0; i < network.Count; i++)
        {
            for(int j=0; j < network[i].Count; j++)
            {
                if(i == 0)
                {
                    network[i][j].EditWeights(new double[1]);
                }
                else
                {
                    network[i][j].EditWeights(new double[network[i - 1].Count]);
                }
            }
        }
        Message?.Invoke("Neural Network Structure:");
        foreach (var layer in network!)
        {
            Message?.Invoke("Layer with " + layer.Count + " neurons.");
        }
         return new MultipleValues<TwoValues<List<List<Neuron>>, string?>>
        {
            Value = new TwoValues<List<List<Neuron>>, string?> { Value1 = network, Value2 = nnName},
            HasError = false,
            ErrorMessage = string.Empty
        };
    }
    public static void ListSavedNetworks(Action<string>? Message = null)
    {
        if (!Directory.Exists(appDataPath))
        {
            Message?.Invoke("No saved Neural Networks found.");
            return;
        }
        string[] files;
        try
        {
            files = Directory.GetFiles(appDataPath, "*.nn");
        }
        catch (Exception ex)
        {
            Message?.Invoke($"Error accessing saved networks: {ex.Message}");
            return;
        }
        if (files.Length == 0)
        {
            Message?.Invoke("No saved Neural Networks found.");
            return;
        }
        Message?.Invoke("Saved Neural Networks:");
        int index = 1;
        foreach (string file in files)
        {
            Message?.Invoke(index + ". " + Path.GetFileNameWithoutExtension(file));
            index++;
        }
    }

    public static string ContentOf(string? nnName, Action<string>? Message = null)
    {
        string filePath = Path.Combine(appDataPath, nnName + ".nn");
        if (File.Exists(filePath))
        {
            try {
            return File.ReadAllText(filePath);
            }
            catch(Exception e)
            {
                Message?.Invoke("An error occurred while reading the Neural Network file.");
                GitHubReportIssue.ReportToGitHub("Failed to Read Neural Network File", e.Message, e.StackTrace ?? "No stack trace available.", "File Read Error in Nameof", true, Message, null);
                return string.Empty;
            }
        }
        else
        {
            Message?.Invoke("Neural Network file not found: " + nnName);
            return string.Empty;
        }
    }

    public static string NameOf(string? nnName, Action<string>? Message = null)
    {
        if (string.IsNullOrEmpty(nnName))
        {
            Message?.Invoke("Neural Network name cannot be empty.");
            return string.Empty;
        }

        if (int.TryParse(nnName, out int number))
        {
            if (!Directory.Exists(appDataPath))
            {
                Message?.Invoke("No saved Neural Networks found.");
                return string.Empty;
            }
            string[] files = Directory.GetFiles(appDataPath, "*.nn");
            int fileIndex = number - 1;
            if (fileIndex < 0 || fileIndex >= files.Length)
            {
                Message?.Invoke("Invalid Neural Network selection.");
                return string.Empty;
            }
            nnName = Path.GetFileNameWithoutExtension(files[fileIndex]);
        }
        else
        {
            nnName = nnName.Trim();
        }

        if (!Directory.Exists(appDataPath))
        {
            Message?.Invoke("No saved Neural Networks found.");
            return string.Empty;
        }

        return nnName;
    }

    public class FileDto
    {
        public MetadataDto? Metadata { get; set; }
        public List<List<NeuronDto>>? Network { get; set; }
    }

    public class MetadataDto
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public double Version { get; set; }
        public string Description { get; set; } = "";
        public string GitHub { get; set; } = "";
        public string VersionOfProgram { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }

    public List<double[]> LoadCSV(string filelocation)
    {
        var lines = File.ReadAllLines(filelocation);
        var rows = new List<double[]>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var values = line.Split(';')
                            .Select(v => double.Parse(v, CultureInfo.InvariantCulture))
                            .ToArray();

            rows.Add(values);
        }
        var fields = rows;
        return fields;
    }

    public TwoValues<double[], double[]> OrderValues(List<double[]> fields, int input_Numbers)
    {
        

        return new TwoValues<double[], double[]> { Value1 = new double[1], Value2 = new double[1]} ;
    }
}