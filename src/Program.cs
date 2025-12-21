using System;
using System.Runtime.InteropServices;
using NeuralNetworkDemo;
using static NeuralNetworkDemo.Main;
using static NeuralNetworkDemo.Filemanagement;
using System.Text.Json;

partial class Program
{
    static void Main()
    {
        int UserOutput;
        bool Error;
        do {
            Error = false;
            Console.Clear();
            Console.WriteLine("Neural Network Demo");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1. Create a NeuralNetwork");
            Console.WriteLine("2. Load a NeuralNetwork");
            if (!int.TryParse(Console.ReadLine(), out UserOutput)){
                Console.WriteLine("Please type in a valid number");
                Error = true;
            }
            switch (UserOutput) {
                case 1:
                    CreateNeuralNetwork();
                    break;
                case 2:
                    Console.WriteLine("Loading Neural Network...");
                    ListSavedNetworks();
                    Console.WriteLine("Please type in the name of the Neural Network you would like to load:");
                    string nnName = Console.ReadLine() ?? "MyNeuralNetwork";
                    string networkData = Filemanagement.LoadNetworkFromFile(nnName);
                    if (!string.IsNullOrEmpty(networkData))
                    {
                        Console.WriteLine("Neural Network loaded successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to load Neural Network.");
                    }
                    List<List<Neuron>>? network = JsonSerializer.Deserialize<List<List<Neuron>>>(networkData);
                    break;
                default:
                    Console.WriteLine("Please type in one of the shown options");
                    Error = true;
                    break;
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        } while (Error);
    }
}