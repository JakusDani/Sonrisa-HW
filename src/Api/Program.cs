using Api.Common.Data;
using Api.Common.Notifications;
using Api.Features.Admin.SimulateEvent;
using Api.Features.Alerts.CreateAlert;
using Api.Features.Alerts.DeleteAlert;
using Api.Features.Alerts.GetAlerts;
using Api.Features.Alerts.UpdateAlert;

const string ViteClientPolicy = "ViteClient";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options => options.AddPolicy(ViteClientPolicy, policy => policy
    .WithOrigins("http://localhost:5173", "https://localhost:5173")
    .AllowAnyHeader()
    .AllowAnyMethod()));

builder.Services.AddSingleton<FakeDatabase>();
builder.Services.AddSingleton<INotificationChannelFactory, NotificationChannelFactory>();
builder.Services.AddSingleton<INotificationQueue, InMemoryNotificationQueue>();
builder.Services.AddHostedService<NotificationDispatchConsumer>();

// Feature slices are registered here in later steps.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(ViteClientPolicy);

// Custom middleware is wired here in a later step.

GetAlertsEndpoint.Map(app);
CreateAlertEndpoint.Map(app);
DeleteAlertEndpoint.Map(app);
UpdateAlertEndpoint.Map(app);
SimulateEventEndpoint.Map(app);

// Logs feature endpoint mapping is wired here in a later step.

app.Run();
