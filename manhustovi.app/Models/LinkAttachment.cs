using Amazon.DynamoDBv2.Model;

namespace manhustovi.app.Models
{
    public class LinkAttachment
    {
        public LinkAttachment(AttributeValue linkAttachment)
        {
            Url = linkAttachment.M["url"].S;
            Title = linkAttachment.M["title"].S;
        }

        public string Title { get; }

        public string Url { get; }
    }
}