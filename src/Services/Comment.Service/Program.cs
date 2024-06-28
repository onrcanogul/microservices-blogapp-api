using Comment.Service.Services;
using Comment.Service.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddMediatR(configure =>
{
    configure.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddControllers();
builder.Services.AddSingleton<IMongoDbService,MongoDbService>();
builder.Services.AddScoped<ICommentService, CommentService>();

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
