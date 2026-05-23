import { Component, inject } from '@angular/core';
import { NgIf, NgFor, DatePipe, NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { ViewChild } from '@angular/core';
import { ReportService } from '../../../services/report.service';

@Component({
  selector: 'app-employee-report',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, NgClass, FormsModule, MatCardModule, MatTableModule, MatPaginatorModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatCheckboxModule, MatButtonModule],
  template: `
    <div class="page-header"><h1>Employee Report</h1></div>
    <mat-card class="filters-card">
      <div class="filter-row">
        <mat-form-field appearance="outline"><mat-label>Department ID</mat-label><input matInput type="number" [(ngModel)]="filters.departmentId"></mat-form-field>
        <mat-checkbox [(ngModel)]="filters.isActive">Active Only</mat-checkbox>
        <button mat-raised-button color="primary" (click)="loadReport()">Search</button>
      </div>
    </mat-card>
    <mat-card class="table-container mat-elevation-z1">
      <mat-table [dataSource]="records">
        <ng-container matColumnDef="employeeCode"><mat-header-cell *matHeaderCellDef> Code </mat-header-cell><mat-cell *matCellDef="let r">{{ r.employeeCode }}</mat-cell></ng-container>
        <ng-container matColumnDef="firstName"><mat-header-cell *matHeaderCellDef> Name </mat-header-cell><mat-cell *matCellDef="let r">{{ r.firstName }} {{ r.lastName }}</mat-cell></ng-container>
        <ng-container matColumnDef="email"><mat-header-cell *matHeaderCellDef> Email </mat-header-cell><mat-cell *matCellDef="let r">{{ r.email }}</mat-cell></ng-container>
        <ng-container matColumnDef="departmentName"><mat-header-cell *matHeaderCellDef> Department </mat-header-cell><mat-cell *matCellDef="let r">{{ r.departmentName }}</mat-cell></ng-container>
        <ng-container matColumnDef="roleName"><mat-header-cell *matHeaderCellDef> Role </mat-header-cell><mat-cell *matCellDef="let r">{{ r.roleName }}</mat-cell></ng-container>
        <ng-container matColumnDef="isActive"><mat-header-cell *matHeaderCellDef> Active </mat-header-cell><mat-cell *matCellDef="let r"><span class="status-badge" [ngClass]="r.isActive ? 'status-present' : 'status-absent'">{{ r.isActive ? 'Yes' : 'No' }}</span></mat-cell></ng-container>
        <ng-container matColumnDef="totalProjects"><mat-header-cell *matHeaderCellDef> Projects </mat-header-cell><mat-cell *matCellDef="let r">{{ r.totalProjects }}</mat-cell></ng-container>
        <ng-container matColumnDef="totalLeaves"><mat-header-cell *matHeaderCellDef> Leaves </mat-header-cell><mat-cell *matCellDef="let r">{{ r.totalLeaves }}</mat-cell></ng-container>
        <ng-container matColumnDef="totalAttendance"><mat-header-cell *matHeaderCellDef> Attendance </mat-header-cell><mat-cell *matCellDef="let r">{{ r.totalAttendance }}</mat-cell></ng-container>
        <mat-header-row *matHeaderRowDef="displayedColumns"></mat-header-row>
        <mat-row *matRowDef="let row; columns: displayedColumns;"></mat-row>
      </mat-table>
      <mat-paginator [length]="totalCount" [pageSize]="pageSize" (page)="page = $event.pageIndex+1; loadReport()" showFirstLastButtons></mat-paginator>
    </mat-card>
  `,
  styleUrls: ['../attendance-report/report-common.css']
})
export class EmployeeReportComponent {
  private reportService = inject(ReportService);
  records: any[] = [];
  totalCount = 0;
  displayedColumns = ['employeeCode', 'firstName', 'email', 'departmentName', 'roleName', 'isActive', 'totalProjects', 'totalLeaves', 'totalAttendance'];
  filters = { departmentId: undefined as number | undefined, isActive: true };
  page = 1;
  pageSize = 10;

  loadReport(): void {
    this.reportService.getEmployee({ ...this.filters, page: this.page, pageSize: this.pageSize }).subscribe(res => {
      this.records = res.data.items;
      this.totalCount = res.data.totalCount;
    });
  }
}
