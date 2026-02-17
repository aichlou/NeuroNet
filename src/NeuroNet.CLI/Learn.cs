using System.Data;
using NeuroNet.Core;
using static NeuroNet.Core.Load;
namespace NeuroNet.CLI;


public class Learn
{
    public static void UserDialoge(List<List<Neuron>> network)
    {
        network = network 
        ?? throw new ArgumentNullException(nameof(network));
        bool Error;
        do {
            Error = false;
            Console.WriteLine("Enter the path to your data file (currently only CSV files(;) are supported)");
            Console.WriteLine("Type e for an Explanation how the CSV should be formatted");
            string? path = Console.ReadLine();
            if(path == "e")
            {
                Console.WriteLine("The CSV file must be structured in pairs of lines:");
                Console.WriteLine("- The first line contains the input values");
                Console.WriteLine("- The second line contains the expected output values");
                Console.WriteLine();
                Console.WriteLine("Each pair of lines represents one training dataset.");
                Console.WriteLine();
                Console.WriteLine("Example:");
                Console.WriteLine("0.2,0.5,1.0");
                Console.WriteLine("1");
                Console.WriteLine("0.1,0.9,0.3");
                Console.WriteLine("0");
                Console.WriteLine();
                Extras.PressKey();
                Error = true;
            }
            else if (Extras.IsReturn(path))
            {
                Console.WriteLine("Exiting Learning Process...");
                Error = true;
            }
            else if (!Directory.Exists(path)) {
                Console.WriteLine("You need to insert a existing path.");
                Extras.PressKey();
                Error = true;
            }
            else
            {
                double[,] Data = GetCSVData(path);
                double[,,] dataSet = SortDataset(Data); //Converts the double[rows,lines] to double[dataset,input/output,entry]
                if(dataSet.GetLength(0) == 0 && dataSet.GetLength(1) == 0 && dataSet.GetLength(2) == 0) {
                    Console.WriteLine("The File is broken...");
                    Error = true;
                    Extras.PressKey();
                }
                else
                { //Das sollte in .Core (Also der Lern Block)
                    
                    for (int i = 0; i < dataSet.GetLength(0); i++)
                    {
                        double[] input = new double[dataSet.GetLength(2)];
                        double[] expected_output = new double[dataSet.GetLength(2)];
                        for (int j = 0; j < dataSet.GetLength(2); i++)
                        {
                            input[j] = dataSet[i, 1, j];
                            expected_output[j] = dataSet[i, 2, j];

                        }
                        double[] output = NeuroNet.Core.Run.RunNeuralNetwork(network, input.ToList());
                        NeuroNet.Core.Learn.Learning(network, input, expected_output);
                    }
                }
            }
        } while (Error) ;
    }
}