using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mo8tareb_RoomRentalWebApp.Api.JwtFeatures;
using Mo8tareb_RoomRentalWebApp.Api.Services.Email;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ImagesManagers;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ReservationManagers;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ReviewManagers;
using Mo8tareb_RoomRentalWebApp.BL.Managers.RoomManagers;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ServiceManagers;
using Mo8tareb_RoomRentalWebApp.BL.Managers.UserOwnersManagers;
using Mo8tareb_RoomRentalWebApp.DAL;
using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using Mo8tareb_RoomRentalWebApp.DAL.Seeding;
using Mo8tareb_UserOwnerRentalWebApp.BL.Managers.UserOwnerManagers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
//using Mo8tareb_RoomRentalWebApp.Api.Services.Email;

namespace Mo8tareb_RoomRentalWebApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            #region Cors
            string? corsPolicy = "AllowAll";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy, p => p.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
            });

            #endregion

            #region Adding DB Context with MSSQL

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                    //b =>
                    //{
                    //    b.UseNetTopologySuite();
                    //    b.MigrationsAssembly("Mo8tareb_RoomRentalWebApp.DAL");
                    //}
                    ));
            #endregion

            #region Adding Identity
            builder.Services.AddIdentity<AppUser, IdentityRole>(option =>
            {
                option.Password.RequireNonAlphanumeric = false;

                option.Lockout.AllowedForNewUsers = true;
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                option.Lockout.MaxFailedAccessAttempts = 3;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            #endregion

            #region Authentication
            IConfigurationSection? jwtSettings = builder.Configuration.GetSection("JwtSettings");
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

                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value))
                };
            });
            #endregion

            #region Authorization

            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AllowEngineers", policy => policy
            //        .RequireClaim(ClaimTypes.Role, "Engineering", "Management")
            //        .RequireClaim(ClaimTypes.NameIdentifier));

            //    options.AddPolicy("AllowManagers", policy => policy
            //        .RequireClaim(ClaimTypes.Role, "Management"));
            //});

            #endregion

            #region Add Google Authentication
            //builder.Services.AddAuthentication().AddGoogle("google", option =>
            //{
            //    var googleAuth = Configuration.GetSection("Authentication:Google");
            //    option.ClientId = googleAuth["ClientId"];
            //    option.ClientSecret = googleAuth["ClientSecret"];
            //    option.SignInScheme = IdentityConstants.ExternalScheme;
            //});
            #endregion

            #region Email Service
            var emailConfig = builder.Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();

            builder.Services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            builder.Services.AddSingleton(emailConfig);

            #endregion

            #region Injectable

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped<JwtHandler>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IReservationManager, ReservationManager>();
            builder.Services.AddScoped<IReviewManager, ReviewManager>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IimageManager, ImageManager>();
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Services.AddScoped<IUserOwnerManager, UserOwnerManager>();

            #endregion 

            #region AddingServiceManager
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            #endregion

            #region AddingRoomManager
            builder.Services.AddScoped<IRoomManager, RoomManager>();
            #endregion

            #region Default
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen( c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mo8tareb API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(corsPolicy);

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            #region Seeding Roles and Create Admin User
            // when app run create the roles [ADMIN, SELLER, CUSTOMER] and make user called "admin@gmail.com", password "1234Admin."
            // and assign this admin with the role ADMIN
            using (IServiceScope? scope = app.Services.CreateScope())
            {
                IServiceProvider? services = scope.ServiceProvider;
                try
                {
                    //  Initialize Roles and Users. This one will create a user with an Admin-role aswell as a separate User-role. See UserRoleInitializer.cs in Areas/Identity/Data
                    UserRoleInitializer.InitializeAsync(services).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured while attempting to seed the database");
                }
            }
            #endregion

            app.Run();
        }
    }
}