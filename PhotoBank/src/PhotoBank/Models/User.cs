using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PhotoBank.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}
