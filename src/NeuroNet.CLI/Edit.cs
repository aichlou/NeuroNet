using NeuroNet.Core;

namespace NeuroNet.CLI;

class EditCLI
{
    public static List<List<Neuron>> RandomizeIfNeeded(List<List<Neuron>> network, string currentnnName)
    {
        bool allWeightsZero = Edit.allWeightsZero(network);
        if (allWeightsZero) {
            network = Edit.RandomizeWeights(network);
            Console.WriteLine("The weights were randomized since they were all null");
            SaveCLI.SaveNetworkToFileY(network, "overwrite", currentnnName);
        }
        return network;
    }

    public static MultipleValues<List<List<Neuron>>> Edit_Network(List<List<Neuron>> network, string currentnnName)
    {
        bool Error = false;
        do {
            Console.WriteLine("Edit Neural Network");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Randomize Weights");
            Console.WriteLine("2. Edit Weights Manually");
            Console.WriteLine("3. Edit Number of Layers & Neurons");
            Console.WriteLine("4. Edit Name of the Network");
            //Console.WriteLine("4. Edit Neuron Types");
            string UserOutputString = Console.ReadLine() ?? string.Empty;
            if (!int.TryParse(UserOutputString, out int UserOutput))
            {
                if (Extras.IsReturn(UserOutputString))
                {
                    Console.WriteLine("Returning to Main Menu...");
                    return new MultipleValues<List<List<Neuron>>>
                    {
                        HasError = true,
                        Value = network,
                        ErrorMessage = Program.returnString,
                    };
                }
                Console.WriteLine("Please insert a valid number");
                Error = true;
            }
            else
            {
                switch (UserOutput)
                {
                    case 1:
                        network = Edit.RandomizeWeights(network);
                        SaveCLI.SaveNetworkToFileY(network, "overwrite", currentnnName);
                        Console.WriteLine("Weights randomized and saved to file");
                        break;
                    case 2:
                        /*TwoValues<List<List<Neuron>>, bool> EditWeightsResult = Edit.EditWeightsManually(network);
                        network = EditWeightsResult.Value1;
                        Error = EditWeightsResult.Value2;
                        if (!Error)
                        {
                            SaveCLI.SaveNetworkToFileY(network, "overwrite", currentnnName);
                            Console.WriteLine("Weights edited and saved to file");
                        } */
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    default:
                        Console.WriteLine("Please insert one of the shown Options");
                        Error = true;
                        break;
                }
            }
        } while (Error);



        return new MultipleValues<List<List<Neuron>>>
        {
          Value = network,
          HasError = false,
        };
    }
}