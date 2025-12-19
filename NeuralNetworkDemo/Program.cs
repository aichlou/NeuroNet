using NeuralNetworkDemo;

int UserOutput;
do {
  Console.Clear();
  Console.WriteLine("Neural Network Demo");
  Console.WriteLine("What would you like to do?");
  Console.WriteLine("1. Create a NeuralNetwork");
  Console.WriteLine("2. Exit");
  if (!int.TryParse(Console.ReadLine(), out UserOutput)) {
    Console.WriteLine("ERROR: INPUT IS NOT A NUMBER!");
  }
  switch (UserOutput) {
    case 1:
      NeuralNetworkDemo.NeuralNetworkDemo.Main();
      break;
    case 2:
      break;
    default:
      Console.WriteLine("Please type in one of the shown options");
      break;
  }
} while (UserOutput != 2);
