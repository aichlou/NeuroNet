using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Xml;
using NeuroNet.Core;
//using NeuroNet.CLI;
using Microsoft.VisualBasic;

namespace NeuroNet.CLI;
/*var result =InputCLI.ConfirmInputReturn();
if (result.Value1 == InputCLI.TriState.True) Console.WriteLine("This is Valid");
else if (result.Value1 == InputCLI.TriState.False) Console.WriteLine("This isn't Valid");
else if (result.Value1 == InputCLI.TriState.Return) Console.WriteLine("Return"); */




public class InputCLI
{

    public enum TriState
    {
        True, False, Return
    }
    /// <summary>
    /// Gets an Input from the Console and returns if the input matches one of the given Values
    /// </summary>
    /// <param name="Values">Matching Values</param>
    /// <param name="Input">Input from the Console</param>
    /// <param name="Config">Preconfigured kit</param>
    /// <returns>TriState True, False or Return</returns>
    public static TriState ConfirmAndReturn(string[]? Values = null, string? Input = null, string? Config = null)
    {
        if (Input == null) Input = Console.ReadLine() ?? "";
        if (Extras.IsReturn(Input)) return TriState.Return;
        if (Config == null)
        { 
            if (Values == null) Values = ["yes", "y"];
            return Values.Contains(Input) ? TriState.True : TriState.False;         
        }
        else if (Config.StartsWith("numbers"))
        {
            string End = Config.Substring("numbers".Length);
            End = End == "" ? "," : End;
            if (End.ToCharArray().First() != 'd')
            {
                int SecondNumber = End.ToCharArray().Last() == ',' ? int.MaxValue : Convert.ToInt32(End.Substring(End.IndexOf(',') + 1));
                int FirstNumber = End.ToCharArray().Last() == ',' ? int.MinValue : Convert.ToInt32(End.Remove(End.IndexOf(',')));
                int IntInput;
                try { IntInput = Convert.ToInt32(Input); }
                catch { return ConfirmAndReturn(Values, Input); }
                return IntInput >= FirstNumber && IntInput <= SecondNumber ? TriState.True : ConfirmAndReturn(Values, Input);
            }
            else
            {
                End = End == "d" ? "," : End.Substring(1);
                double SecondNumber = End.ToCharArray().Last() == ',' ? int.MaxValue : Convert.ToDouble(End.Substring(End.IndexOf(',') + 1));
                double FirstNumber = End.ToCharArray().Last() == ',' ? int.MinValue : Convert.ToDouble(End.Remove(End.IndexOf(',')));
                double DoubleInput;
                try { DoubleInput = Convert.ToDouble(Input); }
                catch { return ConfirmAndReturn(Values, Input); }
                return DoubleInput >= FirstNumber && DoubleInput <= SecondNumber ? TriState.True : ConfirmAndReturn(Values, Input);
            }
        }
        else
        {
            Console.WriteLine("Unknown Configuration, please report an Error, the Program is still working but not as expected");
            return ConfirmAndReturn(Values, Input);
        }
    }
    public static TwoValues<TriState, string> ConfirmInputReturn(string[]? Values = null, string? Config = null)
    {
        string Input = Console.ReadLine() ?? "";
        TriState state = ConfirmAndReturn(Values, Input, Config);
        return new TwoValues<TriState, string>
        {
            Value1 = state,
            Value2 = Input
        };
    }

        public static void PressKey()
    {
        if (!Console.IsInputRedirected)
        {   
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Input is redirected, Press Enter to continue...");
            Console.Read(); //This is to prevent errors when input is redirected
        }
        Console.WriteLine();
    }
}