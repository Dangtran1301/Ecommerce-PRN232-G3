namespace Gateway.API;

public class NoBufferingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.ExpectContinue = false;
        request.Version = new Version(2, 0);
        request.Content ??= new StreamContent(Stream.Null);
        return await base.SendAsync(request, cancellationToken);
    }
}
