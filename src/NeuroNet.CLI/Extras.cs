namespace NeuroNet.CLI;

public class Extras
{
    public static bool IsReturn(string? input)
    {
        if (input == null) return false;
        return input.Trim().ToLower() == Program.returnString;
    }
}