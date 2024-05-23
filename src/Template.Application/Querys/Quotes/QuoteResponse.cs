using System.Collections.Generic;
using Template.Domain.Entidades;

namespace Template.Application.Querys.Quotes
{
    public class QuoteResponse
    {
        public Dictionary<string, Quote> Quotes { get; set;}

        public ApiMetaDataResponse metadata { get; set; }
    }

   
}