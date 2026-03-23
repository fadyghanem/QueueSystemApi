public class Appointment
{
    public int Id { get; set; }

    public int BusinessId { get; set; }
    public Business Business { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int ServiceId { get; set; }
    public Service Service { get; set; }

    public DateTime StartTime { get; set; }
    public string Status { get; set; } // Scheduled / Completed / Cancelled
}