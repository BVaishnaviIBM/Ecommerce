using Microsoft.EntityFrameworkCore;
using PaymentService.DataAccess.Data;
using PaymentService.Business.Services;
using PaymentService.DataAccess.Repositories;
using PaymentBusinessService = PaymentService.Business.Services.PaymentService;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<PaymentServiceDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "PaymentServiceConnection")));

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentBusinessService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
