using AzureAIContentSafety.API.DTO.Responses;

namespace AzureAIContentSafety.API.Utils
{
    public static class ErrorResponseUtil
    {
        public static ErrorResponse CreateErrorResponse(
            HttpContext context,
            int status,
            string title,
            Dictionary<string, string[]> errors = null
        )
        {
            var sectionCode = status switch
            {
                400 => "15.5.1",
                404 => "15.5.5",
                500 => "15.6.1",
                _ => string.Empty,
            };

            return new ErrorResponse
            {
                Type = $"https://tools.ietf.org/html/rfc9110#section-{sectionCode}",
                Title = title,
                Status = status,
                Errors = errors ?? new Dictionary<string, string[]>(),
                TraceId = context.TraceIdentifier,
            };
        }
    }
}
