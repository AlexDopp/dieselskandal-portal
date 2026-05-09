import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { DxDataGridModule, DxButtonModule } from 'devextreme-angular';
import { AuftraegeService } from '../../core/services/auftraege';

@Component({
  selector: 'app-liste',
  imports: [DxDataGridModule, DxButtonModule],
  templateUrl: './liste.html',
  styleUrl: './liste.scss'
})
export class Liste implements OnInit {
  auftraege: any[] = [];

  statusLookup = [
    { value: 0, text: 'Eingereicht' },
    { value: 1, text: 'Angenommen' },
    { value: 2, text: 'In Bearbeitung' },
    { value: 3, text: 'Abgeschlossen' },
    { value: 4, text: 'Abgelehnt' }
  ];

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
        this.cdr.detectChanges();       // Aktualisierung erzwingen
      },
      error: (err) => console.error(err)
    });
  }

  onAnlegen(): void {
    this.router.navigate(['/auftraege/anlegen']);
  }

  onBearbeiten(e: any): void {
    this.router.navigate(['/auftraege/bearbeiten', e.row.data.id]);
  }

  onLoeschen(e: any): void {
    const id = e.row.data.id;
    this.auftraegeService.deleteAuftrag(id).subscribe({
      next: () => {
        this.auftraege = this.auftraege.filter(a => a.id !== id);
        this.cdr.detectChanges();
      },
      error: (err) => console.error(err)
    });
  }

  getStatusText(value: number): string {
    return this.statusLookup.find(s => s.value === value)?.text ?? '';
  }

  getWaehrungText(value: number): string {
    return this.waehrungLookup.find(w => w.value === value)?.text ?? '';
  }

  getKlageText = (rowData: any): string => {
    return rowData.klageEingereicht ? '✅' : '❌';
  }

}
