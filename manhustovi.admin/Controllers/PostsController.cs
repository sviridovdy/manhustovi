using System.IO;
using manhustovi.admin.Models;
using manhustovi.admin.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace manhustovi.admin.Controllers
{
	[Route("api/[controller]")]
	public class PostsController : Controller
	{
		[HttpPost]
		[RequestSizeLimit(104_857_600)]
		public IActionResult Create([FromServices] PostsService postsService)
		{
			var createPostRequest = JsonConvert.DeserializeObject<CreatePostRequest>(Request.Form["post"].ToString());
			foreach (var formFile in Request.Form.Files.GetFiles("photo"))
			{
				using (var stream = formFile.OpenReadStream())
				{
					using (var ms = new MemoryStream())
					{
						stream.CopyTo(ms);
						createPostRequest.Photos.Add(ms.ToArray());
					}
				}
			}

			var createPostResponse = postsService.CreatePost(createPostRequest);
			return Json(createPostResponse);
		}
	}
}