using System.Text.Json.Serialization;

namespace Webapp_Dieselskandal.Models;

public enum AuftragStatus
{
    Eingereicht,
    Angenommen,
    InBearbeitung,
    Abgeschlossen,
    Abgelehnt
}
public enum Waehrung
{
    EUR,
    USD,
    GBP,
    CHF
}

public class Auftrag
{
    public int Id { get; set; }
    public AuftragStatus Status { get; set; } = AuftragStatus.Eingereicht;
    public DateTime ErstelltAm { get; set; } = DateTime.UtcNow;
    public DateTime? AktualisiertAm { get; set; }

    // Fahrzeugdaten
    public string Hersteller { get; set; } = string.Empty;
    public string Modell { get; set; } = string.Empty;
    public int Baujahr { get; set; }
    public string Fahrgestellnummer { get; set; } = string.Empty;
    public string Kennzeichen { get; set; } = string.Empty;
    public decimal Kaufpreis { get; set; }
    public Waehrung Waehrung { get; set; } = Waehrung.EUR;
    public DateTime Kaufdatum { get; set; }
    public string Haendler { get; set; } = string.Empty;
    
    // Zusatzinfos
    public bool FahrzeugInBesitz { get; set; }
    public bool AndereKanzleiBeauftragt { get; set; }
    public bool KlageEingereicht { get; set; }
    public DateTime? KlageDatum { get; set; }

    // User
    public int UserId { get; set; }
    
    [JsonIgnore]                    // Da Userinfos bereits aus JWT Token kommen
    public User User { get; set; } = null!;
}