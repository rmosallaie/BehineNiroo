using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Internal;

namespace BehineNiroo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddDbContext<BehineNirooContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("MySqlConnectionSecurityDb"),
                mySqlOptionsAction: mysqlActions =>
                {
                    mysqlActions.EnableRetryOnFailure();
                })
            );
            //services.AddDbContext<BehineNirooContext>(options =>
            //        options.UseMySql(Configuration.GetConnectionString("MySqlConnectionEnergyDb")));


            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<BehineNirooContext>().AddDefaultTokenProviders();

            //services.AddMvc(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //}).AddXmlSerializerFormatters();

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
                options.AppendTrailingSlash = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                //app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
