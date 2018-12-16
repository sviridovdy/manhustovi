using System;
using System.Linq;
using System.Threading.Tasks;
using manhustovi.admin.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace manhustovi.admin.Controllers
{
	[Route("api/[controller]")]
	public class PostEventsController : Controller
	{
		[HttpGet("{id}")]
		public async Task Get([FromServices] PostsService postsService, [FromRoute] Guid id)
		{
			Response.Headers.Add("Content-Type", "text/event-stream");
			Response.Headers.Add("Cache-Control", "no-cache");
			while (true)
			{
				var events = postsService.GetEvents(id);
				if (events == null)
				{
					break;
				}

				if (events.Length > 0)
				{
					var jEvents = new JArray(events.Select(e => new JValue(e)));
					await Response.WriteAsync($"data: {jEvents.ToString(Formatting.None)}\n\n");
					Response.Body.Flush();
				}

				await Task.Delay(TimeSpan.FromSeconds(1));
			}
		}
	}
}