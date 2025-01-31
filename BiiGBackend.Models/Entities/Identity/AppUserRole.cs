using Microsoft.AspNetCore.Identity;

namespace BiiGBackend.Models.Entities.Identity
{
    public class AppUserRole : IdentityUserRole<Guid>
    {
        public ApplicationUser AppUser { get; set; }
        public AppRole AppRole { get; set; }
    }
}
