import { Component, OnInit, inject } from '@angular/core';
import { NgIf, NgFor, DatePipe, DecimalPipe } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatCardModule } from '@angular/material/card';
import { AttendanceService } from '../../../services/attendance.service';
import { AttendanceDto } from '../../../models/attendance';

@Component({
  selector: 'app-attendance-list',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, DecimalPipe, MatTableModule, MatChipsModule, MatCardModule],
  template: `
    <div class="page-header"><h1>My Attendance</h1></div>
    <mat-card class="table-container mat-elevation-z1">
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
        <ng-container matColumnDef="workMode">
          <mat-header-cell *matHeaderCellDef> Mode </mat-header-cell>
          <mat-cell *matCellDef="let a"> {{ a.workMode }} </mat-cell>
        </ng-container>
        <ng-container matColumnDef="status">
          <mat-header-cell *matHeaderCellDef> Status </mat-header-cell>
          <mat-cell *matCellDef="let a">
            <mat-chip [color]="a.status === 'Present' ? 'primary' : (a.status === 'HalfDay' ? 'accent' : 'warn')" highlighted>{{ a.status }}</mat-chip>
          </mat-cell>
        </ng-container>
        <mat-header-row *matHeaderRowDef="displayedColumns"></mat-header-row>
        <mat-row *matRowDef="let row; columns: displayedColumns;"></mat-row>
      </mat-table>
    </mat-card>
  `,
  styles: [``]
})
export class AttendanceListComponent implements OnInit {
  private attendanceService = inject(AttendanceService);
  records: AttendanceDto[] = [];
  displayedColumns = ['date', 'checkIn', 'checkOut', 'totalHours', 'workMode', 'status'];

  ngOnInit(): void {
    this.attendanceService.getMyAttendance().subscribe(res => this.records = res.data);
  }
}
