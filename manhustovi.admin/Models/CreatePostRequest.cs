using System.Collections.Generic;

namespace manhustovi.admin.Models
{
	public class CreatePostRequest
	{
		public CreatePostRequest(string hashtag, int dayNumber, string text)
		{
			Hashtag = hashtag;
			DayNumber = dayNumber;
			Text = text;
			Photos = new List<byte[]>();
		}

		public string Hashtag { get; }

		public int DayNumber { get; }

		public string Text { get; }

		public List<byte[]> Photos { get; }
	}
}