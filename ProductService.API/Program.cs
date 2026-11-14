using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ProductService.Business.Services;
using ProductService.DataAccess.Data;
using ProductService.DataAccess.Repositories;
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<ProductServiceDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ProductServiceConnection")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

builder.Services.AddScoped<ProductService.Business.Services.ProductService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!))
        };
    });


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
