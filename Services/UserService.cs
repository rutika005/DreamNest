using Aesthetica.Models;

public class UserService
{
    private readonly AppDbContext _context; // Database context

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    //public bool IsUserActive(int userId)
    //{
    //    var user = _context.registeruser.FirstOrDefault(u => u.Id == userId);
    //    return user?.status ?? false; // Returns true if active, false otherwise
    //}
}
