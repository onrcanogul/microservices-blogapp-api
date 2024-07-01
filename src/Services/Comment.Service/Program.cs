using Comment.Service.Consumers;
using Comment.Service.Models.Contexts;
using Comment.Service.Services;
using Comment.Service.Services.Abstractions;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddMediatR(configure =>
{
    configure.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddControllers();
builder.Services.AddSingleton<IMongoDbService,MongoDbService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddDbContext<CommentOutboxDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("sqlserver")));

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<PostNotFoundEventConsumer>();
    configure.AddConsumer<UserNotFoundEventConsumer>();
    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host("rabbitmq");
        configurator.ReceiveEndpoint(RabbitMqSettings.Comment_PostNotFoundEventQueue, e => e.ConfigureConsumer<PostNotFoundEventConsumer>(context));
        configurator.ReceiveEndpoint(RabbitMqSettings.Comment_UserNotFoundEventQueue, e => e.ConfigureConsumer<UserNotFoundEventConsumer>(context));
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHealthChecks()
    .AddMongoDb(builder.Configuration.GetConnectionString("mongodb")!);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseAuthorization();

app.MapControllers();

app.Run();
