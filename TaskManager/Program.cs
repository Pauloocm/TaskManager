using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AWS.Logger;
using AWS.Logger.SeriLog;
using Serilog;
using TaskManager.Domain.Tasks;
using TaskManager.Platform.Application;
using TaskManager.Platform.Infrastructure.Repositorie;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var awsLoggerConfig = new AWSLoggerConfig(builder.Configuration["Serilog:LogGroup"])
{
    Region = "sa-east-1",
    LibraryLogErrors = true,
    LibraryLogFileName = "/tmp/aws-logger-errors.txt"
};

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
        .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Error)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
        .WriteTo.Console()
        .WriteTo.AWSSeriLog(awsLoggerConfig, iFormatProvider: null,
            textFormatter: new Serilog.Formatting.Json.JsonFormatter());
});

builder.Services.AddSingleton(awsLoggerConfig);

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddTransient<ITaskAppService, TaskAppService>();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
