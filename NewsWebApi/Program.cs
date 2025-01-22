using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using NewsWebApi.ConfigurationModels;
using NewsWebApi.Data;
using NewsWebApi.IRepository;
using NewsWebApi.Models;
using NewsWebApi.Repository;
using NewsWebApi.Services;
using System.Text;

namespace NewsWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policy => policy
                        .WithOrigins("https://localhost:7121", "https://localhost:7299") // Allow this origin
                        .AllowAnyMethod()                        // Allow any HTTP methods
                        .AllowAnyHeader()                        // Allow any headers
                        .AllowCredentials());                    // Allow credentials if needed
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });
            //builder.Services.AddSingleton<StartupTaskService>();

            builder.Services.AddHostedService<StartupTaskService>();

            builder.Services.AddScoped<INewsRepository, NewsRepository>();
            builder.Services.AddScoped<ITop15NewsRepository, Top15NewsRepository>();
            builder.Services.AddScoped<ISectionsRepository, SectionsRepository>();
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IRolesRepository, RolesRepository>();
            builder.Services.AddScoped<ITempPasswordRepository, TempPasswordRepository>();

            builder.Services.AddScoped<AppUnitWork>();

            builder.Services.AddScoped<NewsService>();
            builder.Services.AddScoped<SectionsService>();

            builder.Services.AddScoped<Top15NewsService>();
            builder.Services.AddScoped<AdminService>();
            builder.Services.AddScoped<AccountService>();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
            builder.Services.AddTransient<EmailService>();

            builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddTransient<JwtService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddHttpContextAccessor(); // Add this line
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowSpecificOrigin");

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                      Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = "/sfroot",
                EnableDefaultFiles = true
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}