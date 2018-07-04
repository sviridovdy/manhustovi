using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using manhustovi.app.Models;

namespace manhustovi.app.Repositories
{
    public class PostsRepository
    {
        private const string PostsTableName = "posts";

        private readonly DynamoDbRepository dynamoDbRepository;

        private readonly SemaphoreSlim semaphore;

        private Post[] posts;

        public int PostsCount => posts.Length;

        public PostsRepository(DynamoDbRepository dynamoDbRepository)
        {
            this.dynamoDbRepository = dynamoDbRepository;
            posts = new Post[0];
            semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task LoadData()
        {
            if (semaphore.Wait(0))
            {
                try
                {
                    Console.WriteLine("Starting to load posts");
                    posts = new Post[0];
                    var currentKey = new Dictionary<string, AttributeValue>();
                    await PullData(currentKey);
                    Console.WriteLine("Done loading {0} posts", posts.Length);
                }
                catch (Exception x)
                {
                    Console.WriteLine(x);
                }
                finally
                {
                    semaphore.Release();
                }
            }
        }

        public async Task Update()
        {
            if (semaphore.Wait(0))
            {
                try
                {
                    Console.WriteLine("Starting to update posts");
                    Dictionary<string, AttributeValue> currentKey;
                    var postCount = posts.Length;
                    var lastPost = posts.FirstOrDefault();
                    if (lastPost != null)
                    {
                        Console.WriteLine($"Using post {lastPost.UnixTimestamp} as last one.");
                        currentKey = lastPost.CreatePrimaryKey();
                    }
                    else
                    {
                        currentKey = new Dictionary<string, AttributeValue>();
                    }

                    await PullData(currentKey);
                    Console.WriteLine("Done loading {0} posts", posts.Length - postCount);
                }
                catch (Exception x)
                {
                    Console.WriteLine(x);
                }
                finally
                {
                    semaphore.Release();
                }
            }
        }

        private async Task PullData(Dictionary<string, AttributeValue> currentKey)
        {
            do
            {
                var request = new QueryRequest
                {
                    TableName = PostsTableName,
                    ExclusiveStartKey = currentKey,
                    KeyConditionExpression = "id = :v_post",
                    ExpressionAttributeValues = {{":v_post", new AttributeValue {S = "post"}}}
                };
                var result = await dynamoDbRepository.QueryAsync(request);
                posts = posts.Concat(result.Items.Select(i => new Post(i))).OrderByDescending(p => p.UnixTimestamp).ToArray();
                currentKey = result.LastEvaluatedKey;
            } while (currentKey.Any());
        }

        public Post[] GetPosts(int offset, int count)
        {
            lock (this)
            {
                return posts.Skip(offset).Take(count).ToArray();
            }
        }

        public Post[] GetPost(string id)
        {
            lock (this)
            {
                return posts.Where(p => p.Id == id).ToArray();
            }
        }
    }
}