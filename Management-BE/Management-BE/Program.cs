using Management_BE.Data;
using Management_BE.Data.AuthenticationData;
using Management_BE.Interfaces.Authentication;
using Management_BE.Interfaces.Documents;
using Management_BE.Interfaces.Hasher;
using Management_BE.Repositories.Authentication;
using Management_BE.Repositories.Documents;
using Management_BE.Services;
using Microsoft.EntityFrameworkCore;

// Variable to enable Cors Origins
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* Section Data Context DB connection */
builder.Services.AddDbContext<ApplicationDataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

/* Scope Repository */
// Autenthication Repository
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
// Password Repository
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
// Document Repository
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();

// Enable Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        builder =>
        {
            //builder.AllowAnyOrigin();

            // Enable cors for front-end (Angular)
            builder.WithOrigins("https://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use cors
app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
