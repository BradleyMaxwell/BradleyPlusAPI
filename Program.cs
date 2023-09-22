using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using api.Services;
using api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dependancy injections
builder.Services.AddScoped<IAccountService, AccountService>(); // tells the api to use the account service as the implementation of the interface

// configuring DynamoDB client and context for the services to use
BasicAWSCredentials credentials = new BasicAWSCredentials(
    Environment.GetEnvironmentVariable("ACCESS_KEY"),
    Environment.GetEnvironmentVariable("SECRET_KEY"));
AmazonDynamoDBConfig config = new AmazonDynamoDBConfig() { RegionEndpoint = Amazon.RegionEndpoint.EUWest2 };
AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, config);
builder.Services.AddSingleton<IAmazonDynamoDB>(client); // dependancy injection when api needs the client to make the context 
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>(); // how the context interface should be implemented

// accept requests from the frontend domain
string PolicyName = "AllowFrontendRequests";
builder.Services.AddCors(options => // creating new policy that specifies which domains are allowed to make requests
{
    options.AddPolicy(name: PolicyName, policy => { policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader(); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(PolicyName); // putting the policy created previously into affect, had to go before UseAuthorization because of middleware order

app.UseAuthorization();

app.MapControllers();

app.Run();

