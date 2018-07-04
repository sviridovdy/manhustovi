using Amazon.DynamoDBv2.Model;

namespace manhustovi.app.Models
{
    public class VideoAttachment
    {
        public string Src { get; }

        public VideoAttachment(AttributeValue videoAttachment)
        {
            Src = videoAttachment.M["src"].S;
        }
    }
}