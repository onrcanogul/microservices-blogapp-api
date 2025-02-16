using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Post.Service.Consumers;
using Post.Service.Models.Contexts;
using Post.Service.Services;
using Post.Service.Services.Abstractions;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<CommentCreatedEventConsumer>();
    configure.AddConsumer<CommentDeletedEventConsumer>();
    configure.AddConsumer<UserNotFoundEventConsumer>();
    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["rabbitmq"]);
        configurator.ReceiveEndpoint(RabbitMqSettings.Post_CommentCreatedEventQueue, e => e.ConfigureConsumer<CommentCreatedEventConsumer>(context));
        configurator.ReceiveEndpoint(RabbitMqSettings.Post_CommentDeletedEventQueue, e => e.ConfigureConsumer<CommentDeletedEventConsumer>(context));
        configurator.ReceiveEndpoint(RabbitMqSettings.Post_UserNotFoundEventQueue, e => e.ConfigureConsumer<UserNotFoundEventConsumer>(context));
    });
});

builder.Services.AddScoped(typeof(IInboxService<>), typeof(InboxService<>));
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddDbContext<PostDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));


builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("SQLServer")!);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

app.Run();
