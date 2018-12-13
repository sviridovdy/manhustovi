using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using manhustovi.admin.Models;
using manhustovi.admin.Repositories;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace manhustovi.admin.Services
{
	public class PostsService
	{
		private readonly PostsRepository _postsRepository;

		private readonly PhotosRepository _photosRepository;

		private readonly ILogger<PostsService> _logger;

		private readonly SemaphoreSlim _semaphore;

		public PostsService(PostsRepository postsRepository, PhotosRepository photosRepository, ILogger<PostsService> logger)
		{
			_postsRepository = postsRepository;
			_photosRepository = photosRepository;
			_logger = logger;
			var cpuCount = Environment.ProcessorCount;
			_logger.LogInformation($"Cpu count = {cpuCount}");
			_semaphore = new SemaphoreSlim(cpuCount, cpuCount);
		}

		public async Task CreatePostAsync(CreatePostRequest createPostRequest)
		{
			using (_logger.BeginScope($"Creating post for day {createPostRequest.DayNumber}"))
			{
				var sw = Stopwatch.StartNew();

				var nextPostIdTask = _postsRepository.GetNextPostIdAsync();
				var timeStamp = (int) (DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;

				_logger.LogInformation($"timestamp for post = {timeStamp}");

				var photoAttachments = new List<PhotoAttachment>();
				var photoTasks = new List<Task>();

				_logger.LogInformation($"# of photos to process = {createPostRequest.Photos.Count}");
				for (var photoIndex = 0; photoIndex < createPostRequest.Photos.Count; photoIndex++)
				{
					var originalKey = $"posts/{timeStamp}/pa-original-{photoIndex}.jpg";
					var mobileKey = $"posts/{timeStamp}/pa-mobile-{photoIndex}.jpg";
					var previewKey = $"posts/{timeStamp}/pa-preview-{photoIndex}.jpg";

					var photoAttachment = new PhotoAttachment();
					var photoBytes = createPostRequest.Photos[photoIndex];
					var originalResizeTask = ResizeAndUploadPhotoAsync(photoBytes, 2560, originalKey).ContinueWith(task =>
					{
						photoAttachment.Width = task.Result.width;
						photoAttachment.Height = task.Result.height;
						photoAttachment.Src = task.Result.location;
					});
					var mobileResizeTask = ResizeAndUploadPhotoAsync(photoBytes, 1280, mobileKey).ContinueWith(task =>
					{
						photoAttachment.MobileSrc = task.Result.location;
					});
					var previewResizeTask = ResizeAndUploadPhotoAsync(photoBytes, 640, previewKey).ContinueWith(task =>
					{
						photoAttachment.PreviewSrc = task.Result.location;
					});
					photoTasks.AddRange(new[] {originalResizeTask, mobileResizeTask, previewResizeTask});
					photoAttachments.Add(photoAttachment);
				}

				var nextPostId = await nextPostIdTask;
				var post = new Post(nextPostId, createPostRequest.Text, createPostRequest.Hashtag, timeStamp, createPostRequest.DayNumber, photoAttachments);
				var savePostTask = _postsRepository.PutItemAsync(post);

				await Task.WhenAll(photoTasks.Append(savePostTask));
				_logger.LogInformation($"post created within {(int) sw.ElapsedMilliseconds} ms");
			}
		}

		private async Task<(string location, int width, int height)> ResizeAndUploadPhotoAsync(byte[] photoBytes, int desiredWidth, string key)
		{
			await _semaphore.WaitAsync();
			try
			{
				var sw = Stopwatch.StartNew();
				var (width, height, data) = ResizeImage(photoBytes, desiredWidth);
				var resizeTime = (int) sw.ElapsedMilliseconds;
				var location = await _photosRepository.PutPublicItemAsync(key, data);
				_logger.LogInformation($"Resize of image {key} took {resizeTime} ms");
				return (location, width, height);
			}
			finally
			{
				_semaphore.Release();
			}
		}

		private (int width, int height, byte[] data) ResizeImage(byte[] data, int maxWidth)
		{
			var input = new MemoryStream(data);
			using (var inputStream = new SKManagedStream(input))
			{
				using (var original = SKBitmap.Decode(inputStream))
				{
					var width = maxWidth;
					var height = original.Height * maxWidth / original.Width;
					using (var resized = original.Resize(new SKImageInfo(width, height), SKFilterQuality.High))
					{
						using (var image = SKImage.FromBitmap(resized))
						{
							using (var output = new MemoryStream())
							{
								image.Encode(SKEncodedImageFormat.Jpeg, 100).SaveTo(output);
								return (original.Width, original.Height, output.ToArray());
							}
						}
					}
				}
			}
		}
	}
}