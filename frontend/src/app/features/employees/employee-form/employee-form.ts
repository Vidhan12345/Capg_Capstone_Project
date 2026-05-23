import { Component, OnInit, inject } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { EmployeeService } from '../../../services/employee.service';
import { DepartmentService } from '../../../services/department.service';
import { RoleService } from '../../../services/role.service';
import { DepartmentDto } from '../../../models/department';
import { RoleDto } from '../../../models/role';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [NgIf, NgFor, ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatDatepickerModule, MatNativeDateModule, MatButtonModule, MatIconModule, MatSnackBarModule],
  templateUrl: './employee-form.html',
  styleUrl: './employee-form.css'
})
export class EmployeeFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private employeeService = inject(EmployeeService);
  private departmentService = inject(DepartmentService);
  private roleService = inject(RoleService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  isEditMode = false;
  employeeId: number | null = null;
  loading = false;
  departments: DepartmentDto[] = [];
  roles: RoleDto[] = [];

  form = this.fb.nonNullable.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: ['', Validators.pattern(/^\d{10}$/)],
    dateOfBirth: ['', [this.minAgeValidator(18)]],
    dateOfJoining: [''],
    gender: [''],
    address: [''],
    emergencyContact: [''],
    departmentId: [0, [Validators.required, Validators.min(1)]],
    roleId: [0, [Validators.required, Validators.min(1)]]
  });

  private minAgeValidator(minAge: number) {
    return (control: { value: string }) => {
      if (!control.value) return null;
      const dob = new Date(control.value);
      const today = new Date();
      const age = today.getFullYear() - dob.getFullYear();
      const monthDiff = today.getMonth() - dob.getMonth();
      if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < dob.getDate())) {
        if (age - 1 < minAge) return { minAge: { required: minAge, actual: age - 1 } };
      }
      if (age < minAge) return { minAge: { required: minAge, actual: age } };
      return null;
    };
  }

  ngOnInit(): void {
    this.loadDepartments();
    this.loadRoles();
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.employeeId = +id;
      this.loadEmployee(+id);
    }
  }

  private loadDepartments(): void {
    this.departmentService.getAll().subscribe(res => this.departments = res.data);
  }

  private loadRoles(): void {
    this.roleService.getAll().subscribe(res => this.roles = res.data);
  }

  private loadEmployee(id: number): void {
    this.loading = true;
    this.employeeService.getById(id).subscribe({
      next: (res) => {
        const e = res.data;
        this.form.patchValue({
          firstName: e.firstName,
          lastName: e.lastName,
          email: e.email,
          phoneNumber: e.phoneNumber || '',
          dateOfBirth: e.dateOfBirth?.split('T')[0] || '',
          dateOfJoining: e.dateOfJoining?.split('T')[0] || '',
          gender: e.gender || '',
          address: e.address || '',
          emergencyContact: e.emergencyContact || '',
          departmentId: e.departmentId,
          roleId: e.roleId
        });
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading = true;
    const dto = this.form.getRawValue();

    if (this.isEditMode && this.employeeId) {
      this.employeeService.update(this.employeeId, dto).subscribe({
        next: () => {
          this.snackBar.open('Employee updated', 'Close', { duration: 3000 });
          this.router.navigate(['/employees']);
        },
        error: (err) => {
          this.loading = false;
          this.snackBar.open(err.error?.message || 'Update failed', 'Close', { duration: 5000 });
        }
      });
    } else {
      this.employeeService.create(dto).subscribe({
        next: () => {
          this.snackBar.open('Employee created', 'Close', { duration: 3000 });
          this.router.navigate(['/employees']);
        },
        error: (err) => {
          this.loading = false;
          this.snackBar.open(err.error?.message || 'Creation failed', 'Close', { duration: 5000 });
        }
      });
    }
  }
}
