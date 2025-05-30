using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Diagnostics;

namespace PedalPricerServer.Services
{
    public class FileService()
    {
        private readonly string? _bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME");

        public async Task<Stream> ReadFile(string key, string filename)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{key}/{filename}",
            };
            using var client = new AmazonS3Client();
            using var getObjectResponse = await client.GetObjectAsync(request);
            using var responseStream = getObjectResponse.ResponseStream;
            var stream = new MemoryStream();
            await responseStream.CopyToAsync(stream);
            stream.Position = 0;
            return stream;
        }
    }
}
