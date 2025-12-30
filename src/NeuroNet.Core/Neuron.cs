namespace NeuroNet.Core;

public class Neuron
{
    public double bias;
    public double[] weights;
    public double value;
    public Neuron(double bias, double[] weights)
    {
        this.bias = bias;
        this.weights = weights;
        this.value = 0;
    }
    public void EditWeights(double[] newWeights)
    {
        this.weights = newWeights;
    }
    public double Fire(double[] inputs)
    {
        if (inputs.Length != weights.Length)
        {
            throw new ArgumentException("Input length must match weights length."); //Todo: Better error handling
        }

        double totalInput = bias;
        for (int i = 0; i < inputs.Length; i++)
        {
            totalInput += inputs[i] * weights[i];
        }

        value = Sigmoid(totalInput);
        return value;
    }
    public void RandomizeWeights(Random rand, double minValue = -1.0, double maxValue = 1.0)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = rand.NextDouble() * (maxValue - minValue) + minValue;
        }
        bias = rand.NextDouble() * (maxValue - minValue) + minValue;
    }
    public NeuronDto ToDto()
    {
        return new NeuronDto
        {
            type = "default",
            bias = this.bias,
            weights = this.weights
        };
    }
    public static double Sigmoid(double x)
    {
        return 1 / (1 + Math.Exp(-x));
    }
}



public class NeuronDto
{
    public string type { get; set; } = "default";
    public double? bias { get; set; }
    public double[]? weights { get; set; }

    public Neuron ToNeuron()
    {
        var weightsCopy = this.weights != null ? (double[])this.weights.Clone() : Array.Empty<double>();
        return new Neuron(this.bias ?? 0, weightsCopy);
    }
}