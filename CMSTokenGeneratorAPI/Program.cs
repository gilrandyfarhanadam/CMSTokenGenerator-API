using DatabaseConnection;
using Function;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

TokenCallback tc = new();

Database database= Database.GetInstance();
database.Connect();

// Router
app.MapPost("/tokens/generate", tc.GenerateToken).WithOpenApi();
app.MapPost("/tokens/check", tc.CheckToken).WithOpenApi();

app.Run();
