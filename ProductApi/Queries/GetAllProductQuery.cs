using MediatR;
using ProductApi.Models;

namespace ProductApiAsync.Queries
{
    public class GetAllProductQuery : IRequest<IEnumerable<Product>> { }
     
}
