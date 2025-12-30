namespace NeuroNet.Core;

public class Learn {
    public List<List<Neuron>> Lernen(List<List<Neuron>> network, double[] Input, double[] Expected_Output) {
        return network;
    }
    public double[] Cost(double[] input, double[] expected_output)
    {
        if(input.Length != expected_output.Length) { throw new Exception("Input and expected output must have the same length"); }
        double[] difference = new double[input.Length];
        for(int i = 0; i < input.Length; i++)
        {
            difference[i] = Math.Pow(input[i] - expected_output[i], 2);
        }
        return difference;
    }
    public double overall_Cost(double[] input, double[] expected_output)
    {
        double[] difference = Cost(input, expected_output);
        double overall = 0;
        for(int i = 0; i < difference.Length; i++)
        {
            overall += difference[i];
        }
        return overall;
    }
}