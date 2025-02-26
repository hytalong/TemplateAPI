using Template.Worker;

var builder = Host.CreateApplicationBuilder(args);

// Iniciação do worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
