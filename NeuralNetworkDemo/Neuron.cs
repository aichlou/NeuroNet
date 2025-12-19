namespace NeuralNetworkDemo
{
  public class Neuron
  {
    private double value;
    public Neuron(double value)
    {
      Console.WriteLine("New neuron created");
      this.value = value;
      Console.WriteLine($"Value of the neuron is { this.value }");
    }
  }
}
