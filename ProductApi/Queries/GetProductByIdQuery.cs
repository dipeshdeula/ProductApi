using MediatR;
using ProductApi.Models;

namespace ProductApiAsync.Queries
{
    public class GetProductByIdQuery:IRequest<Product>
    {
        public int Id { get; set; }
    }
}
