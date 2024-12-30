using ECommerce.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureDb(builder);
builder.Services.ConfigureIdentity();
builder.Services.AddControllers();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureSwagger();
builder.Services.ConfigureJwtAuth(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
