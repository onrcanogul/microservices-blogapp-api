using MassTransit;
using Microsoft.EntityFrameworkCore;
using Post.Service.Consumers;
using Post.Service.Models.Contexts;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<CommentCreatedEventConsumer>();
    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["rabbitmq"]);
        configurator.ReceiveEndpoint(RabbitMqSettings.Post_CommentCreatedEventQueue, e => e.ConfigureConsumer<CommentCreatedEventConsumer>(context));
    });
});

builder.Services.AddDbContext<PostDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));

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

app.MapControllers();

app.Run();
