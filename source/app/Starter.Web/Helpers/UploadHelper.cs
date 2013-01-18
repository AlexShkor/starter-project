using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using MongoDB.Bson;

namespace DQF.Web.Helpers
{
    public class UploadHelper
    {
        private const string BucketName = "bucket";
        private const string BaseDocsUrl = "https://bucket.s3.amazonaws.com/docs/";
        private readonly SiteSettings _settings;

        public UploadHelper(SiteSettings settings)
        {
            _settings = settings;
        }

        public string GenerateFilename(string originalFileName)
        {
            return ObjectId.GenerateNewId().ToString() + Path.GetExtension(originalFileName);
        }

        public void UploadFileOnAmazonS3(string filename, Stream inputStream)
        {
            using (AmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(_settings.AmazonAccessKey, _settings.AmazonSecretKey))
            {
                var request = new PutObjectRequest();
                request.WithBucketName(BucketName)
                    .WithCannedACL(S3CannedACL.PublicRead)
                    .WithKey("docs/" + filename).InputStream = inputStream;
                S3Response response = client.PutObject(request);
            }
        }

        public static string BuildDocumentUrl(string filename)
        {
            return BaseDocsUrl + filename;
        }
    }
}