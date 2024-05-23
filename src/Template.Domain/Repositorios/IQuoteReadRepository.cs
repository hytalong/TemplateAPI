using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Domain.Entidades;

namespace Template.Domain.Repositorios
{
    public interface IQuoteReadRepository
    {
        Task<Dictionary<string, Quote>> Find(string coins);
    }
}