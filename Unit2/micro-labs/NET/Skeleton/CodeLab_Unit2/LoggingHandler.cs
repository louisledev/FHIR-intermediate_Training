namespace CodeLab_Unit2;

public class LoggingHandler : DelegatingHandler
{
    public LoggingHandler(HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    {
        // Log the request URI
        string body = "";
        if (request.Content != null)
        {
            body = await request.Content.ReadAsStringAsync(cancellationToken);
        }
        Console.WriteLine($"Request URI: {request.RequestUri}, Method: {request.Method}, Body: {body}");

        // Proceed with the request
        var response = await base.SendAsync(request, cancellationToken);

        // You could also log response details here if desired

        return response;
    }
}