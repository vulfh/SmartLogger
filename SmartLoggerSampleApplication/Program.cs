using SmartLogger.Core;
using SmartLoggerSampleApplication.Exceptions;
using SmartLoggerSampleApplication.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ILogAggregator, SmartLogger.Core.SmartLoggerHub>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

 app.MapGet("/happyflow", (ILogAggregator logAggregator) => {
     logAggregator.LogDebug("Start happy flow");
     logAggregator.LogInformation("HappyFlow finished");
     return "Happy flow !!!";
});

app.MapGet("/error404flow", (ILogAggregator logAggregator) => {
    logAggregator.LogDebug("Start not found flow");
    logAggregator.LogInformation("HappyFlow finished");
    throw new KeyNotFoundException("An error occured");
});

app.MapGet("/error400flow", (ILogAggregator logAggregator) => {
    logAggregator.LogDebug("Start bad request flow");
    logAggregator.LogInformation("HappyFlow finished");
    throw new BadRequestException();
});

app.MapGet("/exceptionflow", (ILogAggregator logAggregator) => {
    logAggregator.LogDebug("Start exception flow");
    logAggregator.LogDebug("");
    logAggregator.LogInformation("exception flow finished");
    throw new Exception("An error occured");
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}