using HeatingService.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcClient<HeatPumpSvc.HeatPumpSvcClient>(o => {
    o.Address = new Uri("http://host.docker.internal:50051");
});
builder.Services.AddScoped<IHeatPumpService, HeatPumpService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => {
    options.AddPolicy("AllowSpecificOrigin", corsPolicyBuilder => {
         corsPolicyBuilder
            .WithOrigins("http://localhost:8000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();