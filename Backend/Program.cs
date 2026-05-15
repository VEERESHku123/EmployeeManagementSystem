using Auth.Fillters;
using Backend.Data.Context;
using Backend.Data.Repos;
using Backend.Mapper;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Repos 
builder.Services.AddDbContext<EmployeeDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("employeeManagementDbConStr")));
builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
builder.Services.AddScoped<DepartmentRepo>();
builder.Services.AddScoped<ManagerRepo>();


// Add services to the container.
builder.Services.AddControllers(options => options.Filters.Add<CommonExceptionFilter>());

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddAutoMapper(config => config.AddProfile<MappingProfile>());
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<ManagerService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI();
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
