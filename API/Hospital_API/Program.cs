using Hospital_API.Data;
using Hospital_API.Data.Models;
using Hospital_API.Interfaces;
using Hospital_API.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hospital_API.Extensions;



var builder = WebApplication.CreateBuilder(args);

#region Logger
ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = loggerFactory.CreateLogger<Program>();
#endregion

#region Database
builder.Services.AddMsSQLDatabase();
//builder.Services.AddDbContext<AppDBContext>(opt =>
//    opt.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB")));
#endregion


builder.Services.AddTransient<ExceptionHandler>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IRepository<Patient>, PatientRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Patient Management",
        Description = "ASP.NET Core 6 Web API for managing patient data, supporting CRUD operations and search by birthday",        
        Contact = new OpenApiContact
        {
            Name = "Alexander Medved",
            Url = new Uri("https://github.com/Grizzly-Alex")
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});



var app = builder.Build();


#region Adding Migration
using (var scope = app.Services.CreateScope()) 
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    try
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        logger.LogError(message: ex.Message);
    }
}
#endregion


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandler>();

app.MapControllers();

app.Run();
