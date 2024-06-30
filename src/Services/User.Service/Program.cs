using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Post.Service.Services;
using Post.Service.Services.Abstractions;
using Shared;
using Shared.Events;
using User.Service.Consumers;
using User.Service.Models.Contexts;
using User.Service.Models.Entities;
using User.Service.Services;
using User.Service.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMediatR(configure =>
{
    configure.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddDbContext<AppUserDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppUserDbContext>();

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<CommentSavedToPostEventConsumer>();
    configure.AddConsumer<CommentDeletedFromPostEventConsumer>();
    configure.AddConsumer<PostCreatedEventConsumer>();
    configure.AddConsumer<PostDeletedEventConsumer>();
    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["rabbitmq"]);
        configurator.ReceiveEndpoint(RabbitMqSettings.User_CommentSavedToPostEventQueue, e => e.ConfigureConsumer<CommentSavedToPostEventConsumer>(context));
        configurator.ReceiveEndpoint(RabbitMqSettings.User_CommentDeletedEventQueue, e => e.ConfigureConsumer<CommentDeletedFromPostEventConsumer>(context));
        configurator.ReceiveEndpoint(RabbitMqSettings.User_PostDeletedEventQueue, e => e.ConfigureConsumer<PostCreatedEventConsumer>(context));
        configurator.ReceiveEndpoint(RabbitMqSettings.User_PostCreatedEventQueue, e => e.ConfigureConsumer<PostDeletedEventConsumer>(context));
    });
});


builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped(typeof(ICommentInboxService<,>), typeof(CommentInboxService<,>));

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
