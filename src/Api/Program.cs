using System.Text.Json.Serialization;
using Api.Common.Data;
using Api.Common.Notifications;
using Api.Features.Admin.SimulateEvent;
using Api.Features.Alerts.CreateAlert;
using Api.Features.Alerts.DeleteAlert;
using Api.Features.Alerts.GetAlerts;
using Api.Features.Alerts.UpdateAlert;
using Api.Features.Logs.GetLogs;
using Api.Middleware;

const string ViteClientPolicy = "ViteClient";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddCors(options => options.AddPolicy(ViteClientPolicy, policy => policy
    .WithOrigins("http://localhost:5173", "https://localhost:5173")
    .AllowAnyHeader()
    .AllowAnyMethod()));

// Notification pipeline: FakeDatabase (store), channel factory (Strategy), in-memory queue, and dispatch consumer (BackgroundService).
builder.Services.AddSingleton<FakeDatabase>();
builder.Services.AddSingleton<INotificationChannelFactory, NotificationChannelFactory>();
builder.Services.AddSingleton<INotificationQueue, InMemoryNotificationQueue>();
builder.Services.AddHostedService<NotificationDispatchConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(ViteClientPolicy);

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

// Feature slice endpoint mappings (Vertical Slice Architecture).
GetAlertsEndpoint.Map(app);
CreateAlertEndpoint.Map(app);
DeleteAlertEndpoint.Map(app);
UpdateAlertEndpoint.Map(app);
SimulateEventEndpoint.Map(app);
GetLogsEndpoint.Map(app);

app.Run();
