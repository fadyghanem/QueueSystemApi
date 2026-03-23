using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string Role { get; set; } // Owner / Customer

    public List<Business> Businesses { get; set; }
}