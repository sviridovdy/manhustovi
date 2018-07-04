using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.Model;
using manhustovi.app.Models;

namespace manhustovi.app.Repositories
{
    public class AlbumsRepository
    {
        private const string AlbumsTableName = "albums";

        private readonly DynamoDbRepository dynamoDbRepository;

        private Album[] albums;

        private bool updating;

        public AlbumsRepository(DynamoDbRepository dynamoDbRepository)
        {
            this.dynamoDbRepository = dynamoDbRepository;
            albums = new Album[0];
        }

        public void LoadData()
        {
            lock (this)
            {
                if (!updating)
                {
                    updating = true;
                    LoadAlbums();
                }
            }
        }

        public Album GetAlbum(string id)
        {
            lock (this)
            {
                return albums.First(a => a.Id == id);
            }
        }

        private async void LoadAlbums()
        {
            try
            {
                Console.WriteLine("Starting to load albums");
                var currentKey = new Dictionary<string, AttributeValue>();
                var albumsList = new List<Album>();
                do
                {
                    var request = new QueryRequest
                    {
                        TableName = AlbumsTableName,
                        ExclusiveStartKey = currentKey,
                        KeyConditionExpression = "id = :v_album",
                        ExpressionAttributeValues = {{":v_album", new AttributeValue {S = "album"}}}
                    };
                    var result = await dynamoDbRepository.QueryAsync(request);
                    albumsList.AddRange(result.Items.Select(i => new Album(i)));
                    currentKey = result.LastEvaluatedKey;
                } while (currentKey.Any());

                Console.WriteLine("Done loading {0} albums", albumsList.Count);
                lock (this)
                {
                    albums = Enumerable.Reverse(albumsList).ToArray();
                    updating = false;
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }
        }

        public AlbumAttachment GetAlbumAttachment(string albumId)
        {
            if (albumId== null) return null;
            lock (this)
            {
                var album = albums.First(a => a.Id == albumId);
                return new AlbumAttachment(album);
            }
        }
    }
}