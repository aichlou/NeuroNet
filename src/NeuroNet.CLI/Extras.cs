namespace NeuroNet.CLI;

public class Extras
{
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
    }
}