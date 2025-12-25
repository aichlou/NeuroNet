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
}