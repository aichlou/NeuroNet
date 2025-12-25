using System;
using System.Runtime.InteropServices;
using static NeuroNet.Main;
using static NeuroNet.Filemanagement;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using MeuroNet;

namespace NeuroNet
{
    public static class LoadNetwork
    {
        public static MultipleValues<List<List<Neuron>>> LoadNeuralNetwork()
        {
            Console.WriteLine("Loading Neural Network...");
            ListSavedNetworks();
            Console.WriteLine("Please type in the name of the Neural Network you would like to load:");
            string nnName = Console.ReadLine() ?? "MyNeuralNetwork";
            string networkData = Filemanagement.LoadNetworkFromFile(nnName);
            if (!string.IsNullOrEmpty(networkData))
            {
                Console.WriteLine("Neural Network loaded successfully.");
            }
            else
            {
                Console.WriteLine("Failed to load Neural Network.");
                return new MultipleValues<List<List<Neuron>>>
                {
                    Value = null!,
                    HasError = true,
                    ErrorMessage = "Failed to load Neural Network."
                };
            }
            List<List<Neuron>>? network;
            try
            {
                network = JsonSerializer.Deserialize<List<List<Neuron>>>(networkData);
            }
            catch (Exception e)
            {
                GitHubReportIssue.ReportToGitHub("Failed to Load Neural Network", e.Message, e.StackTrace ?? "No stack trace available.", "Deserialization Error in LoadNeuralNetwork", true);
                return new MultipleValues<List<List<Neuron>>>
                {
                    Value = null!,
                    HasError = true,
                    ErrorMessage = "Deserialization Error."
                };

            }
            for(int i=0; i < network!.Count; i++)
            {
                for(int j=0; j < network[i].Count; j++)
                {
                    if(i == 0)
                    {
                        network[i][j].EditWeights(new double[1]);
                    }
                    else
                    {
                        network[i][j].EditWeights(new double[network[i - 1].Count]);
                    }
                }
            }

            Console.WriteLine("Neural Network Structure:");
            foreach (var layer in network!)
            {
                Console.WriteLine("Layer with " + layer.Count + " neurons.");
            }
            return new MultipleValues<List<List<Neuron>>>
            {
                Value = network,
                HasError = false,
                ErrorMessage = string.Empty
            };
        }
    }
}