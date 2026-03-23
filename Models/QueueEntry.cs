public class QueueEntry
{
    public int Id { get; set; }

    public int BusinessId { get; set; }
    public Business Business { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int Position { get; set; }
    public string Status { get; set; } // Waiting / Done / Skipped
}