import { Component, inject } from '@angular/core';
import { NgIf, NgFor, DatePipe, NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { ViewChild } from '@angular/core';
import { ReportService } from '../../../services/report.service';

@Component({
  selector: 'app-leave-report',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, NgClass, FormsModule, MatCardModule, MatTableModule, MatPaginatorModule, MatFormFieldModule, MatInputModule, MatDatepickerModule, MatNativeDateModule, MatSelectModule, MatButtonModule],
  template: `
    <div class="page-header"><h1>Leave Report</h1></div>
    <mat-card class="filters-card">
      <div class="filter-row">
        <mat-form-field appearance="outline"><mat-label>From</mat-label><input matInput [matDatepicker]="f1" [(ngModel)]="filters.fromDate"><mat-datepicker-toggle matSuffix [for]="f1"></mat-datepicker-toggle><mat-datepicker #f1></mat-datepicker></mat-form-field>
        <mat-form-field appearance="outline"><mat-label>To</mat-label><input matInput [matDatepicker]="f2" [(ngModel)]="filters.toDate"><mat-datepicker-toggle matSuffix [for]="f2"></mat-datepicker-toggle><mat-datepicker #f2></mat-datepicker></mat-form-field>
        <mat-form-field appearance="outline"><mat-label>Status</mat-label><mat-select [(ngModel)]="filters.status"><mat-option value="">All</mat-option><mat-option value="Pending">Pending</mat-option><mat-option value="Approved">Approved</mat-option><mat-option value="Rejected">Rejected</mat-option></mat-select></mat-form-field>
        <button mat-raised-button color="primary" (click)="loadReport()">Search</button>
      </div>
    </mat-card>
    <mat-card class="table-container mat-elevation-z1">
      <mat-table [dataSource]="records">
        <ng-container matColumnDef="employeeName"><mat-header-cell *matHeaderCellDef> Name </mat-header-cell><mat-cell *matCellDef="let r">{{ r.employeeName }}</mat-cell></ng-container>
        <ng-container matColumnDef="leaveType"><mat-header-cell *matHeaderCellDef> Type </mat-header-cell><mat-cell *matCellDef="let r">{{ r.leaveType }}</mat-cell></ng-container>
        <ng-container matColumnDef="fromDate"><mat-header-cell *matHeaderCellDef> From </mat-header-cell><mat-cell *matCellDef="let r">{{ r.fromDate | date:'shortDate' }}</mat-cell></ng-container>
        <ng-container matColumnDef="toDate"><mat-header-cell *matHeaderCellDef> To </mat-header-cell><mat-cell *matCellDef="let r">{{ r.toDate | date:'shortDate' }}</mat-cell></ng-container>
        <ng-container matColumnDef="daysRequested"><mat-header-cell *matHeaderCellDef> Days </mat-header-cell><mat-cell *matCellDef="let r">{{ r.daysRequested }}</mat-cell></ng-container>
        <ng-container matColumnDef="reason"><mat-header-cell *matHeaderCellDef> Reason </mat-header-cell><mat-cell *matCellDef="let r">{{ r.reason }}</mat-cell></ng-container>
        <ng-container matColumnDef="status"><mat-header-cell *matHeaderCellDef> Status </mat-header-cell><mat-cell *matCellDef="let r"><span class="status-badge" [ngClass]="'status-' + r.status.toLowerCase()">{{ r.status }}</span></mat-cell></ng-container>
        <mat-header-row *matHeaderRowDef="displayedColumns"></mat-header-row>
        <mat-row *matRowDef="let row; columns: displayedColumns;"></mat-row>
      </mat-table>
      <mat-paginator [length]="totalCount" [pageSize]="pageSize" (page)="page = $event.pageIndex+1; loadReport()" showFirstLastButtons></mat-paginator>
    </mat-card>
  `,
  styleUrls: ['../attendance-report/report-common.css']
})
export class LeaveReportComponent {
  private reportService = inject(ReportService);
  records: any[] = [];
  totalCount = 0;
  displayedColumns = ['employeeName', 'leaveType', 'fromDate', 'toDate', 'daysRequested', 'reason', 'status'];
  filters = { fromDate: '', toDate: '', status: '' };
  page = 1;
  pageSize = 10;

  loadReport(): void {
    this.reportService.getLeave({ ...this.filters, page: this.page, pageSize: this.pageSize }).subscribe(res => {
      this.records = res.data.items;
      this.totalCount = res.data.totalCount;
    });
  }
}
