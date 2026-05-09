import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { DecimalPipe } from '@angular/common';
import { AuftraegeService } from '../core/services/auftraege';

@Component({
  selector: 'app-dashboard',
  imports: [MatCardModule, DecimalPipe],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard implements OnInit {
  auftraege: any[] = [];

  waehrungLookup = [
    { value: 0, text: 'EUR' },
    { value: 1, text: 'USD' },
    { value: 2, text: 'GBP' },
    { value: 3, text: 'CHF' }
  ];

  constructor(
    private auftraegeService: AuftraegeService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.auftraegeService.getAuftraege().subscribe({
      next: (data) => {
        this.auftraege = data;
        this.cdr.detectChanges();
      },
      error: (err) => console.error(err)
    });
  }

  getWaehrungText(value: number): string {
    return this.waehrungLookup.find(w => w.value === value)?.text ?? '';
  }

  getKurzinfo(auftrag: any): string {
    return `${auftrag.hersteller} ${auftrag.modell} ${auftrag.baujahr}`;
  }

  getStatusText(status: number): string {
    switch (status) {
      case 0: return 'Eingereicht';
      case 1: return 'Angenommen';
      case 2: return 'In Bearbeitung';
      case 3: return 'Abgeschlossen';
      case 4: return 'Abgelehnt';
      default: return 'Unbekannt';
    }
  }

  navigateTo(path: string): void {
    this.router.navigate([path]);
  }
}
