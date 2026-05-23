import { Component, OnInit, inject } from '@angular/core';
import { NgIf, NgFor, DatePipe, DecimalPipe, NgClass } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { AttendanceService } from '../../../services/attendance.service';
import { AttendanceDto } from '../../../models/attendance';

@Component({
  selector: 'app-monthly-view',
  standalone: true,
  imports: [NgIf, NgFor, NgClass, MatCardModule, MatTableModule, DatePipe, DecimalPipe],
  template: `
    <div class="page-header"><h1>Monthly Attendance</h1></div>
    <mat-card class="table-container mat-elevation-z1">
      <mat-table [dataSource]="records">
        <ng-container matColumnDef="date">
          <mat-header-cell *matHeaderCellDef> Date </mat-header-cell>
          <mat-cell *matCellDef="let a"> {{ a.date | date:'mediumDate' }} </mat-cell>
        </ng-container>
        <ng-container matColumnDef="dayOfWeek">
          <mat-header-cell *matHeaderCellDef> Day </mat-header-cell>
          <mat-cell *matCellDef="let a"> {{ getDayName(a.date) }} </mat-cell>
        </ng-container>
        <ng-container matColumnDef="checkIn">
          <mat-header-cell *matHeaderCellDef> In </mat-header-cell>
          <mat-cell *matCellDef="let a"> {{ a.checkIn }} </mat-cell>
        </ng-container>
        <ng-container matColumnDef="checkOut">
          <mat-header-cell *matHeaderCellDef> Out </mat-header-cell>
          <mat-cell *matCellDef="let a"> {{ a.checkOut || '-' }} </mat-cell>
        </ng-container>
        <ng-container matColumnDef="totalHours">
          <mat-header-cell *matHeaderCellDef> Hours </mat-header-cell>
          <mat-cell *matCellDef="let a"> {{ a.totalHours != null ? (a.totalHours | number:'1.1-1') : '-' }} </mat-cell>
        </ng-container>
        <ng-container matColumnDef="status">
          <mat-header-cell *matHeaderCellDef> Status </mat-header-cell>
          <mat-cell *matCellDef="let a">
            <span class="status-badge" [ngClass]="'status-' + a.status.toLowerCase()">{{ a.status }}</span>
          </mat-cell>
        </ng-container>
        <mat-header-row *matHeaderRowDef="displayedColumns"></mat-header-row>
        <mat-row *matRowDef="let row; columns: displayedColumns;"></mat-row>
      </mat-table>
    </mat-card>
  `,
  styles: [``]
})
export class MonthlyViewComponent implements OnInit {
  private attendanceService = inject(AttendanceService);
  records: AttendanceDto[] = [];
  displayedColumns = ['date', 'dayOfWeek', 'checkIn', 'checkOut', 'totalHours', 'status'];

  ngOnInit(): void {
    this.attendanceService.getMyAttendance().subscribe(res => this.records = res.data);
  }

  getDayName(dateStr: string): string {
    const days = ['Sunday','Monday','Tuesday','Wednesday','Thursday','Friday','Saturday'];
    return days[new Date(dateStr).getDay()] || '';
  }
}
