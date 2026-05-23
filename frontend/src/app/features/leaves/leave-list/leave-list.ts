import { Component, OnInit, inject } from '@angular/core';
import { NgIf, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { LeaveService } from '../../../services/leave.service';
import { LeaveDto } from '../../../models/leave';

@Component({
  selector: 'app-leave-list',
  standalone: true,
  imports: [NgIf, DatePipe, RouterLink, MatTableModule, MatButtonModule, MatIconModule, MatChipsModule, MatCardModule, MatSnackBarModule],
  templateUrl: './leave-list.html',
  styleUrl: './leave-list.css'
})
export class LeaveListComponent implements OnInit {
  private leaveService = inject(LeaveService);
  private snackBar = inject(MatSnackBar);
  leaves: LeaveDto[] = [];
  displayedColumns = ['fromDate', 'toDate', 'leaveType', 'reason', 'status', 'appliedAt', 'actions'];

  ngOnInit(): void {
    this.leaveService.getMyLeaves().subscribe(res => this.leaves = res.data);
  }

  getStatusColor(status: string): string {
    return status === 'Approved' ? 'primary' : status === 'Rejected' ? 'warn' : 'accent';
  }

  onCancel(leave: LeaveDto): void {
    if (confirm(`Cancel ${leave.leaveType} leave from ${leave.fromDate}?`)) {
      this.leaveService.cancel(leave.leaveId).subscribe({
        next: () => {
          this.snackBar.open('Leave cancelled', 'Close', { duration: 3000 });
          this.leaveService.getMyLeaves().subscribe(res => this.leaves = res.data);
        },
        error: (err) => this.snackBar.open(err.error?.message || 'Cancel failed', 'Close', { duration: 5000 })
      });
    }
  }
}
