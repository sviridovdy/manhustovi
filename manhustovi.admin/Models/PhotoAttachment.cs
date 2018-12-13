using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;

namespace manhustovi.admin.Models
{
	public class PhotoAttachment
	{
		public string PreviewSrc { get; set; }

		public string MobileSrc { get; set; }

		public string Src { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public Dictionary<string, AttributeValue> ToDictionary()
		{
			return new Dictionary<string, AttributeValue>
			{
				["previewSrc"] = new AttributeValue(PreviewSrc),
				["mobileSrc"] = new AttributeValue(MobileSrc),
				["src"] = new AttributeValue(Src),
				["width"] = new AttributeValue {N = Width.ToString()},
				["height"] = new AttributeValue {N = Height.ToString()}
			};
		}
	}
}