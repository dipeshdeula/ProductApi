using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductApi.Data;
using ProductApi.Repositories;
using ProductApi.Services;
using ProductApi.Utilities;
using ProductApiAsync.Mappers;
using System.Reflection;
using System.Text;

namespace ProductApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Register MediatR
            //builder.Services.AddMediatR(configuration =>
            //{
            //    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
            //});

            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

            //Register the UserRepository with DI container
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            


            //Register the ProductDbContext with DI container
            builder.Services.AddDbContext<ProductDbContext>(
                opt => opt.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            //Register the IproductRepository with DI container
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            //Register the IProductService with DI container
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<JwtTokenHelper>();

            //Register the Mapper
            builder.Services.AddScoped<IProductMapper,ProductMapper>();

            //Add CORS services and configure policies
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("AllowAllOrigins",
                    policy =>
                    {
                        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });

            //configure JWT authentication middleware
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
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

            //Custom Authorization policy
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                opt.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });

          

            //Add Swagger services
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Product API",
                    Version = "v1",
                    Description = "A simple example ASP.NET Core Web API",
                });

            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();

            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            //Use CORS with the defined policy
            app.UseCors("AllowAllOrigins");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
