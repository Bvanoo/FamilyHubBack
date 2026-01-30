// ... tes usings existants

using FamHubBack.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 1. AJOUTE SignalR ici
builder.Services.AddSignalR();

// 2. AJOUTE le CORS pour Angular (indispensable)
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngular", policy => {
        policy.WithOrigins("http://localhost:4200") // Ton port Angular
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Obligatoire pour SignalR
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowAngular");


app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// 4. MAP ton Hub (vérifie le namespace de ChatHub)
app.MapHub<ChatHub>("/chatHub");

app.Run();