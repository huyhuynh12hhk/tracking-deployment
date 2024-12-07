using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PostTrackingAPI.Services;
using ProductTrackingAPI.Data;
using ProductTrackingAPI.Services;
using ProductTrackingAPI.Utils;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using ProductTrackingAPI;

//https://stackoverflow.com/questions/59199593/net-core-3-0-possible-object-cycle-was-detected-which-is-not-supported

var builder = WebApplication.CreateBuilder(args);
var applicationAssembly = typeof(Program).Assembly;

// Add services to the container.


/*JsonSerializerOptions options = new()
{
    ReferenceHandler = ReferenceHandler.IgnoreCycles,
    WriteIndented = true
};*/
builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        
    }
)
 //.AddNewtonsoftJson(options =>
// options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
//)
    ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TokenWriter>();
builder.Services.AddAutoMapper(applicationAssembly);
//builder.Services.AddDbContext<TrackingManagementContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDbContext<TrackingManagementContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
}, ServiceLifetime.Transient);

builder.Services.AddCors(options =>
{
    options.AddPolicy("default", builder => {
        //builder.WithOrigins("http://localhost:8001").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        //builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
        //builder.SetIsOriginAllowed(origin => true);
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = false,
        //ValidAudience = audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateLifetime = true, // Ensures token hasn't expired
        ClockSkew = TimeSpan.Zero, // Adjust if needed for token clock tolerance

    };
});

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<SocialService>();
builder.Services.AddScoped<SearchService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.EnsureDataInit<TrackingManagementContext>().Wait();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

//app.UseHttpsRedirection();
//app.UseCors("default");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
