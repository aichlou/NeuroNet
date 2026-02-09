
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Globalization;
using System.Text.Json;
using System.Collections.Specialized;
namespace NeuroNet.Core;

public class Load {

    static readonly string baseDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    static readonly string appDataPath = Path.Combine(baseDataPath, "NeuroNet");
    
    public static string[]? ListSavedNetworks(string? DirectoryPath = null)
    {
        if(DirectoryPath == null) DirectoryPath = appDataPath;
        if (!Directory.Exists(DirectoryPath))
        {
            return Array.Empty<string>();
        }
        string[] files;
        files = Directory.GetFiles(DirectoryPath, "*.nn");
        if (files.Length == 0)
        {
            return new string [0];
        }
        int index = 0;
        string[] output = new string[files.Length];
        foreach (string file in files)
        {
            output[index] = Path.GetFileNameWithoutExtension(file);
            index++;
        }
        return output;
    }

    public static MultipleValues<string> ContentOf(string? nnName, Action<string>? Message = null)
    {
        string filePath = Path.Combine(appDataPath, nnName + ".nn");
        if (File.Exists(filePath))
        {
            try {
                return new MultipleValues<string>
                {
                    Value = File.ReadAllText(filePath),
                    HasError = false,
                };
            }
            catch(Exception e)
            {
                return new MultipleValues<string>
                {
                    Value = string.Empty,
                    HasError = true,
                    ErrorMessage = "Failed to read Neural Network file. Exeption: " + e.Message
                };
            }
        }
        else
        {
            return new MultipleValues<string>
            {
                Value = string.Empty,
                HasError = true,
                ErrorMessage = "Failed to read Neural Network file. File does not exist."
            };
        }
    }

    public static List<List<Neuron>>? JsonToList (string networkData)
    {
        List<List<NeuronDto>>? networkDto;
        try
        {
            var file = JsonSerializer.Deserialize<FileDto>(networkData) ?? throw new Exception("Deserialized file is null.");
            networkDto = file.Network;
        }
        catch (Exception)
        {
            Console.WriteLine("An Error occurred while deserializing the Neural Network.");
            return new List<List<Neuron>>{};
        }
        List<List<Neuron>>? network = networkDto?.Select(layer => layer.Select(neuronDto => neuronDto.ToNeuron()).ToList()).ToList();
        return network;
    }

    public static string NameOf(string? nnName) //Returns the Name of a File (in the App Data Path) if it exists based on the name or a number
    {
        if (string.IsNullOrEmpty(nnName))
        {
            return string.Empty;
        }

        if (int.TryParse(nnName, out int number))
        {
            if (!Directory.Exists(appDataPath))
            {
                return string.Empty;
            }
            string[] files = Directory.GetFiles(appDataPath, "*.nn");
            int fileIndex = number - 1;
            if (fileIndex < 0 || fileIndex >= files.Length)
            {
                return string.Empty; //should Retrun an Error
            }
            nnName = Path.GetFileNameWithoutExtension(files[fileIndex]);
        }
        else
        {
            nnName = nnName.Trim();
        }

        if (!Directory.Exists(appDataPath))
        {
            return string.Empty;
        }

        return nnName;
    }

    public static double[,] GetCSVData(string path)
    {
        var lines = File.ReadAllLines(path);
        var rows = new List<double[]>();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var values = line.Split(';')
                            .Select(v => double.Parse(v, CultureInfo.InvariantCulture))
                            .ToArray();

            rows.Add(values);
        }
        double[,] fields = new double[rows.Count(), lines.Count()];
        for(int i = 0; i < rows.Count(); i++)
        {
            for(int j = 0; i < lines.Count(); i++)
            {
                fields[i,j] = rows[i][j];
            }
        }
        return fields;
    }
    
    public static double[,,] SortDataset(double[,] data) //Im sure that this contains 1000 Errors but AHHHHHH
    {
        int datasets;
        int lines_Length = data.GetLength(1);
        try {
            datasets = data.GetLength(0) / 2;
        }
        catch (Exception)
        {
            return new double[0,0,0];
        }
        double[,,] output = new double[datasets, 2, lines_Length];
        for(int i = 0; i < datasets; i++)
        {
            for(int j = 0; j < lines_Length; i++)
            {
                output[i, 0, j] = data[i * 2 - 1, j];
                output[i, 1, j] = data[i * 2, j];
            }
        }
        return output;
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