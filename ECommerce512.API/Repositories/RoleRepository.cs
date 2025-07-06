using Microsoft.AspNetCore.Identity;

namespace ECommerce512.API.Repositories
{
    public class RoleRepository : Repository<IdentityRole>, IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        //
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
