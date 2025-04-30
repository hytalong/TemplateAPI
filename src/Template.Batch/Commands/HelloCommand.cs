using Serilog;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Template.Batch.Commands
{
    [Command(Description = "Hello World exemple")]
    public class HelloCommand : ICommand
    {
        private ILogger Logger { get; }
        private IMediator Mediator { get; }
        [Option("-m|--message", Description = "Exemplo de parametro")]
        public string Message { get; set; }

        public HelloCommand(ILogger logger, IMediator mediator)
        {
            Logger = logger;
            Mediator = mediator;
        }

        public void OnExecute()
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(Message);
        }

        

        // public Task<List<Lote<T>>> ExecuteQuebraLote<T>(IList<T> dados)
        // {
        //     return null;
        // }
    }
}
