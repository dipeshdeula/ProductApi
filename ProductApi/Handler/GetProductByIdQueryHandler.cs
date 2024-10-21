using MediatR;
using ProductApi.Models;
using ProductApi.Services;
using ProductApiAsync.Queries;

namespace ProductApiAsync.Handler
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery,Product>
    {
        private readonly IProductService _productService;
        public GetProductByIdQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetProductByIdAsync(request.Id);
        }
    }
}
