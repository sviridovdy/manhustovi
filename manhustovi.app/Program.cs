using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace manhustovi.app
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.UseKestrel(options =>
				{
					var env = options.ApplicationServices.GetService<IHostingEnvironment>();
					var config = options.ApplicationServices.GetService<IConfiguration>();
					var certificatePath = Path.Combine(env.ContentRootPath, "manhustovi.pfx");
					var cert = new X509Certificate2(certificatePath, config["certificatePassword"]);
					options.ConfigureHttpsDefaults(adapterOptions => adapterOptions.ServerCertificate = cert);
				});
	}
}