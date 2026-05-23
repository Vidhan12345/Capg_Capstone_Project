import { Component, inject } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { LeaveService } from '../../../services/leave.service';
import { LEAVE_TYPES } from '../../../models/leave';

@Component({
  selector: 'app-apply-leave',
  standalone: true,
  imports: [NgFor, NgIf, ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatDatepickerModule, MatNativeDateModule, MatButtonModule, MatSnackBarModule],
  templateUrl: './apply-leave.html',
  styleUrl: './apply-leave.css'
})
export class ApplyLeaveComponent {
  private fb = inject(FormBuilder);
  private leaveService = inject(LeaveService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  loading = false;
  leaveTypes = [...LEAVE_TYPES];

  form = this.fb.nonNullable.group({
    fromDate: ['', Validators.required],
    toDate: ['', Validators.required],
    leaveType: ['', Validators.required],
    reason: ['', Validators.required]
  });

  private toDateString(d: any): string {
    if (!d) return '';
    const date = new Date(d);
    return date.toISOString().split('T')[0];
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    const raw = this.form.getRawValue();
    if (raw.fromDate > raw.toDate) {
      this.snackBar.open('To date must be after from date', 'Close', { duration: 4000 });
      return;
    }
    this.loading = true;
    this.leaveService.apply({
      fromDate: this.toDateString(raw.fromDate),
      toDate: this.toDateString(raw.toDate),
      leaveType: raw.leaveType,
      reason: raw.reason
    }).subscribe({
      next: () => {
        this.snackBar.open('Leave applied successfully', 'Close', { duration: 3000 });
        this.router.navigate(['/leaves']);
      },
      error: (err) => {
        this.loading = false;
        this.snackBar.open(err.error?.message || 'Failed to apply leave', 'Close', { duration: 5000 });
      }
    });
  }
}
