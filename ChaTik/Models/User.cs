using Microsoft.AspNetCore.Identity;

namespace ChaTik.Models;

public class User : IdentityUser
{
    public int Year { get; set; }
}