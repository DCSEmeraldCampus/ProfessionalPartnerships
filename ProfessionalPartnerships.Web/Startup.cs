﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfessionalPartnerships.Web.Data;
using ProfessionalPartnerships.Web.Models;
using ProfessionalPartnerships.Web.Services;
using ProfessionalPartnerships.Data.Models;
using ProfessionalPartnerships.Web.Services.Interface;

namespace ProfessionalPartnerships.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddDbContext<PartnershipsContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IInvitationService, InvitationService>();
            services.AddTransient<IUserAuthorizationService, UserAuthorizationService>();
                       
            var serviceProvider = services.BuildServiceProvider();
            ConfigureGoogleAuthentication(services, serviceProvider.GetRequiredService<PartnershipsContext>());

            await CreateRoles(serviceProvider);            
        }

        private async Task CreateRoles(IServiceProvider serviceProvider) {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            foreach(var roleName in new string[] {  "Administrator", "Student", "Professional"})
            {
                var existing = await roleManager.FindByNameAsync(roleName);

                if (existing == null)
                {
                    var role = new IdentityRole
                    {
                        Name = roleName
                    };
                    await roleManager.CreateAsync(role);
                }
            }        

        }

        private void ConfigureGoogleAuthentication(IServiceCollection services, PartnershipsContext db)
        {
            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = db.ConfigurationValues.FirstOrDefault(x => x.Key == ProfessionalPartnerships.Web.Constants.ConfigurationValueKeys.SystemGoogleClientId).Value; 
                googleOptions.ClientSecret = db.ConfigurationValues.FirstOrDefault(x => x.Key == ProfessionalPartnerships.Web.Constants.ConfigurationValueKeys.SystemClientSecret).Value;
            });
            services.AddMvc().AddSessionStateTempDataProvider();

            services.AddSession(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }        
    }


}
