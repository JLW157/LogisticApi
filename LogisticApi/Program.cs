using LogisticApi.Models.DbOptions;
using LogisticApi.Services;
using LogisticApi.Services.LogisticRepository;
using LogisticApi.Services.LogisticService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<LogisticDatabaseOptions>(
    builder.Configuration.GetSection("LogisticDatabase"));

builder.Services.AddSingleton<ILogisticRepository, LogisticRepository>();
builder.Services.AddScoped<ILogisticService, LogisticService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
