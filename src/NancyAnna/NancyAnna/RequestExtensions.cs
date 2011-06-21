namespace NancyAnna
{
    using System.IO;
    using System.Reactive.Linq;

    using Nancy.IO;

    public static class RequestExtensions
    {
        public static Nancy.Request ToNancyRequest(this Anna.Request.Request request)
        {
            return new Nancy.Request(
                request.HttpMethod,
                request.Url.AbsolutePath,
                request.Headers,
                new RequestStream(request.InputStream, 0, true),
                "http");
        }
       }
}