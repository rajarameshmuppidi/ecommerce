using EcommercePlatform.Data;
using EcommercePlatform.Repositories;
using EcommercePlatform.Services;
using EcommercePlatform.Utilities;
using EcommercePlatform.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.ClearProviders(); // Optional: Clears default providers
builder.Logging.AddConsole();     // Adds console logging
builder.Logging.AddDebug();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Ecommerce by Raja", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            },
            Scheme = "Oauth2",
            Name = JwtBearerDefaults.AuthenticationScheme,
            In = ParameterLocation.Header
        },
        new List<string>()
        }
    });

});

// ------------------ REPOSITORY REGISTRY -----------------
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IOrdersRepository, OrdersEfRepository>();
builder.Services.AddScoped<IProductsRepository, ProductEfRepository>();
builder.Services.AddScoped<ICartRepository, CartEfRepository>();
builder.Services.AddScoped<IReviewsRepository, ReviewsRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// ------------------ SERVICE REGISTRY --------------------
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IReviewsService, ReviewsService>();
builder.Services.AddTransient<ITransientService, TransientService>();
builder.Services.AddScoped<IScopedService, ScopedService>();
builder.Services.AddSingleton<ISingleTon, SingleTon>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity with AppUser
builder.Services.AddIdentityCore<AppUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<AppUser>>("Random")
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add UserManager<AppUser> service
builder.Services.AddScoped<UserManager<AppUser>>();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
});



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for the hub
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/order")))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });



//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = "yourIssuer",
//        ValidAudience = "yourAudience",
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKey"))
//    };

//    //  Enable JWT auth for SignalR
//    new JwtBearerEvents
//    {
//        OnMessageReceived = context =>
//        {
//            var accessToken = context.Request.Query["access_token"];

//            // If the request is for the hub
//            var path = context.HttpContext.Request.Path;
//            if (!string.IsNullOrEmpty(accessToken) &&
//                (path.StartsWithSegments("/sellerHub")))
//            {
//                context.Token = accessToken;
//            }

//            return Task.CompletedTask;
//        }
//    };
//});


builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<OrderHub>("/order");

app.Run();
