using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using manhustovi.app.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace manhustovi.app
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });

			var endpoint = RegionEndpoint.GetBySystemName(Configuration["endpoint"]);
			var credentials = new BasicAWSCredentials(Configuration["accessKey"], Configuration["secretKey"]);
			var dynamoDbClient = new AmazonDynamoDBClient(credentials, endpoint);

			services.AddSingleton(new DynamoDbRepository(dynamoDbClient));
			services.AddSingleton<PostsRepository>();
			services.AddSingleton<AlbumsRepository>();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseStaticFiles();
			app.UseSpaStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller}/{action=Index}/{id?}");
			});
			applicationLifetime.ApplicationStarted.Register(() =>
			{
				app.ApplicationServices.GetService<PostsRepository>().LoadData();
				app.ApplicationServices.GetService<AlbumsRepository>().LoadData();
			});
			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = "ClientApp";

				if (env.IsDevelopment())
				{
					spa.UseAngularCliServer(npmScript: "start");
				}
			});
		}
	}
}