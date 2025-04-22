using Amazon.S3;
using Amazon.S3.Model;

namespace PedalPricerServer.Services
{
    public class FileService(IConfiguration configuration)
    {
        private string? _bucketName => configuration.GetValue<string>("AWS:BucketName");
        private string? _region => configuration.GetValue<string>("AWS:Region");
        private string? _accessKey => configuration.GetValue<string>("AWS:AccessKey");
        private string? _secretKey => configuration.GetValue<string>("AWS:SecretKey");

        public async Task<Stream> ReadImage(string key, string filename)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = $"{key}/{filename}",
                };
                using var client = new AmazonS3Client(_accessKey, _secretKey, Amazon.RegionEndpoint.GetBySystemName(_region));
                using var getObjectResponse = await client.GetObjectAsync(request);
                using var responseStream = getObjectResponse.ResponseStream;
                var stream = new MemoryStream();
                await responseStream.CopyToAsync(stream);
                stream.Position = 0;
                return stream;
            }
            catch (Exception e)
            {
                throw new AggregateException(e);
            }
        }
    }
}
