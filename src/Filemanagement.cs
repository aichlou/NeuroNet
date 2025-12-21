namespace NeuralNetworkDemo;

using System.Data;
using System.Text.Json;

public static class Filemanagement
{
    static string baseDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    static string appDataPath = Path.Combine(baseDataPath, "NeuroNet");
    public static void SaveNetworkToFile(string nnName, string networkData)
    {
        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }
        string filePath = Path.Combine(appDataPath, nnName + ".nn");
        File.WriteAllText(filePath, networkData);
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
        foreach (string file in files)
        {
            Console.WriteLine(Path.GetFileNameWithoutExtension(file));
        }
    }
    public static string LoadNetworkFromFile(string nnName)
    {
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
}