using Serilog;
using FundBeacon.Application.Interfaces;
using FundBeacon.Application.Seed;
using FundBeacon.Application.Services;
using FundBeacon.Data;
using FundBeacon.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
builder.Services.AddDbContext<FundBeaconDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<FundBeaconDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IScholarshipService, ScholarshipService>();
builder.Services.AddHostedService<UserCleanupService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        googleOptions.CallbackPath = "/signin-google"; // default
    });

builder.Services.AddAuthorization();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FundBeacon API", Version = "v1" });
 
//    var jwtSecurityScheme = new OpenApiSecurityScheme
//    {
//        Scheme = "bearer",
//        BearerFormat = "JWT",
//        Name = "JWT Authentication",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.Http,
//        Description = "Enter JWT Bearer token ONLY",
 
//        Reference = new OpenApiReference
//        {
//            Id = JwtBearerDefaults.AuthenticationScheme,
//            Type = ReferenceType.SecurityScheme
//        }
//    };
 
//    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        { jwtSecurityScheme, Array.Empty<string>() }
//    });
//});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedRolesAsync(roleManager);
}
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
