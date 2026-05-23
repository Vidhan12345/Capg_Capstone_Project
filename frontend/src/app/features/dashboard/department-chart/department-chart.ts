import { Component, OnInit, inject } from '@angular/core';
import { NgIf } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { BaseChartDirective } from 'ng2-charts';
import { ChartData } from 'chart.js';
import { DashboardService } from '../../../services/dashboard.service';

@Component({
  selector: 'app-department-chart',
  standalone: true,
  imports: [NgIf, MatCardModule, BaseChartDirective],
  template: `
    <mat-card>
      <mat-card-header><mat-card-title>Department Distribution</mat-card-title></mat-card-header>
      <mat-card-content>
        <canvas *ngIf="chartData" baseChart [data]="chartData" [options]="chartOptions" [type]="'bar'" style="height: 300px"></canvas>
      </mat-card-content>
    </mat-card>
  `
})
export class DepartmentChartComponent implements OnInit {
  private dashboardService = inject(DashboardService);
  chartData: ChartData<'bar'> | null = null;
  chartOptions: any = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: { legend: { display: false } },
    scales: {
      y: { beginAtZero: true, ticks: { stepSize: 1 } }
    }
  };

  ngOnInit(): void {
    this.dashboardService.getDepartmentDistribution().subscribe((res: any) => {
      const data = res.data;
      this.chartData = {
        labels: data.map((d: any) => d.department),
        datasets: [{
          label: 'Employees',
          data: data.map((d: any) => d.count),
          backgroundColor: ['#3b82f6', '#22c55e', '#f59e0b', '#ef4444', '#8b5cf6', '#ec4899', '#06b6d4']
        }]
      };
    });
  }
}
