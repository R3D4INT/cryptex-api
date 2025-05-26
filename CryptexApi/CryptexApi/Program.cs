using System.Text;
using CryptexApi.BackGroundJob;
using CryptexApi.Context;
using CryptexApi.Identity.Services;
using CryptexApi.Models;
using CryptexApi.Models.Identity;
using CryptexApi.Repos;
using CryptexApi.Repos.Interfaces;
using CryptexApi.Services;
using CryptexApi.Services.Interfaces;
using CryptexApi.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;

namespace CryptexApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; 
                    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; 
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                })
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration["Google:ClientId"];
                    options.ClientSecret = builder.Configuration["Google:ClientSecret"];
                    options.CallbackPath = "/signin-google";
                    options.SaveTokens = true;
                });

            SD.Issuer = builder.Configuration["Jwt:Issuer"];
            SD.JWTKey = builder.Configuration["Jwt:Key"];
            SD.Audience = builder.Configuration["Audience"];
            builder.Services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = new JobKey(nameof(UpdateCoinPrices));

                q.AddJob<UpdateCoinPrices>(jobKey, opts => opts
                    .WithDescription("Update coin prices")
                    .StoreDurably());

                q.AddTrigger(t => t
                    .ForJob(jobKey)
                    .WithSimpleSchedule(x => x.WithIntervalInMinutes(15).RepeatForever()));
            });

            builder.Services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CryptexDB")));
            // Add services to the container.
            builder.Services.AddScoped<IBinanceRequestService, BinanceRequestService>();
            builder.Services.AddScoped<IAchievmentRepository, AchievmentRepository>();
            builder.Services.AddScoped<IAdminActionRepository, AdminActionRepository>();
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<ICoinRepository, CoinRepository>();
            builder.Services.AddScoped<IFuethersDealRepository, FuethersDealRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IRewardRepository, RewardRepository>();
            builder.Services.AddScoped<ISeedPhraseRepository, SeedPhraseRepository>();
            builder.Services.AddScoped<ISupportRepository, SupportRepository>();
            builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IWalletForMarketRepository, WalletForMarketRepository>();
            builder.Services.AddScoped<IWalletRepository, WalletRepository>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<ICoinService, CoinService>();
            builder.Services.AddScoped<IFuethersDealService, FuethersDealService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<ISeedPhraseService, SeedPhraseService>();
            builder.Services.AddScoped<ISupportService, SupportService>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<IWalletService, WalletService>();
            builder.Services.AddScoped<IMarketWalletService, MarketWalletService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITokenGeneratingService, TokenGeneratingService>(); builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection("EmailSettings"));

            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cryptex API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = JobKey.Create(nameof(UpdateCoinPrices));

                options
                    .AddJob<UpdateCoinPrices>(jobKey)
                    .AddTrigger(trigger => trigger.ForJob(jobKey)
                        .WithSimpleSchedule(schedule => schedule
                            .WithIntervalInMinutes(1).RepeatForever()));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
