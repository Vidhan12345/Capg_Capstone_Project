import { Component, OnInit, inject } from '@angular/core';
import { NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { DepartmentService } from '../../../services/department.service';
import { EmployeeService } from '../../../services/employee.service';
import { AuthService } from '../../../services/auth.service';
import { DepartmentDto } from '../../../models/department';

@Component({
  selector: 'app-department-list',
  standalone: true,
  imports: [NgIf, RouterLink, MatTableModule, MatButtonModule, MatIconModule, MatSnackBarModule],
  templateUrl: './department-list.html',
  styleUrl: './department-list.css'
})
export class DepartmentListComponent implements OnInit {
  private departmentService = inject(DepartmentService);
  private employeeService = inject(EmployeeService);
  private authService = inject(AuthService);
  private snackBar = inject(MatSnackBar);

  departments: DepartmentDto[] = [];
  displayedColumns: string[] = ['name', 'description', 'employeeCount'];
  isAdmin = false;

  ngOnInit(): void {
    this.isAdmin = this.authService.hasRole('Admin');
    if (this.isAdmin) this.displayedColumns.push('actions');
    this.loadDepartments();
  }

  loadDepartments(): void {
    forkJoin([
      this.departmentService.getAll(),
      this.employeeService.getAll({ page: 1, pageSize: 10000 })
    ]).pipe(
      map(([deptRes, empRes]) => {
        const countMap = new Map<number, number>();
        (empRes.data?.items ?? []).forEach(emp => {
          const id = emp.departmentId;
          countMap.set(id, (countMap.get(id) ?? 0) + 1);
        });
        return (deptRes.data ?? []).map(dept => ({
          ...dept,
          employeeCount: countMap.get(dept.departmentId) ?? 0
        }));
      })
    ).subscribe(depts => this.departments = depts);
  }

  onDelete(id: number, name: string): void {
    if (confirm(`Delete department "${name}"? This will fail if it has active employees.`)) {
      this.departmentService.delete(id).subscribe({
        next: () => {
          this.snackBar.open('Department deleted', 'Close', { duration: 3000 });
          this.loadDepartments();
        },
        error: (err) => this.snackBar.open(err.error?.message || 'Delete failed', 'Close', { duration: 5000 })
      });
    }
  }
}
