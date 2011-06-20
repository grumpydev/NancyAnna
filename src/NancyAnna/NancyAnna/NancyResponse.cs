namespace NancyAnna
{
    using System;
    using System.IO;
    using System.Reactive.Linq;
    using System.Text;

    using Nancy;

    public class NancyResponse : Anna.Responses.Response
    {
        private readonly string body;

        public NancyResponse(Response response)
        {
            this.Headers = response.Headers;
            this.StatusCode = (int)response.StatusCode;

            if (!this.Headers.ContainsKey("Content-Type"))
            {
                this.Headers["Content-Type"] = "text/html";
            }

            // Not sure how to map the delegate to what Anna expects,
            // so grab the body contents and stick it in a string.
            // Probably not the most efficient way of doing it :-)
            var bodyStream = new MemoryStream();
            response.Contents.Invoke(bodyStream);
            this.body = Encoding.UTF8.GetString(bodyStream.ToArray());

            this.WriteStream = Write;
        }

        public IObservable<Stream> Write(Stream stream)
        {
            var bytes = Encoding.UTF8.GetBytes(this.body);
            return Observable.FromAsyncPattern<byte[], int, int>(stream.BeginWrite, stream.EndWrite)(bytes, 0, bytes.Length)
                .Select(u => stream);
        }

    }
}