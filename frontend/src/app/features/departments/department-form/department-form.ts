import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { DepartmentService } from '../../../services/department.service';

@Component({
  selector: 'app-department-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatSnackBarModule],
  templateUrl: './department-form.html',
  styleUrl: './department-form.css'
})
export class DepartmentFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private departmentService = inject(DepartmentService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  isEditMode = false;
  departmentId: number | null = null;
  loading = false;

  form = this.fb.nonNullable.group({
    name: ['', Validators.required],
    description: ['']
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.departmentId = +id;
      this.loadDepartment(+id);
    }
  }

  private loadDepartment(id: number): void {
    this.departmentService.getById(id).subscribe(res => {
      this.form.patchValue({
        name: res.data.name,
        description: res.data.description || ''
      });
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading = true;
    const dto = { name: this.form.value.name!, description: this.form.value.description || undefined };

    if (this.isEditMode && this.departmentId) {
      this.departmentService.update(this.departmentId, dto).subscribe({
        next: () => {
          this.snackBar.open('Department updated', 'Close', { duration: 3000 });
          this.router.navigate(['/departments']);
        },
        error: (err) => {
          this.loading = false;
          this.snackBar.open(err.error?.message || 'Update failed', 'Close', { duration: 5000 });
        }
      });
    } else {
      this.departmentService.create(dto).subscribe({
        next: () => {
          this.snackBar.open('Department created', 'Close', { duration: 3000 });
          this.router.navigate(['/departments']);
        },
        error: (err) => {
          this.loading = false;
          this.snackBar.open(err.error?.message || 'Creation failed', 'Close', { duration: 5000 });
        }
      });
    }
  }
}
