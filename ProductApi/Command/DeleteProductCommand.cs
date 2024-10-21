using MediatR;

namespace ProductApiAsync.Command
{
    public class DeleteProductCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
