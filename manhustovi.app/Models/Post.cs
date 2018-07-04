using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.Model;

namespace manhustovi.app.Models
{
    public class Post
    {
        public string Id { get; }

        public string Text { get; }

        public string HashTag { get; }

        public int UnixTimestamp { get; }

        public int DayNumber { get; }

        public List<PhotoAttachment> PhotoAttachments { get; } = new List<PhotoAttachment>();

        public List<VideoAttachment> VideoAttachments { get; } = new List<VideoAttachment>();

        public string AlbumId { get; }

        public LinkAttachment LinkAttachment { get; }

        public Post(Dictionary<string, AttributeValue> attributeValues)
        {
            Id = attributeValues["postId"].N;
            Text = attributeValues["postText"].S;
            HashTag = attributeValues["hashTag"].S;
            UnixTimestamp = int.Parse(attributeValues["unixTimestamp"].N);
            DayNumber = int.Parse(attributeValues["dayNumber"].N);
            if (attributeValues.TryGetValue("photoAttachments", out AttributeValue photoAttachments))
            {
                PhotoAttachments.AddRange(photoAttachments.L.Select(a => new PhotoAttachment(a)));
            }
            if (attributeValues.TryGetValue("videoAttachments", out AttributeValue videoAttachments))
            {
                VideoAttachments.AddRange(videoAttachments.L.Select(a => new VideoAttachment(a)));
            }
            if (attributeValues.TryGetValue("albumAttachment", out AttributeValue albumId))
            {
                AlbumId = albumId.N;
            }
            if (attributeValues.TryGetValue("linkAttachment", out AttributeValue linkAttachment))
            {
                LinkAttachment = new LinkAttachment(linkAttachment);
            }
        }

        public Dictionary<string, AttributeValue> CreatePrimaryKey()
        {
            return new Dictionary<string, AttributeValue>
            {
                {"id", new AttributeValue {S = "post"}},
                {"unixTimestamp", new AttributeValue {N = UnixTimestamp.ToString()}}
            };
        }
    }
}