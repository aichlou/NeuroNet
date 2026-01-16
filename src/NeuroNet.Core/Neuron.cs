using System.Reflection.Metadata.Ecma335;

namespace NeuroNet.Core;


public abstract class Neuron
{
    //public double[] weights;
    public double value;
    public abstract double Fire(double[] inputs);
    public abstract NeuronDto ToDto();
    public abstract void EditWeights(double[] newWeights);
}
public class SigmoidNeuron : Neuron
{
    public double bias;
    public double[] weights;
    //public double value;
    public SigmoidNeuron(double bias, double[] weights)
    {
        this.bias = bias;
        this.weights = weights;
        this.value = 0;
    }
    public override void EditWeights(double[] newWeights)
    {
        this.weights = newWeights;
    }
    override public double Fire(double[] inputs)
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

        this.value = Sigmoid(totalInput);
        return this.value;
    }
    public void RandomizeWeights(Random rand, double minValue = -1.0, double maxValue = 1.0)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = rand.NextDouble() * (maxValue - minValue) + minValue;
        }
        bias = rand.NextDouble() * (maxValue - minValue) + minValue;
    }
    public override NeuronDto ToDto()
    {
        return new NeuronDto
        {
            type = "sigmoid",
            bias = this.bias,
            weights = this.weights
        };
    }
    public static double Sigmoid(double x)
    {
        return 1 / (1 + Math.Exp(-x));
    }
}

public class InputNeuron : Neuron
{
    //public double value;
    public InputNeuron ()
    {
    }
    public override double Fire(double[] inputs)
    {
        return this.value;
    }
    public override NeuronDto ToDto()
    {
        return new NeuronDto
        {
            type = "Input"
        };
    }
    public override void EditWeights(double[] newWeights)
    {
        throw new Exception("You should not be able to weight this Connection");
    }
}


public class NeuronDto
{
    public string type { get; set; } = "sigmoid";
    public double? bias { get; set; }
    public double[]? weights { get; set; }

    public Neuron ToNeuron()
    {
        var weightsCopy = this.weights != null ? (double[])this.weights.Clone() : Array.Empty<double>();
        switch(type)
        {
            case "sigmoid":
                return new SigmoidNeuron(this.bias ?? 0, weightsCopy);
            case "Input":
                return new InputNeuron();
            default: //Assume that it is a sigmoid Neuron
                return new SigmoidNeuron(this.bias ?? 0, weightsCopy);
        }
    }
}