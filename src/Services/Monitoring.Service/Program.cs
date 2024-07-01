var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecksUI(settings =>
{
    settings.AddHealthCheckEndpoint("Comment.Service", "https://localhost:7229/health");
    settings.AddHealthCheckEndpoint("User.Service", "https://localhost:7109/health");
    settings.AddHealthCheckEndpoint("Post.Service", "https://localhost:7299/health");

}).AddSqlServerStorage(builder.Configuration.GetConnectionString("SQLServer")!);

var app = builder.Build();

app.UseHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

app.UseHttpsRedirection();
app.Run();
