using HeatingGateway.API;
using HeatingGateway.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
  .AddAPI()
  .AddApplication(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
