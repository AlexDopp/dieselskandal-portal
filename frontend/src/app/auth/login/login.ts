import { Component, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { MatCard, MatCardActions, MatCardContent } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../core/services/auth';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrl: './login.scss',
  imports: [
    MatCard,
    MatCardContent,
    MatCardActions,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    ReactiveFormsModule
  ]
})
export class Login {
  @ViewChild('submitBtn') submitBtn!: ElementRef;

  form = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(4)])
  });

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onEnter(): void {
    // Button kurz animieren
    const btn = this.submitBtn?.nativeElement;
    if (btn) {
      btn.classList.add('btn-pressed');
      setTimeout(() => btn.classList.remove('btn-pressed'), 150);
    }
    this.onLogin();
  }

  onLogin(): void {
    if (this.form.valid) {
      this.authService.login(
        this.form.value.email!,
        this.form.value.password!
      ).subscribe({
        next: () => this.router.navigate(['/dashboard']),
        error: () => console.error('Login fehlgeschlagen')
      });
    }
  }
}
