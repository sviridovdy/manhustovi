using Amazon.DynamoDBv2.Model;

namespace manhustovi.app.Models
{
    public class PhotoAttachment
    {
        public string PreviewSrc { get; }

        public string MobileSrc { get; }

        public string Src { get; }

        public int Width { get; }

        public int Height { get; }

        public PhotoAttachment(AttributeValue photoAttachment)
        {
            PreviewSrc = photoAttachment.M["previewSrc"].S;
            MobileSrc = photoAttachment.M["mobileSrc"].S;
            Src = photoAttachment.M["src"].S;
            Width = int.Parse(photoAttachment.M["width"].N);
            Height = int.Parse(photoAttachment.M["height"].N);
        }
    }
}