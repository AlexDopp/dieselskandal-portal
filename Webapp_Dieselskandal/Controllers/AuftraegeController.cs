using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Webapp_Dieselskandal.Data;
using Webapp_Dieselskandal.DTOs;
using Webapp_Dieselskandal.Models;

namespace Webapp_Dieselskandal.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]     // Nur mit gültigem JWT Token
public class AuftraegeController : ControllerBase
{
    // Dependancy Injection
    private readonly AppDbContext _context;
    public AuftraegeController(AppDbContext context)
    {
        _context = context;
    }

    // GET alles
    [HttpGet]
    public async Task<IActionResult> GetAuftraege()
    {
        // UserId aus Token extrahieren und als int speichern
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Abfrage aller Aufträge unter aktueller userId (aus Token)
        var auftraege = await _context.Auftraege
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.ErstelltAm)
            .ToListAsync();

        return Ok(auftraege);
    }

    // GET spezifische id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuftrag(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var auftrag = await _context.Auftraege
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (auftrag == null)
            return NotFound();

        return Ok(auftrag);
    }

    // Neuen Auftrag anlegen
    [HttpPost]
    public async Task<IActionResult> CreateAuftrag([FromBody] AuftragDto dto)
    {   // RequestBody wird zu DTO umgewandelt
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        var auftrag = new Auftrag
        {
            UserId = userId,
            Status = AuftragStatus.Eingereicht,
            ErstelltAm = DateTime.UtcNow,
            
            Hersteller = dto.Hersteller,
            Modell = dto.Modell,
            Baujahr = dto.Baujahr,
            Fahrgestellnummer = dto.Fahrgestellnummer,
            Kennzeichen = dto.Kennzeichen,
            Kaufpreis = dto.Kaufpreis,
            Waehrung = dto.Waehrung,
            Kaufdatum = dto.Kaufdatum.ToUniversalTime(),
            Haendler = dto.Haendler,
            FahrzeugInBesitz = dto.FahrzeugInBesitz,
            AndereKanzleiBeauftragt = dto.AndereKanzleiBeauftragt,
            KlageEingereicht = dto.KlageEingereicht,
            KlageDatum = dto.KlageDatum.HasValue ? dto.KlageDatum.Value.ToUniversalTime() : null
        };

        _context.Auftraege.Add(auftrag);        // Change gequeued
        await _context.SaveChangesAsync();      // INSERT

        return CreatedAtAction(nameof(GetAuftrag), new { id = auftrag.Id }, auftrag.Id);
    }
    
    // Auftrag anpassen
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuftrag(int id, [FromBody] AuftragDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Nur wenn Auftrag existiert und UserID passt
        var auftrag = await _context.Auftraege
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (auftrag == null)
            return NotFound();
        
        /*  Auftrag nur bearbeitbar wenn "Eingereicht" oder "Angenommen"
            und wird zurück auf "Eingereicht" gesetzt
            
        if (auftrag.Status != AuftragStatus.Eingereicht && 
            auftrag.Status != AuftragStatus.Angenommen)
            return BadRequest("Auftrag kann nicht mehr bearbeitet werden");
        if (auftrag.Status == AuftragStatus.Angenommen)
            auftrag.Status = AuftragStatus.Eingereicht;
        */
        
        auftrag.Hersteller = dto.Hersteller;
        auftrag.Modell = dto.Modell;
        auftrag.Baujahr = dto.Baujahr;
        auftrag.Fahrgestellnummer = dto.Fahrgestellnummer;
        auftrag.Kennzeichen = dto.Kennzeichen;
        auftrag.Kaufpreis = dto.Kaufpreis;
        auftrag.Waehrung = dto.Waehrung;
        auftrag.Kaufdatum = dto.Kaufdatum.ToUniversalTime();
        auftrag.Haendler = dto.Haendler;
        auftrag.FahrzeugInBesitz = dto.FahrzeugInBesitz;
        auftrag.AndereKanzleiBeauftragt = dto.AndereKanzleiBeauftragt;
        auftrag.KlageEingereicht = dto.KlageEingereicht;
        auftrag.KlageDatum = dto.KlageDatum.HasValue ? dto.KlageDatum.Value.ToUniversalTime() : null;
        auftrag.AktualisiertAm = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Auftrag löschen
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuftrag(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Nur wenn Auftrag existiert und UserID passt
        var auftrag = await _context.Auftraege
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (auftrag == null)
            return NotFound();

        _context.Auftraege.Remove(auftrag);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}