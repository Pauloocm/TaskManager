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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var awsLoggerConfig = new AWSLoggerConfig("/aws/lambda/TaskManager-Api")
{
    Region = "us-east-1", // Change to your AWS region
    LibraryLogFileName = "/tmp/aws-logger-errors.txt" // Use /tmp/ instead of /var/task/
};

builder.Host.UseSerilog((_, loggerConfig) =>
{
    loggerConfig.WriteTo.Console().ReadFrom.Configuration(builder.Configuration)
    .WriteTo.AWSSeriLog(awsLoggerConfig);
});

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);



//DynamoDb

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
