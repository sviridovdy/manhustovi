using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace manhustovi.app.Repositories
{
    public class DynamoDbRepository
    {
        private readonly AmazonDynamoDBClient client;

        private const int ReadCapacity = 10;

        public DynamoDbRepository(AmazonDynamoDBClient client)
        {
            this.client = client;
        }

        public async Task<QueryResponse> QueryAsync(QueryRequest request)
        {
            var stopwatch = Stopwatch.StartNew();
            request.Limit = 5;
            request.ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL;
            var result = await client.QueryAsync(request);
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("Received {0} items in total {1} bytes.", result.Count, result.ContentLength);
            Console.WriteLine("Consumed {0} unit in {1} seconds", result.ConsumedCapacity?.CapacityUnits, TimeSpan.FromMilliseconds(elapsedMilliseconds).ToString("g"));
            var expectedTimeSeconds = (result.ConsumedCapacity?.CapacityUnits ?? 0) / ReadCapacity;
            var diff = expectedTimeSeconds - elapsedMilliseconds / 1000.0;
            if (diff > 0)
            {
                Console.WriteLine("Waiting {0} ({1} * 1.25) seconds", diff * 1.25, diff);
                await Task.Delay(TimeSpan.FromSeconds(diff * 1.25));
            }

            return result;
        }
    }
}
