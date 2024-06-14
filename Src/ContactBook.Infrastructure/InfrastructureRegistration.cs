using ContactBook.Core.Entities;
using ContactBook.Core.Helper;
using ContactBook.Core.Interfaces;
using ContactBook.Core.Sharing;
using ContactBook.Infrastructure.Data;
using ContactBook.Infrastructure.Data.Config;
using ContactBook.Infrastructure.Repository;
using ContactBook.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IInviteUserRepository, InviteUserRepository>();
            services.AddScoped<ITokenServices, TokenServices>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddDbContext<ContactBookDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefulteConnection"));
            });

            services.AddHostedService<StatusUserUpdateService>();

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ContactBookDbContext>()
                .AddDefaultTokenProviders();
            services.AddMemoryCache();
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"])),
                            ValidIssuer = configuration["Token:Issuer"],
                            ValidateIssuer = true,
                            ValidateAudience = false,
                        };
                    });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminOrUserRole", policy => policy.RequireRole(UserRoles.Admin, UserRoles.User));
            });

            return services;
        }

        public static async void InfrastructureConfigMiddleware(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedingData.SeedUserAsync(userManager, roleManager);
            }
        }
    }
}
