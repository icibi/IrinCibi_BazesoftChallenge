/* Author: Irin Cibi
 * Date: 19/07/2016
 */

using IrinCibiBlazesoftChallenge.Configuration;
using IrinCibiBlazesoftChallenge.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

//Load MongoDB configuration from appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

//Register a single MongoDB client instance for the application
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

//Register MongoDB service used throughout the API
builder.Services.AddSingleton<MongoService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.MapControllers();

app.Run();