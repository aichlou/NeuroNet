using NeuralNetworkDemo;

Start:
Console.Clear();
Console.WriteLine("Neural Network Demo");
Console.WriteLine("What would you like to do?");
Console.WriteLine("1. Create a NeuralNetwork");
int UserOutput;
if (int.TryParse(Console.ReadLine(), out int value)){
    UserOutput = value;
}
else
{
    Console.WriteLine("Please type in a number");
    Console.WriteLine("Press any Key");
    Console.ReadKey();
    goto Start;
}

switch(UserOutput)
{
    case 1:
        Main.CreateNeuralNetwork();
        break;
    default:
        Console.WriteLine("Please type in one of the shown options");
        goto Start;
}
