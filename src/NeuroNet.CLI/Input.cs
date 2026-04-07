using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Xml;
using NeuroNet.Core;
using NeuroNet.CLI;
using Microsoft.VisualBasic;

var result =InputCLI.ConfirmInputReturn();
if (result.Value1 == InputCLI.TriState.True) Console.WriteLine("This is Valid");
else if (result.Value1 == InputCLI.TriState.False) Console.WriteLine("This isn't Valid");
else if (result.Value1 == InputCLI.TriState.Return) Console.WriteLine("Return");




public class InputCLI
{

    public enum TriState
    {
        True, False, Return
    }
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
            int SecondNumber;
            int FirstNumber;
            try {
                SecondNumber = Convert.ToInt32(End
                    .Substring(End.IndexOf(',') + 1));
                FirstNumber = Convert.ToInt32(End
                    .Remove(End.IndexOf(',') - 1));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("The configuration numbers is wrong. It is not guaranteed that the program will continue the right way. Please report an Issue on GitHub");
                return TriState.Return;
            }
            int IntInput;
            try { IntInput = Convert.ToInt32(Input); }
            catch { return TriState.False; }
            return IntInput > FirstNumber && IntInput < SecondNumber ? TriState.True : TriState.False;
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
}