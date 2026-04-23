using News.BusinessLogic;
using News.BusinessLogic.Interfaces;
using News.Infrastructure;
using System.Reflection;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using News.BusinessLogic.Common.Mappings;
using News.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;  

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)),
            ValidateIssuer = true,
            ValidIssuer = "ETN-News",
            ValidateAudience = true,
            ValidAudience = "ETN-Users",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["jwt"];
                context.Token = token;
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(INewsDbContext).Assembly));
});
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
var provider = builder.Configuration["Embedding:Provider"] == "Gemini"
    ? EmbeddingProvider.Gemini
    : EmbeddingProvider.SentenceTransformers;
builder.Services.AddBusinessLogic(
    provider,
    sentenceTransformersUrl: builder.Configuration["Embedding:SentenceTransformersUrl"] ?? "",
    geminiApiKey: builder.Configuration["Embedding:GeminiApiKey"] ?? ""
);

builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins( "http://localhost:3000")
            .AllowCredentials() 
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        serviceProvider.GetRequiredService<NewsDbContext>();
    }
    catch
    {
        // ignored
    }
}

app.Run();
