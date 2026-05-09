import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { AuftraegeService } from '../../core/services/auftraege';

@Component({
  selector: 'app-anlegen',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatDividerModule
  ],
  templateUrl: './anlegen.html',
  styleUrl: './anlegen.scss'
})
export class Anlegen implements OnInit {
  isBearbeiten = false;
  auftragId: number | null = null;

  form = new FormGroup({
    hersteller: new FormControl('', Validators.required),
    modell: new FormControl('', Validators.required),
    baujahr: new FormControl(new Date().getFullYear(), Validators.required),
    fahrgestellnummer: new FormControl('', Validators.required),
    kennzeichen: new FormControl('', Validators.required),
    haendler: new FormControl('', Validators.required),
    kaufpreis: new FormControl(0, Validators.required),
    waehrung: new FormControl(0, Validators.required),
    kaufdatum: new FormControl<Date | null>(null, Validators.required),
    fahrzeugInBesitz: new FormControl(false),
    andereKanzleiBeauftragt: new FormControl(false),
    klageEingereicht: new FormControl(false),
    klageDatum: new FormControl<Date | null>(null)
  });

  constructor(
    private auftraegeService: AuftraegeService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.form.get('klageEingereicht')?.valueChanges.subscribe(checked => {
      const klageDatum = this.form.get('klageDatum');
      if (checked) {
        klageDatum?.setValidators(Validators.required);
      } else {
        klageDatum?.clearValidators();
        klageDatum?.setValue(null);
      }
      klageDatum?.updateValueAndValidity();
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isBearbeiten = true;
      this.auftragId = parseInt(id);
      this.auftraegeService.getAuftrag(this.auftragId).subscribe({
        next: (data) => {
          // Status prüfen
          if (data.status !== 0 && data.status !== 1) {
            this.router.navigate(['/auftraege']);
            return;
          }
          this.form.patchValue({
            hersteller: data.hersteller,
            modell: data.modell,
            baujahr: data.baujahr,
            fahrgestellnummer: data.fahrgestellnummer,
            kennzeichen: data.kennzeichen,
            haendler: data.haendler,
            kaufpreis: data.kaufpreis,
            waehrung: data.waehrung,
            kaufdatum: data.kaufdatum ? new Date(data.kaufdatum) : null,
            fahrzeugInBesitz: data.fahrzeugInBesitz,
            andereKanzleiBeauftragt: data.andereKanzleiBeauftragt,
            klageEingereicht: data.klageEingereicht,
            klageDatum: data.klageDatum ? new Date(data.klageDatum) : null
          });
        },
        error: (err) => console.error(err)
      });
    }
  }

  onSubmit(): void {
    if (this.form.valid) {
      if (this.isBearbeiten && this.auftragId) {
        this.auftraegeService.updateAuftrag(this.auftragId, this.form.value).subscribe({
          next: () => this.router.navigate(['/auftraege']),
          error: (err) => console.error(err)
        });
      } else {
        this.auftraegeService.createAuftrag(this.form.value).subscribe({
          next: () => this.router.navigate(['/auftraege']),
          error: (err) => console.error(err)
        });
      }
    } else {
      // Fehlende Pflichtfelder werden markiert
      this.form.markAllAsTouched();
    }
  }

  onAbbrechen(): void {
    this.router.navigate([this.isBearbeiten ? '/auftraege' : '/dashboard']);
  }
}
