using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace manhustovi.app
{
	public class DynamicRoutesMiddleware
	{
		private readonly RequestDelegate _next;

		public DynamicRoutesMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var dynamicRoutes = new Dictionary<string, string>();
			context.RequestServices.GetService<IConfiguration>().Bind("dynamicRoutes", dynamicRoutes);
			if (dynamicRoutes.TryGetValue(context.Request.Path, out var content))
			{
				await context.Response.WriteAsync(content);
			}
			else
			{
				await _next(context);
			}
		}
	}
}