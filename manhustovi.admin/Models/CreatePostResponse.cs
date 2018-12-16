using System;

namespace manhustovi.admin.Models
{
	public class CreatePostResponse
	{
		public CreatePostResponse(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; }
	}
}