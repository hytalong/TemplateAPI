using Template.Worker;

var builder = Host.CreateApplicationBuilder(args);

// Inicia��o do worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
