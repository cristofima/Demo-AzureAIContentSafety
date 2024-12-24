namespace AzureAIContentSafety.API.Exceptions;

public class HarmfulContentException : Exception
{
    public Dictionary<string, string[]> Errors { get; set; }

    public HarmfulContentException(string message)
        : base(message) { }

    public HarmfulContentException(string message, Dictionary<string, string[]> errors)
        : base(message)
    {
        Errors = errors;
    }
}
