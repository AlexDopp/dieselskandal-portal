import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthService } from '../../core/services/auth';

@Component({
  selector: 'app-banner',
  imports: [RouterOutlet, MatIconModule, MatButtonModule, MatTooltipModule],
  templateUrl: './banner.html',
  styleUrl: './banner.scss'
})
export class Banner {
  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  onHome(): void {
    this.router.navigate(['/dashboard']);
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }
}
