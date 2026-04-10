using News.BusinessLogic;
using News.BusinessLogic.Interfaces;
using News.BusinessLogic.Token;
using News.Infrastructure;
using System.Reflection;
using News.BusinessLogic.Common.Mappings;
using News.Enums;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
builder.Services.AddScoped<Token>();
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
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
