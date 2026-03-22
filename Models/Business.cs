public class Business
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int OwnerId { get; set; }
    public User Owner { get; set; }
}