using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.Model;

namespace manhustovi.app.Models
{
    public class Album
    {
        public string Id { get; }

        public int Size { get; }

        public string Title { get; }

        public int CreatedTimestamp { get; set; }

        public PhotoAttachment Thumb { get; }

        public PhotoAttachment[] Photos { get; set; }

        public Album(Dictionary<string, AttributeValue> attributeValues)
        {
            Id = attributeValues["albumId"].N;
            Size = int.Parse(attributeValues["size"].N);
            Title = attributeValues["title"].S;
            CreatedTimestamp = int.Parse(attributeValues["created"].N);
            Thumb = new PhotoAttachment(attributeValues["thumb"]);
            Photos = attributeValues["photos"].L.Select(a => new PhotoAttachment(a)).ToArray();
        }
    }
}