using System;
using System.Runtime.InteropServices;
using NeuroNet;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using static NeuroNet.LoadNetwork;
using static NeuroNet.RunNetwork;
using static NeuroNet.Main;
using static NeuroNet.Filemanagement;

int UserOutput;
bool Error;
List<List<Neuron>>? LoadedNetwork;
do
{
    Error = false;
    Console.Clear();
    Console.WriteLine("Neural Network Demo");
    Console.WriteLine("What would you like to do?");
    Console.WriteLine("1. Create a NeuralNetwork");
    Console.WriteLine("2. Load a NeuralNetwork");
    if (!int.TryParse(Console.ReadLine(), out UserOutput))
    {
        Console.WriteLine("Please type in a valid number");
        Error = true;
    }
    switch (UserOutput)
    {
        case 1:
            CreateNeuralNetwork();
            break;
        case 2:
            Error = LoadNeuralNetwork();
            break;
        default:
            Console.WriteLine("Please type in one of the shown options");
            Error = true;
            break;
    }
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
} while (Error);

Console.WriteLine("Do you want to run the Neural Network now? (y/n)");
if(Console.ReadLine()!.ToLower() == "y")
{
    List<double> inputData = new List<double>();
    // TODO: Anzeigen wie viele Inputs benötigt werden basierend auf der Input Layer
    Console.WriteLine("Please enter input data separated by commas (e.g., 0.5,0.2,0.8):");
    string? inputLine = Console.ReadLine();
    if (!string.IsNullOrEmpty(inputLine))
    {
        inputData = inputLine.Split(',').Select(s => double.Parse(s.Trim())).ToList();
    }
    else
    {
        Console.WriteLine("No input data provided. Using default input data: 0.0, 0.0, 0.0");
        inputData = new List<double> { 0.0, 0.0, 0.0 };
    }
    RunNeuralNetwork(LoadedNetwork!, inputData); //WHY DOES IT THROWS AN ERROR HERE???? GRRRRRR
}
Console.WriteLine("Running Neural Network...");

Console.WriteLine("Exiting Program...");
Console.ReadKey();