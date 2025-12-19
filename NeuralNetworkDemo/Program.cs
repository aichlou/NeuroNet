using System;
using NeuralNetworkDemo;
using static NeuralNetworkDemo.Main;

partial class Program
{
    static void Main()
    {
        int[] testmenge = new int[] { 1, 5, 9, 7 };
        int UserOutput;
        bool Error;
        do {
            Error = false;
            Console.Clear();
            Console.WriteLine("Neural Network Demo");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1. Create a NeuralNetwork");
            if (!int.TryParse(Console.ReadLine(), out UserOutput)){
                Console.WriteLine("Please type in a valid number");
                Error = true;
            }
            switch (UserOutput) {
                case 1:
                    CreateNeuralNetwork(testmenge);
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


