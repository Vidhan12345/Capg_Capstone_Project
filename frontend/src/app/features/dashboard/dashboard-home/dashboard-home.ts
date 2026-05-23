import { Component, OnInit, inject } from '@angular/core';
import { NgIf } from '@angular/common';
import { StatsCardsComponent } from '../stats-cards/stats-cards';
import { AttendanceChartComponent } from '../attendance-chart/attendance-chart';
import { LeaveChartComponent } from '../leave-chart/leave-chart';
import { DepartmentChartComponent } from '../department-chart/department-chart';

@Component({
  selector: 'app-dashboard-home',
  standalone: true,
  imports: [NgIf, StatsCardsComponent, AttendanceChartComponent, LeaveChartComponent, DepartmentChartComponent],
  template: `
    <div class="dashboard">
      <div class="page-header"><h1>Dashboard</h1></div>
      <app-stats-cards></app-stats-cards>
      <div class="charts-grid">
        <app-attendance-chart></app-attendance-chart>
        <app-leave-chart></app-leave-chart>
      </div>
      <app-department-chart></app-department-chart>
    </div>
  `,
  styles: [`
    .dashboard { padding-bottom: 32px; }
    .page-header { margin-bottom: 16px; }
    .page-header h1 { margin: 0; font-size: 1.75rem; font-weight: 500; }
    .charts-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; margin: 24px 0; }
  `]
})
export class DashboardHomeComponent {}
