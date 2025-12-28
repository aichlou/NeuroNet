namespace NeuroNet.Core;

public static class GitHubReportIssue
{
    public static void ReportToGitHub(string title, string errorMessage, string stackTrace, string sidenote, bool v, Action<string>? Message = null, Func<string>? readInput = null)
    {
        Message?.Invoke("Do you want to report the Issue to GitHub? (y/n)");
        string reportChoice = readInput?.Invoke() ?? "y";
        if (reportChoice.ToLower() == "y")
        {
            Message?.Invoke("Please describe the issue you encountered:");
            string userDescription = readInput?.Invoke() ?? "";
            Message?.Invoke("Opening GitHub Issues page...");

            string body = $"Error Message: {errorMessage}\n\nStack Trace:\n{stackTrace}\n\nSidenote: {sidenote}\n\nUser Description: {userDescription}";
            string url = $"https://github.com/aichlou/NeuroNet/issues/new?title={Uri.EscapeDataString(title)}&body={Uri.EscapeDataString(body)}";
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
                Message?.Invoke("Failed to open browser: " + ex.Message);
            }
        }
    }
}

public class MultipleValues<T1>
{
    public T1? Value { get; init; }
    public bool HasError { get; init; }
    public string? ErrorMessage { get; init; }
}

public class TwoValues<T1, T2>
{
    public T1? Value1 { get; set; }
    public T2? Value2 { get; set; }
}