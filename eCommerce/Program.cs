using eCommerce.Core.Interfaces;
using eCommerce.Data;
using eCommerce.Data.Auth;
using eCommerce.Services.AuthService;
using eCommerce.Services.CategoriesService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace eCommerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // ! Swagger with JWT auth
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });

            }
            );

            //services cors
            builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            // ! Get secret key from appsettings.json
            var secretKey = builder.Configuration.GetValue<string>("SecretKey");
            var configuration = builder.Configuration;



            // ! For Identity // SHOULD COME FIRST
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            // Adding Authentication
            builder.Services.AddAuthentication(options =>
            {
                // ! THIS FUCKING ERROR FUCKING SHIT
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                // ! this gives back errors!
                options.IncludeErrorDetails = true;
                //options.SaveToken = true;
                //options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    //ValidAudience = configuration["aud"],
                    //ValidIssuer = configuration["iss"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtConfiguration:TokenSecret"]))
                };
            });


            //transient: light and not multi instances
            //scoped: not light and scoped to http request

            // ! Add Authorization
            builder.Services.AddAuthorization();

            // ! Dependency Injection
            builder.Services.AddTransient<IAuth, AuthService>();
            //builder.Services.AddScoped<ICategories, CategoriesService>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //Auto mapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            // ! Data Base
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            //app cors
            app.UseCors("corsapp");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            // ! double auth
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.MapControllers();

            #region Apply migrations, create DB on app start & seed data
            using var scope = app.Services.CreateScope();
            var servies = scope.ServiceProvider;
            var context = servies.GetRequiredService<AppDbContext>();
            var logger = servies.GetRequiredService<ILogger<Program>>();

            try
            {
                await context.Database.MigrateAsync();
                await ContextSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error ocurred during migrations");
            }
            #endregion



            app.Run();
        }
    }
}