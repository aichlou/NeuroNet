﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.NetworkInformation;
using NeuroNet.Core;
using Spectre.Console;
namespace NeuroNet.CLI;

internal class Program
{
    public const string returnString = "return";
    public static void Main(string[] args)
    {
        while (true) {
            bool again = false;
            string? Way = null;
            TwoValues<List<List<Neuron>>, string?> CreationResult = default!;
            string currentnnName = "MyNeuralNetwork";
            do {
                int UserOutput;
                bool Error;
                List<List<Neuron>>? LoadedNetwork = null;
                do
                {
                    Error = false;
                    if (!again || Way == "") {
                        Console.Clear();
                        Console.WriteLine("NeuroNet");
                        Console.WriteLine("What would you like to do?");
                        Console.WriteLine("1. Create a NeuralNetwork");
                        Console.WriteLine("2. Load a NeuralNetwork");
                        Console.WriteLine("3. Exit");
                        string UserOutputString = Console.ReadLine() ?? string.Empty;
                        if (!int.TryParse(UserOutputString, out UserOutput))
                        {
                            if (Extras.IsReturn(UserOutputString))
                            {
                                Console.WriteLine("Exiting Program...");
                                Error = true;
                                return;
                            }
                            else
                            {
                                Console.WriteLine("Please type in a valid number");
                                Extras.PressKey();
                                Error = true;
                            }
                        }
                    }
                    else
                    {
                        switch (Way)
                        {
                            case "Create":
                                UserOutput = 1;
                                break;
                            case "Load":
                                UserOutput = 2;
                                break;
                            default:
                                Console.WriteLine("This should never happen, but if it does, just restart the program & Report on GitHub.");
                                return;
                        }
                    }
                    switch (UserOutput)
                    {
                        case 1:
                            Way = "Create";
                            bool repeat;
                            if (again) repeat = true;
                            else repeat = false;
                            do {
                                CreationResult = CreateCLI.CreatingProcess(CreationResult, again);
                                repeat = false;
                                if (CreationResult.Value2 == returnString)
                                {
                                    Console.WriteLine("Exiting Neural Network Creation...");
                                    Error = true;
                                }
                                else {
                                    LoadedNetwork = CreationResult.Value1 ?? throw new Exception("Loaded Network cannot be null");
                                    if (again)
                                    {
                                        Console.WriteLine("Deleting old Network & Creating new Network...");
                                        if (SaveCLI.DeleteFile(currentnnName)) Extras.PressKey();
                                        else //Return Keyword was typed in
                                        {
                                            Console.WriteLine("Why do you do that?");
                                            Console.WriteLine("Do you think I have no Life");
                                            Console.WriteLine("I have feelings too, you know...");
                                            Console.WriteLine("Please just type in the same name and everything will be fine...");
                                            Console.WriteLine();
                                        }
                                    }
                                    currentnnName = SaveCLI.SaveNetworkToFile(LoadedNetwork, "new");
                                    again = false;
                                    if (currentnnName.ToLower() == returnString.ToLower()) repeat = true;
                                }
                            }
                            while(repeat);
                            break;
                        case 2:
                            Way = "Load";
                            var result = LoadCLI.LoadNeuralNetwork();
                            if (result.HasError)
                            {
                                Error = true;
                                if (result.ErrorMessage != "User exited load process.") Extras.PressKey();
                            }
                            else
                            {
                                LoadedNetwork = (result.Value ?? throw new Exception("Network and network name cannot be null")).Value1 ?? throw new Exception("Network cannot be null");
                                currentnnName = result.Value.Value2 ?? throw new Exception("Network name cannot be null");
                            }
                            break;
                        case 3:
                            Console.WriteLine("Exiting Program...");
                            return;
                        default:
                            if(!Error) {
                                Console.WriteLine("Please type in one of the shown options");
                                Extras.PressKey();
                                Error = true;
                            }
                            break;
                    }
                    again = false;
                } while (Error);
                if (LoadedNetwork is null) throw new InvalidOperationException("LoadedNetwork must not be null here");

                Extras.PressKey();
                LoadedNetwork = EditCLI.RandomizeIfNeeded(LoadedNetwork, currentnnName);
                UserOutput = 0;
                do {
                    Error = false;
                    Console.WriteLine();
                    Console.WriteLine("What do you want to do?");
                    Console.WriteLine("1. Run the Neural Network");
                    Console.WriteLine("2. Let the Neural Network learn");
                    Console.WriteLine("3. Edit the Neural Network");
                    Console.WriteLine("4. Show NeuralNetwork");
                    Console.WriteLine("5. Return to Load/Create Menu");
                    Console.WriteLine("6. Exit");
                    string UserOutputString = Console.ReadLine() ?? string.Empty;
                    if (!int.TryParse(UserOutputString, out UserOutput))
                    {
                        if (Extras.IsReturn(UserOutputString)) again = true;
                        else {
                            Console.WriteLine("Please type in a valid number");
                            Error = true;
                        }
                    }
                    else
                    {
                        LoadedNetwork = LoadedNetwork ?? throw new Exception("Loaded Network cannot be null");
                        switch(UserOutput)
                        {
                            case 1:
                                var runResult = RunCLI.Run_Network(LoadedNetwork);
                                if (runResult.HasError)
                                {
                                    if (runResult.ErrorMessage == "User exited run process.") Error = true;
                                    else Error = true;
                                }
                                else {
                                double[] output = runResult.Value  ?? throw new Exception("Network Output cannot be null"); //No Error handeling
                                for(int i = 0; i < output.Length; i++)
                                {
                                    Console.WriteLine($"Neuron {i + 1}: {output[i]}");
                                }
                                }
                                Error = true;
                                break;
                            case 2:
                                Console.WriteLine("This feature is in the working process...");
                                Console.WriteLine("CAREFUL: This feature isn't working yet");
                                try {
                                    Error = Learn.UserDialoge(LoadedNetwork);
                                    Console.WriteLine("You Exited the Learning Process Successfully");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine($"You Exited the Learning Process with the Exception {e.Data}");
                                }
                                break;
                            case 3:
                                var editResult = EditCLI.Edit_Network(LoadedNetwork, currentnnName);
                                if (editResult.HasError)Error = true;
                                else
                                {
                                    LoadedNetwork = (editResult.Value ?? throw new Exception("Network cannot be null, when HasError is false")).Value1;
                                    currentnnName = (editResult.Value ?? throw new Exception("Network cannot be null, when HasError is false")).Value2 ?? currentnnName;
                                }
                                break;
                            case 4:
                                ShowCLI.ShowNetwork(LoadedNetwork);
                                Error = true;
                                break;
                            case 5: 
                                Error = false;
                                again = true;
                                Way = "";
                                break;
                            case 6:
                                Console.WriteLine("Exiting Program...");
                                Extras.PressKey();
                                return;
                            default:
                                Console.WriteLine("Please insert one of the shown Options");
                                Error = true;
                                break;
                        }
                    }
                }
                while (Error);
            } while (again);
            Extras.PressKey();
            Console.WriteLine("Returning to Main Menu...");
            Console.WriteLine("------------------------------");
        }
    }
}