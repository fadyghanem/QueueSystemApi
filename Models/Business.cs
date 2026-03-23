using System.ComponentModel.DataAnnotations;

public class Business
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public int OwnerId { get; set; }
    public User Owner { get; set; }

    public List<Service> Services { get; set; }
}