namespace NancyAnna
{
    using System;
    using System.IO;
    using System.Reactive.Linq;
    using System.Text;

    using Nancy;

    public class NancyResponse : Anna.Responses.Response
    {
        private readonly byte[] body;

        public NancyResponse(Response response)
        {
            this.Headers = response.Headers;
            this.StatusCode = (int)response.StatusCode;

            this.Headers["Content-Type"] = response.ContentType;

            // Not sure how to map the delegate to what Anna expects,
            // so grab the body contents and stick it in a string.
            // Probably not the most efficient way of doing it :-)
            var bodyStream = new MemoryStream();
            response.Contents.Invoke(bodyStream);
            this.body = bodyStream.ToArray();
        }

        public override IObservable<Stream> WriteStream(Stream stream)
        {
            var bytes = this.body;
            return Observable.FromAsyncPattern<byte[], int, int>(stream.BeginWrite, stream.EndWrite)(bytes, 0, bytes.Length)
               .Select(u => stream);
        }
    }
}