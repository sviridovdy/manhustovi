using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace manhustovi.admin.Repositories
{
	public class PhotosRepository
	{
		private const string BaseUrl = "https://s3.eu-central-1.amazonaws.com/manhustovi.life.media/";

		private const string BucketName = "manhustovi.life.media";

		private readonly AmazonS3Client _client;

		private readonly ILogger<PhotosRepository> _logger;

		public PhotosRepository(AmazonS3Client client, ILogger<PhotosRepository> logger)
		{
			_client = client;
			_logger = logger;
			_logger.LogInformation($"Base url is set to '{BaseUrl}'");
			_logger.LogInformation($"Bucket in use '{BucketName}'");
		}

		public async Task<string> PutPublicItemAsync(string objectKey, byte[] data)
		{
			using (_logger.BeginScope($"Creating an object '{objectKey}'"))
			{
				_logger.LogInformation($"size = {data.Length}");
				var request = new PutObjectRequest
				{
					Key = objectKey,
					BucketName = BucketName,
					InputStream = new MemoryStream(data),
					CannedACL = S3CannedACL.PublicRead
				};
				var sw = Stopwatch.StartNew();
				var result = await _client.PutObjectAsync(request);
				_logger.LogInformation($"response status = {result.HttpStatusCode}; time = {(int) sw.ElapsedMilliseconds} ms");
				if (result.HttpStatusCode != HttpStatusCode.OK)
				{
					throw new Exception($"Failed to put an object to s3 with key {objectKey} and data length = {data.Length}. Response status {result.HttpStatusCode}");
				}
			}

			return $"{BaseUrl}{objectKey}";
		}
	}
}