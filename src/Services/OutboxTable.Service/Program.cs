using Comment.Outbox.Table.Publisher.Sevice.Jobs;
using MassTransit;
using Outbox.Tables.Sevice.Jobs;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddQuartz(configure =>
{
    JobKey jobKey = new("CommentOutboxPublishJob");
    configure.AddJob<CommentOutboxPublishJob>(options => options.WithIdentity(jobKey));

    TriggerKey triggerKey = new("CommentOutboxPublishTrigger");
    configure.AddTrigger(options => options.ForJob(jobKey)
    .WithIdentity(triggerKey)
    .StartAt(DateTime.UtcNow)
    .WithSimpleSchedule(builder => builder
    .WithIntervalInSeconds(5)
    .RepeatForever()));
});
builder.Services.AddQuartz(configure =>
{
    JobKey jobKey = new("PostOutboxPublishJob");
    configure.AddJob<PostOutboxPublishJob>(options => options.WithIdentity(jobKey));

    TriggerKey triggerKey = new("PostOutboxPublishTrigger");
    configure.AddTrigger(options => options.ForJob(jobKey)
    .WithIdentity(triggerKey)
    .StartAt(DateTime.UtcNow)
    .WithSimpleSchedule(builder => builder
    .WithIntervalInSeconds(5)
    .RepeatForever()));
});

builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

builder.Services.AddMassTransit(configure =>
{
    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["rabbitmq"]);
    });
});

var host = builder.Build();
host.Run();
