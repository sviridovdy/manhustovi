using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using manhustovi.admin.Models;
using Microsoft.Extensions.Logging;

namespace manhustovi.admin.Repositories
{
	public class PostsRepository
	{
		private const string TableName = "posts";

		private readonly AmazonDynamoDBClient _client;

		private readonly ILogger<PostsRepository> _logger;

		public PostsRepository(AmazonDynamoDBClient client, ILogger<PostsRepository> logger)
		{
			_client = client;
			_logger = logger;
			_logger.LogInformation($"Table name is set to '{TableName}'");
		}

		public async Task PutItemAsync(Post post)
		{
			using (_logger.BeginScope($"Save a post with timestamp {post.UnixTimestamp}"))
			{
				var request = new PutItemRequest(TableName, post.ToDictionary());
				var sw = Stopwatch.StartNew();
				var result = await _client.PutItemAsync(request);
				_logger.LogInformation($"response status = {result.HttpStatusCode}; time = {(int) sw.ElapsedMilliseconds} ms");
				if (result.HttpStatusCode != HttpStatusCode.OK)
				{
					throw new Exception($"Failed to save a post with timestamp {post.UnixTimestamp}. Response status {result.HttpStatusCode}");
				}
			}
		}

		public async Task<int> GetNextPostIdAsync()
		{
			using (_logger.BeginScope("Getting next post id"))
			{
				var request = new QueryRequest(TableName)
				{
					Limit = 1,
					KeyConditionExpression = "id = :v_Id",
					ExpressionAttributeValues = new Dictionary<string, AttributeValue>
					{
						[":v_Id"] = new AttributeValue("post")
					},
					ProjectionExpression = "postId",
					ScanIndexForward = false
				};
				var sw = Stopwatch.StartNew();
				var result = await _client.QueryAsync(request);
				_logger.LogInformation($"response status = {result.HttpStatusCode}; time = {(int) sw.ElapsedMilliseconds} ms");
				if (result.HttpStatusCode != HttpStatusCode.OK)
				{
					throw new Exception($"Failed to get next post id. Response status {result.HttpStatusCode}");
				}

				var lastPost = result.Items.Single();
				var newPostId = int.Parse(lastPost["postId"].N) + 1;
				_logger.LogInformation($"Providing next post id = {newPostId}");
				return newPostId;
			}
		}
	}
}