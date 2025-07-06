using Microsoft.AspNetCore.Identity;

namespace ECommerce512.API.Repositories
{
    public class OrderItemRepository : Repository<OrderItems>, IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        //
        public OrderItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
