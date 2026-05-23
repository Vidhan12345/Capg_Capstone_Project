import { Component, OnInit, inject } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { DashboardService } from '../../../services/dashboard.service';
import { DashboardStatsDto } from '../../../models/dashboard';

@Component({
  selector: 'app-stats-cards',
  standalone: true,
  imports: [NgIf, NgFor, MatCardModule, MatIconModule],
  template: `
    <div class="stats-grid">
      <mat-card class="stat-card" *ngFor="let stat of stats">
        <mat-card-content>
          <div class="stat-content">
            <div>
              <div class="stat-value">{{ stat.value }}</div>
              <div class="stat-label">{{ stat.label }}</div>
            </div>
            <mat-icon class="stat-icon" [style.color]="stat.color">{{ stat.icon }}</mat-icon>
          </div>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .stats-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(180px, 1fr)); gap: 16px; }
    .stat-card { border-radius: 12px; }
    .stat-content { display: flex; justify-content: space-between; align-items: center; padding: 8px 0; }
    .stat-value { font-size: 2rem; font-weight: 700; }
    .stat-label { font-size: 0.85rem; color: #666; margin-top: 4px; }
    .stat-icon { font-size: 40px; width: 40px; height: 40px; opacity: 0.7; }
  `]
})
export class StatsCardsComponent implements OnInit {
  private dashboardService = inject(DashboardService);
  stats: { label: string; value: number; icon: string; color: string }[] = [];

  ngOnInit(): void {
    this.dashboardService.getStats().subscribe((res: any) => {
      const d: DashboardStatsDto = res.data;
      this.stats = [
        { label: 'Total Employees', value: d.totalEmployees, icon: 'people', color: '#3b82f6' },
        { label: 'Present Today', value: d.presentToday, icon: 'check_circle', color: '#22c55e' },
        { label: 'Absent Today', value: d.absentToday, icon: 'cancel', color: '#ef4444' },
        { label: 'Pending Leaves', value: d.pendingLeaves, icon: 'pending_actions', color: '#f59e0b' },
        { label: 'Active Projects', value: d.activeProjects, icon: 'assignment_turned_in', color: '#8b5cf6' }
      ];
    });
  }
}
