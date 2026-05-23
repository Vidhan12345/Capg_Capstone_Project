import { Component, OnInit, inject } from '@angular/core';
import { NgIf } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { BaseChartDirective } from 'ng2-charts';
import { ChartData } from 'chart.js';
import { LeaveService } from '../../../services/leave.service';

@Component({
  selector: 'app-leave-chart',
  standalone: true,
  imports: [NgIf, MatCardModule, BaseChartDirective],
  template: `
    <mat-card>
      <mat-card-header><mat-card-title>Leave Statistics</mat-card-title></mat-card-header>
      <mat-card-content>
        <canvas *ngIf="chartData" baseChart [data]="chartData" [options]="chartOptions" [type]="'doughnut'" style="height: 300px"></canvas>
      </mat-card-content>
    </mat-card>
  `
})
export class LeaveChartComponent implements OnInit {
  private leaveService = inject(LeaveService);
  chartData: ChartData<'doughnut'> | null = null;
  chartOptions: any = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { position: 'bottom' }
    }
  };

  ngOnInit(): void {
    this.leaveService.getMyLeaves().subscribe(res => {
      const leaves = res.data ?? [];
      const pending = leaves.filter(l => l.status === 'Pending').length;
      const approved = leaves.filter(l => l.status === 'Approved').length;
      const rejected = leaves.filter(l => l.status === 'Rejected').length;
      const hasData = pending + approved + rejected > 0;
      this.chartData = {
        labels: ['Pending', 'Approved', 'Rejected'],
        datasets: [{
          data: hasData ? [pending, approved, rejected] : [1, 1, 1],
          backgroundColor: ['#f59e0b', '#22c55e', '#ef4444']
        }]
      };
    });
  }
}
