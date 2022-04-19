using Microsoft.EntityFrameworkCore;
using RequestsService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("DefaultConnection"));

var app = builder.Build();

app.UseSwagger();

app.MapDefaultControllerRoute();

app.UseSwaggerUI();

app.Run();
