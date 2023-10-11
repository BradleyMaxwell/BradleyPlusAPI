using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using api.Services;
using api.Services.Interfaces;
using api.Utility;
using api.Utility.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
JwtTokenManager tokenManager = new JwtTokenManager();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Access", options => // adding validation parameters for access tokens
    {
        options.TokenValidationParameters = tokenManager.GetTokenValidationParameters(TokenType.Access);
    })
    .AddJwtBearer("Refresh", options => // adding validation parameters for refresh tokens
    {
        options.TokenValidationParameters = tokenManager.GetTokenValidationParameters(TokenType.Refresh);
    });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dependancy injections
builder.Services.AddScoped<IAccountService, AccountService>(); // tells the api to use the account service as the implementation of the interface
builder.Services.AddScoped<IJwtTokenManager, JwtTokenManager>();

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
    options.AddPolicy(name: PolicyName, policy => { policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials(); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// the following is the pipeline a request goes through
app.UseExceptionHandler("/error"); // redirect user to this endpoint if any exceptions are raised to protect sensitive information
app.UseHttpsRedirection();
app.UseCors(PolicyName); // putting the policy created previously into affect, had to go before UseAuthorization because of middleware order
//app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

