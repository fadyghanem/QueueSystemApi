public class AppointmentDto
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public DateTime StartTime { get; set; }
    public string Status { get; set; } = string.Empty;
}