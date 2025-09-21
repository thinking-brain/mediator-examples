using Mediator.Core.Repositories;
using Mediator.Core.Services;
using MassTransit.Example.Orders.PlaceOrder;
using MassTransit.Example.Orders.OrderCreated;
using MassTransit.Example.Catalogue.GetProducts;
using MassTransit.Example.Customer.GetCustomerExist;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register repositories and services
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();
builder.Services.AddSingleton<IProductCatalog, InMemoryProductCatalog>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<ILoyaltyService, LoyaltyService>();
builder.Services.AddSingleton<IEventBus, EventBus>();

// Configure MassTransit
builder.Services.AddMassTransit(x =>
{
    // Register consumers
    x.AddConsumer<PlaceOrderConsumer>();
    x.AddConsumer<SendConfirmationEmailConsumer>();
    x.AddConsumer<AddLoyaltyPointsConsumer>();
    x.AddConsumer<PublishIntegrationEventConsumer>();
    x.AddConsumer<AuditLogConsumer>();
    x.AddConsumer<GetProductsConsumer>();
    x.AddConsumer<GetCustomerExistConsumer>();

    // Configure In-Memory transport for demo
    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/place-order", async (IRequestClient<PlaceOrderCommand> client, [FromBody] PlaceOrderCommand command) =>
{
    try
    {
        var response = await client.GetResponse<PlaceOrderResult>(command);
        var result = response.Message;

        if (result.Success)
        {
            return Results.Ok(new { OrderId = result.OrderId, Message = "Order placed successfully" });
        }
        else
        {
            return Results.BadRequest(new { Error = result.ErrorMessage });
        }
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred: {ex.Message}");
    }
})
.WithName("PlaceOrder");

app.Run();
