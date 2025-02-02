using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Tasks;
using TaskManager.Platform.Application;
using TaskManager.Platform.Infrastructure.Database;
using TaskManager.Platform.Infrastructure.Repositorie;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName.ToLower();

//builder.Services.AddAWSService<IAmazonSimpleSystemsManagement>();

//var client = new AmazonSimpleSystemsManagementClient();

//var response = await client.GetParameterAsync(new GetParameterRequest
//{
//    Name = $"/{environment}/connectionString"
//});

//var parameterValue = response.Parameter.Value;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

//DynamoDb

builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddScoped<ITaskRepository, DynamoDBTaskRepository>();

//builder.Services.AddTransient<ITaskRepository, TaskRepository>();
builder.Services.AddTransient<ITaskAppService, TaskAppService>();

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(""));

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
