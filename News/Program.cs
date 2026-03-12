using News.BusinessLogic;
using News.BusinessLogic.Interfaces;
using News.BusinessLogic.Token;
using News.Infrastructure;
using System.Reflection;
using News.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(
    Assembly.GetExecutingAssembly(),
    typeof(INewsDbContext).Assembly
);
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
builder.Services.AddScoped<Token>();
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
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
