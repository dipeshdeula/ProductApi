using MediatR;
using ProductApi.Models;

namespace ProductApiAsync.Queries
{
    public class GetUserByIdQuery : IRequest<User>
    {
        public int UserId { get; set; }
    }
}
