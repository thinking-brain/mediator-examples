using MeaditR.Example.Orders.PlaceOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mediator.Core.Repositories;
using Mediator.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Register repositories and services
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();
builder.Services.AddSingleton<IProductCatalog, InMemoryProductCatalog>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<ILoyaltyService, LoyaltyService>();
builder.Services.AddSingleton<IEventBus, EventBus>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/place-order", async (ISender sender, [FromBody] PlaceOrderCommand command) =>
{
    try
    {
        var orderId = await sender.Send(command);
        return Results.Ok(new { OrderId = orderId, Message = "Order placed successfully" });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { Error = ex.Message });
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred: {ex.Message}");
    }
})
.WithName("PlaceOrder");

app.Run();
