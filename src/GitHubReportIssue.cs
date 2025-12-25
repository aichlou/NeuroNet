namespace NeuroNet;

public static class GitHubReportIssue
{
    public static void ReportToGitHub(string title, string errorMessage, string stackTrace, string sidenote, bool UserExperience)
    {
        Console.WriteLine("Do you want to report the Issue to GitHub? (y/n)");
        string reportChoice = Console.ReadLine() ?? "y";
        if (reportChoice.ToLower() == "y")
        {
            Console.WriteLine("Please describe the issue you encountered:");
            string userDescription = Console.ReadLine() ?? "";
            Console.WriteLine("Opening GitHub Issues page...");
            //string title = title;
            string body = $"Error Message: {errorMessage}\n\nStack Trace:\n{stackTrace}\n\nSidenote: {sidenote}\n\nUser Description: {userDescription}";
            string url = $"github.com/aichlou/NeuroNet/issues/new?title={Uri.EscapeDataString(title)}&body={Uri.EscapeDataString(body)}";
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to open browser: " + ex.Message);
            }
        }
    }
}