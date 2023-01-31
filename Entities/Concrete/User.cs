using Microsoft.AspNetCore.Identity;

namespace Entities.Concrete;

public class User : IdentityUser<int>
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? ImageUrl { get; set; }
    public long CreatedAt { get; set; }
    public long ModifiedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
}

public class Role : IdentityRole<int>
{
    public string? Description { get; set; }
}
public class UserRole : IdentityUserRole<int>
{

}
public class RoleClaim : IdentityRoleClaim<int>
{

}
public class UserClaim : IdentityUserClaim<int>
{

}
public class UserLogin : IdentityUserLogin<int>
{

}
public class UserToken : IdentityUserToken<int>
{

}