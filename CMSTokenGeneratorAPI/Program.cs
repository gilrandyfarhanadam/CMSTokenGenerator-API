using Asp.Versioning;
using DatabaseConnection;
using Function;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(options => {
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new HeaderApiVersionReader("api-version");
});

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

// Versioning
var SetVersion = app.NewApiVersionSet().HasApiVersion(new ApiVersion(1,0)).ReportApiVersions().Build();

// Router
app.MapPost("/tokens/generate", tc.GenerateToken).WithOpenApi().WithApiVersionSet(SetVersion);
app.MapPost("/tokens/check", tc.CheckToken).WithOpenApi().WithApiVersionSet(SetVersion);

app.Run("http://localhost:5000");
