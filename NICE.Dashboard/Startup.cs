using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NICE.Dashboard
{
	public class Startup
	{
		private readonly IConfiguration Configuration;

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddHealthChecksUI(setupSettings: setup =>
				{
					setup.MaximumHistoryEntriesPerEndpoint(100); //limits database size.
				})
				.AddSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHealthChecksUI(setupOptions: setup =>
				{
					setup.UIPath = "/";
					setup.AddCustomStylesheet("wwwroot/NICE.Style.css");
				});
			});
		}
	}
}
