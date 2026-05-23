import { Component, OnInit, inject } from '@angular/core';
import { NgIf, DatePipe, DecimalPipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { AttendanceService } from '../../../services/attendance.service';
import { AttendanceDto } from '../../../models/attendance';

@Component({
  selector: 'app-timesheet-report',
  standalone: true,
  imports: [NgIf, DatePipe, DecimalPipe, MatCardModule, MatTableModule, MatFormFieldModule, MatDatepickerModule, MatNativeDateModule, MatInputModule, FormsModule, MatButtonModule],
  template: `
    <div class="page-header"><h1>Timesheet Report</h1></div>
    <mat-card class="filters-card">
      <div class="filter-row">
        <mat-form-field appearance="outline">
          <mat-label>From Date</mat-label>
          <input matInput [matDatepicker]="fromPicker" [(ngModel)]="fromDate">
          <mat-datepicker-toggle matSuffix [for]="fromPicker"></mat-datepicker-toggle>
          <mat-datepicker #fromPicker></mat-datepicker>
        </mat-form-field>
        <mat-form-field appearance="outline">
          <mat-label>To Date</mat-label>
          <input matInput [matDatepicker]="toPicker" [(ngModel)]="toDate">
          <mat-datepicker-toggle matSuffix [for]="toPicker"></mat-datepicker-toggle>
          <mat-datepicker #toPicker></mat-datepicker>
        </mat-form-field>
        <button mat-raised-button color="primary" (click)="loadReport()">Generate</button>
      </div>
    </mat-card>
    <mat-card class="table-container mat-elevation-z1" *ngIf="records.length">
      <mat-table [dataSource]="records">
        <ng-container matColumnDef="date">
          <mat-header-cell *matHeaderCellDef> Date </mat-header-cell>
          <mat-cell *matCellDef="let a"> {{ a.date | date:'mediumDate' }} </mat-cell>
        </ng-container>
        <ng-container matColumnDef="checkIn">
          <mat-header-cell *matHeaderCellDef> Check In </mat-header-cell>
          <mat-cell *matCellDef="let a"> {{ a.checkIn }} </mat-cell>
        </ng-container>
        <ng-container matColumnDef="checkOut">
          <mat-header-cell *matHeaderCellDef> Check Out </mat-header-cell>
          <mat-cell *matCellDef="let a"> {{ a.checkOut || '-' }} </mat-cell>
        </ng-container>
        <ng-container matColumnDef="totalHours">
          <mat-header-cell *matHeaderCellDef> Hours </mat-header-cell>
          <mat-cell *matCellDef="let a"> {{ a.totalHours != null ? (a.totalHours | number:'1.1-1') : '-' }} </mat-cell>
        </ng-container>
        <ng-container matColumnDef="status">
          <mat-header-cell *matHeaderCellDef> Status </mat-header-cell>
          <mat-cell *matCellDef="let a"> {{ a.status }} </mat-cell>
        </ng-container>
        <mat-header-row *matHeaderRowDef="displayedColumns"></mat-header-row>
        <mat-row *matRowDef="let row; columns: displayedColumns;"></mat-row>
      </mat-table>
    </mat-card>
  `,
  styles: [`
    .filters-card { padding: 16px; margin-bottom: 16px; }
    .filter-row { display: flex; gap: 16px; align-items: center; }
  `]
})
export class TimesheetReportComponent {
  private attendanceService = inject(AttendanceService);
  records: AttendanceDto[] = [];
  displayedColumns = ['date', 'checkIn', 'checkOut', 'totalHours', 'status'];
  fromDate: Date = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
  toDate: Date = new Date();

  loadReport(): void {
    this.attendanceService.getMyAttendance().subscribe(res => {
      const from = this.fromDate.toISOString().split('T')[0];
      const to = this.toDate.toISOString().split('T')[0];
      this.records = res.data.filter((a: any) => {
        const d = a.date.split('T')[0] || a.date;
        return d >= from && d <= to;
      });
    });
  }
}
