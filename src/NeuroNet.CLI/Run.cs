using NeuroNet.Core;

namespace NeuroNet.CLI;

public class RunCLI
{
    public static MultipleValues<double[]> Run_Network(List<List<Neuron>>? LoadedNetwork)
    {
        bool Error;
        List<double> inputData = new List<double>();
            if(LoadedNetwork == null || LoadedNetwork.Count == 0 || LoadedNetwork[0] == null || LoadedNetwork[0].Count == 0)
            {
                Console.WriteLine("No Neural Network loaded. Exiting...");
                return new MultipleValues<double[]> 
                {
                    Value = new double[0],
                    HasError = true,
                    ErrorMessage = "No Loaded Network"
                };
            }
            int InputLength = LoadedNetwork[0][0].GetWeights().Length;
            do {
                Error = false;
                Console.WriteLine($"You have to Input {InputLength} Values.");
                Console.WriteLine("Please enter input data separated by commas (e.g., 0.5,0.2,0.8):");
                string? inputLine = Console.ReadLine();
                if (!string.IsNullOrEmpty(inputLine))
                {
                    if (Extras.IsReturn(inputLine))
                    {
                        Console.WriteLine("Exiting Run Process...");
                        return new MultipleValues<double[]> 
                        {
                            Value = new double[0],
                            HasError = true,
                            ErrorMessage = "User exited run process."
                        };
                    }
                    try {
                        inputData = inputLine.Split(',').Select(s => double.Parse(s.Trim())).ToList();
                        if(inputData.Count != InputLength)
                        {
                            Console.WriteLine($"Invalid number of inputs. Expected {InputLength} values.");
                            Console.WriteLine($"You entered {inputData.Count} values.");
                            Extras.PressKey();
                            Error = true;
                        }
                        /*else
                        {
                            Console.WriteLine("Inputs:");
                            foreach (double item in inputData)
                            {
                                Console.WriteLine(item);
                            }
                        } */
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid input format. Please ensure you enter numbers separated by commas.");
                        Extras.PressKey();
                        Error = true;
                    }
                }
                else
                {
                    string defaultInput = string.Join(",", Enumerable.Repeat("0.0", InputLength));
                    Console.WriteLine("No input data provided. Using default input data: " + defaultInput);
                    inputData = new List<double>(Enumerable.Repeat(0.0, InputLength));
                }
            }
            while(Error);
            Console.WriteLine("Running Neural Network...");
            double[] output = NeuroNet.Core.Run.RunNeuralNetwork(LoadedNetwork, inputData);
            return new MultipleValues<double[]>
            {
                Value = output,
                HasError = false
            };
    }
}