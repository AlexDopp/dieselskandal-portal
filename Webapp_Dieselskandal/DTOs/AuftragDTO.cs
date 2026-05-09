using Webapp_Dieselskandal.Models;

namespace Webapp_Dieselskandal.DTOs;

public class AuftragDto
{
    public string Hersteller { get; set; } = string.Empty;
    public string Modell { get; set; } = string.Empty;
    public int Baujahr { get; set; }
    public string Fahrgestellnummer { get; set; } = string.Empty;
    public string Kennzeichen { get; set; } = string.Empty;
    public decimal Kaufpreis { get; set; }
    public Waehrung Waehrung { get; set; } = Waehrung.EUR;
    public DateTime Kaufdatum { get; set; }
    public string Haendler { get; set; } = string.Empty;
    public bool FahrzeugInBesitz { get; set; }
    public bool AndereKanzleiBeauftragt { get; set; }
    public bool KlageEingereicht { get; set; }
    public DateTime? KlageDatum { get; set; }
}