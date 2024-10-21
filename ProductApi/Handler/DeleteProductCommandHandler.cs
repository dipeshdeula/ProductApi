using MediatR;
using ProductApi.Services;
using ProductApiAsync.Command;

namespace ProductApiAsync.Handler
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
    {
        private readonly IProductService _productService;

        public DeleteProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await _productService.DeleteProductAsync(request.Id);
            return request.Id; // Ensure the method returns the product ID
        }
    }
}
