using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.Model;

namespace manhustovi.admin.Models
{
	public class Post
	{
		public int Id { get; }

		public string Text { get; }

		public string HashTag { get; }

		public int UnixTimestamp { get; }

		public int DayNumber { get; }

		public List<PhotoAttachment> PhotoAttachments { get; }

		public Post(int id, string text, string hashTag, int unixTimestamp, int dayNumber, List<PhotoAttachment> photoAttachments)
		{
			Id = id;
			Text = text;
			HashTag = hashTag;
			UnixTimestamp = unixTimestamp;
			DayNumber = dayNumber;
			PhotoAttachments = photoAttachments;
		}

		public Dictionary<string, AttributeValue> ToDictionary()
		{
			var map = new Dictionary<string, AttributeValue>
			{
				["id"] = new AttributeValue("post"),
				["postId"] = new AttributeValue {N = Id.ToString()},
				["postText"] = new AttributeValue(Text),
				["hashTag"] = new AttributeValue(HashTag),
				["unixTimestamp"] = new AttributeValue {N = UnixTimestamp.ToString()},
				["dayNumber"] = new AttributeValue {N = DayNumber.ToString()}
			};

			if (PhotoAttachments.Any())
			{
				map["photoAttachments"] = new AttributeValue
				{
					L = PhotoAttachments.Select(a => new AttributeValue
					{
						M = a.ToDictionary()
					}).ToList()
				};
			}

			return map;
		}
	}
}