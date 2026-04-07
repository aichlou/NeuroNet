using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Xml;
using NeuroNet.Core;

namespace NeuroNet.CLI;

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
            int SecondNumber = Convert.ToInt32(End
                .Substring(End.IndexOf(',')));
            int FirstNumber = Convert.ToInt32(End
                .Remove(End.Length - End.IndexOf(',')));
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