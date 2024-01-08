using HeatingGateway.API;
using HeatingGateway.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
  .AddAPI()
  .AddApplication(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => {
  options.AddPolicy(name: "Allow-All", policy => {
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    policy.AllowAnyOrigin();
  });
});

var app = builder.Build();

app.UseCors("Allow-All");
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();
