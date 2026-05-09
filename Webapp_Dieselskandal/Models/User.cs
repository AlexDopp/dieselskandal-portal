namespace Webapp_Dieselskandal.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Vorname { get; set; } = string.Empty;
    public string Nachname { get; set; } = string.Empty;

    // Navigation
    public ICollection<Auftrag> Auftraege { get; set; } = new List<Auftrag>();
}