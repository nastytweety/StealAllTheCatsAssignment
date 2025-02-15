using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StealAllTheCatsAssignment.Application.IRepository;
using StealAllTheCatsAssignment.Application.IService;
using StealAllTheCatsAssignment.Application.Mapperly;
using StealAllTheCatsAssignment.Data;
using StealAllTheCatsAssignment.Infrastructure.Repository;
using StealAllTheCatsAssignment.Application.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));

builder.Services.AddScoped<IAppService, AppService>();

builder.Services.AddScoped<IAppRepository, AppRepository>();

builder.Services.AddScoped<IMapper, Mapper>();

builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { 
                                                                        Title = "StealAllTheCats", 
                                                                        Version = "v1", 
                                                                        Description = "A simple api that steals and presents cats from thecatapi.com",
                                                                        Contact = new OpenApiContact
                                                                        {
                                                                            Name = "Nikolaos Diakos",
                                                                            Email = "nikolaosdiakos@gmail.com",
                                                                        }
                                                                       });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
         c.SwaggerEndpoint("/swagger/v1/swagger.json", "StealAllTheCats V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
