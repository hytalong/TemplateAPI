using MediatR;

namespace Template.Application.Querys.Quotes
{
    public class QuoteRequest : IRequest<QuoteResponse>
    {
        public string coins {get; set;}   
    }
}