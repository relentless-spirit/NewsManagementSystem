namespace NewsManagementSystem.Models;

public class ProfileAccounViewModel
{
    public short Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int? Role { get; set; }
    public string RoleName
    {
        get
        {
            return Role switch
            {
                1 => "Staff",
                2 => "Lecture",
            };
        }
    }
}