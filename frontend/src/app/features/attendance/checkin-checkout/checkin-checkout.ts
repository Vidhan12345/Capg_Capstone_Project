import { Component, OnInit, inject } from '@angular/core';
import { NgIf, NgFor, DatePipe, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AttendanceService } from '../../../services/attendance.service';

@Component({
  selector: 'app-checkin-checkout',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, DecimalPipe, FormsModule, MatCardModule, MatButtonModule, MatIconModule, MatSelectModule, MatFormFieldModule, MatSnackBarModule],
  templateUrl: './checkin-checkout.html',
  styleUrl: './checkin-checkout.css'
})
export class CheckinCheckoutComponent implements OnInit {
  private attendanceService = inject(AttendanceService);
  private snackBar = inject(MatSnackBar);

  loading = false;
  todayRecord: any = null;
  today = new Date();
  workMode = 'WFO';
  workModes = ['WFO', 'WFH', 'Hybrid'];

  ngOnInit(): void {
    this.attendanceService.getMyAttendance().subscribe({
      next: (res) => {
        const todayStr = this.today.toISOString().split('T')[0];
        this.todayRecord = res.data.find((a: any) => (a.date.split('T')[0] || a.date) === todayStr) || null;
      }
    });
  }

  private timeNow(): string {
    const now = new Date();
    return now.toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit', second: '2-digit' });
  }

  onCheckIn(): void {
    this.loading = true;
    const time = this.timeNow();
    this.attendanceService.checkIn({ checkIn: time, workMode: this.workMode }).subscribe({
      next: (res) => {
        this.todayRecord = res.data;
        this.snackBar.open(`Checked in at ${time}`, 'Close', { duration: 3000 });
        this.loading = false;
      },
      error: (err) => {
        this.snackBar.open(err.error?.message || 'Check-in failed', 'Close', { duration: 5000 });
        this.loading = false;
      }
    });
  }

  onCheckOut(): void {
    this.loading = true;
    const time = this.timeNow();
    this.attendanceService.checkOut({ checkOut: time }).subscribe({
      next: (res) => {
        this.todayRecord = res.data;
        this.snackBar.open(`Checked out at ${time}`, 'Close', { duration: 3000 });
        this.loading = false;
      },
      error: (err) => {
        this.snackBar.open(err.error?.message || 'Check-out failed', 'Close', { duration: 5000 });
        this.loading = false;
      }
    });
  }
}
