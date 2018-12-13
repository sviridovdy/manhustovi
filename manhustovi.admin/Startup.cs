using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;
using manhustovi.admin.Repositories;
using manhustovi.admin.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace manhustovi.admin
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
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options => options.Events.OnRedirectToLogin = context =>
				{
					context.Response.StatusCode = 401;
					return Task.CompletedTask;
				});

			services.AddMvc(options =>
			{
				var policy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
				options.Filters.Add(new AuthorizeFilter(policy));
			}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });

			var endpoint = RegionEndpoint.GetBySystemName(Configuration["endpoint"]);
			var credentials = new BasicAWSCredentials(Configuration["accessKey"], Configuration["secretKey"]);
			var dynamoDbClient = new AmazonDynamoDBClient(credentials, endpoint);
			var s3Client = new AmazonS3Client(credentials, endpoint);

			services.AddSingleton(dynamoDbClient);
			services.AddSingleton(s3Client);
			services.AddSingleton<PostsRepository>();
			services.AddSingleton<PhotosRepository>();
			services.AddSingleton<PostsService>();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

			app.UseAuthentication();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller}/{action=Index}/{id?}");
			});

			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = "ClientApp";

				if (env.IsDevelopment())
				{
					spa.UseAngularCliServer("start");
				}
			});
		}
	}
}
