using MediatR;
using ProductApi.Models;
using ProductApi.Services;
using ProductApiAsync.Queries;

namespace ProductApiAsync.Handler
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, IEnumerable<Product>>
    {
        public readonly IProductService _productService;
        public GetAllProductQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IEnumerable<Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetAllProductsAsync();
        }
    }
}
