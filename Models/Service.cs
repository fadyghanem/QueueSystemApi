using System.ComponentModel.DataAnnotations;

public class Service
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }

    public int BusinessId { get; set; }
    public Business Business { get; set; }
}