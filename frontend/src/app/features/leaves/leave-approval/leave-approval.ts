import { Component, OnInit, inject } from '@angular/core';
import { NgIf, DatePipe } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { LeaveService } from '../../../services/leave.service';
import { LeaveDto } from '../../../models/leave';

@Component({
  selector: 'app-leave-approval',
  standalone: true,
  imports: [NgIf, DatePipe, MatTableModule, MatButtonModule, MatIconModule, MatChipsModule, MatCardModule, MatSnackBarModule],
  templateUrl: './leave-approval.html',
  styleUrl: './leave-approval.css'
})
export class LeaveApprovalComponent implements OnInit {
  private leaveService = inject(LeaveService);
  private snackBar = inject(MatSnackBar);

  pendingLeaves: LeaveDto[] = [];
  displayedColumns = ['employeeName', 'fromDate', 'toDate', 'leaveType', 'reason', 'appliedAt', 'actions'];

  ngOnInit(): void {
    this.loadPending();
  }

  loadPending(): void {
    this.leaveService.getPending().subscribe(res => this.pendingLeaves = res.data);
  }

  onApprove(id: number): void {
    this.leaveService.approve(id).subscribe({
      next: () => {
        this.snackBar.open('Leave approved', 'Close', { duration: 3000 });
        this.loadPending();
      },
      error: (err) => this.snackBar.open(err.error?.message || 'Approval failed', 'Close', { duration: 5000 })
    });
  }

  onReject(id: number): void {
    this.leaveService.reject(id).subscribe({
      next: () => {
        this.snackBar.open('Leave rejected', 'Close', { duration: 3000 });
        this.loadPending();
      },
      error: (err) => this.snackBar.open(err.error?.message || 'Rejection failed', 'Close', { duration: 5000 })
    });
  }
}
