import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [RouterLink, MatCardModule, MatButtonModule, MatIconModule],
  template: `
    <div class="error-container">
      <mat-card class="error-card">
        <mat-icon class="error-icon">search_off</mat-icon>
        <h1>404 - Page Not Found</h1>
        <p>The page you're looking for doesn't exist.</p>
        <button mat-raised-button color="primary" routerLink="/dashboard">
          <mat-icon>arrow_back</mat-icon> Go to Dashboard
        </button>
      </mat-card>
    </div>
  `,
  styles: [`
    .error-container {
      display: flex;
      align-items: center;
      justify-content: center;
      min-height: 60vh;
    }
    .error-card {
      text-align: center;
      padding: 48px;
      max-width: 480px;
    }
    .error-icon {
      font-size: 64px;
      width: 64px;
      height: 64px;
      color: #f59e0b;
    }
    h1 { margin: 16px 0; font-size: 2rem; }
    p { color: #64748b; margin-bottom: 24px; }
  `]
})
export class NotFoundComponent {}
