namespace ECommerce512.API.Repositories
{
    public class ApplicationUserOtpRepository : Repository<ApplicationUserOTP>, IApplicationUserOtpRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserOtpRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
