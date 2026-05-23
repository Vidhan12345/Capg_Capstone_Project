import { Component, OnInit, inject } from '@angular/core';
import { NgIf } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';
import { DashboardService } from '../../../services/dashboard.service';

@Component({
  selector: 'app-attendance-chart',
  standalone: true,
  imports: [NgIf, MatCardModule, BaseChartDirective],
  template: `
    <mat-card>
      <mat-card-header><mat-card-title>Attendance Trend (7 days)</mat-card-title></mat-card-header>
      <mat-card-content>
        <canvas *ngIf="chartData" baseChart [data]="chartData" [options]="chartOptions" [type]="'line'" style="height: 300px"></canvas>
      </mat-card-content>
    </mat-card>
  `
})
export class AttendanceChartComponent implements OnInit {
  private dashboardService = inject(DashboardService);
  chartData: ChartData<'line'> | null = null;
  chartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: { legend: { position: 'bottom' } },
    scales: {
      y: { beginAtZero: true, ticks: { stepSize: 1 } }
    }
  };

  ngOnInit(): void {
    this.dashboardService.getAttendanceTrend(7).subscribe((res: any) => {
      const data = res.data;
      this.chartData = {
        labels: data.map((d: any) => new Date(d.date).toLocaleDateString('en-US', { weekday: 'short' })),
        datasets: [
          { label: 'Present', data: data.map((d: any) => d.present), borderColor: '#22c55e', backgroundColor: 'rgba(34,197,94,0.1)', tension: 0.3, fill: true },
          { label: 'Absent', data: data.map((d: any) => d.absent), borderColor: '#ef4444', backgroundColor: 'rgba(239,68,68,0.1)', tension: 0.3, fill: true }
        ]
      };
    });
  }
}
