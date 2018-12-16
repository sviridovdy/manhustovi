using System.Collections.Generic;

namespace manhustovi.admin.Models
{
	public class CreatePostRequest
	{
		public CreatePostRequest(string hashtag, int dayNumber, string text, string videoUrl)
		{
			Hashtag = hashtag;
			DayNumber = dayNumber;
			Text = text;
			VideoUrl = videoUrl;
			Photos = new List<byte[]>();
		}

		public string Hashtag { get; }

		public int DayNumber { get; }

		public string Text { get; }

		public string VideoUrl { get; }

		public List<byte[]> Photos { get; }
	}
}