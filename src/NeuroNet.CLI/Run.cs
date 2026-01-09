using NeuroNet.Core;

namespace NeuroNet.CLI;

public class RunCLI
{
    public static MultipleValues<double[]> Run_Network(List<List<Neuron>>? LoadedNetwork)
    {
        bool Error;
        List<double> inputData = new List<double>();
            if(LoadedNetwork == null)
            {
                Console.WriteLine("No Neural Network loaded. Exiting...");
                return new MultipleValues<double[]> 
                {
                    Value = new double[0],
                    HasError = true,
                    ErrorMessage = "No Loaded Network"
                };
            }
            int InputLength = LoadedNetwork[0].Count;
            do {
                Error = false;
                Console.WriteLine($"You have to Input {InputLength} Values.");
                Console.WriteLine("Please enter input data separated by commas (e.g., 0.5,0.2,0.8):");
                string? inputLine = Console.ReadLine();
                if (!string.IsNullOrEmpty(inputLine))
                {
                    try {
                        inputData = inputLine.Split(',').Select(s => double.Parse(s.Trim())).ToList();
                        if(inputData.Count != InputLength)
                        {
                            Console.WriteLine($"Invalid number of inputs. Expected {InputLength} values.");
                            Console.WriteLine($"You entered {inputData.Count} values.");
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
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
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
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
            double[] output = NeuroNet.Core.Run.RunNeuralNetwork(LoadedNetwork!, inputData);
            for (int j = 0; j < output.Length; j++)
            {
                output[j] = output[j];
            }
            return new MultipleValues<double[]>
            {
                Value = output,
                HasError = false
            };
    }
}