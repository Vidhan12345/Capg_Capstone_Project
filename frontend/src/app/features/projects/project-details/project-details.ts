import { Component, OnInit, inject } from '@angular/core';
import { NgFor, NgIf, DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDividerModule } from '@angular/material/divider';
import { ProjectService } from '../../../services/project.service';
import { EmployeeService } from '../../../services/employee.service';
import { AuthService } from '../../../services/auth.service';
import { ProjectDto, AllocationDto } from '../../../models/project';

@Component({
  selector: 'app-project-details',
  standalone: true,
  imports: [NgFor, NgIf, DatePipe, ReactiveFormsModule, RouterLink, MatCardModule, MatTableModule, MatButtonModule, MatIconModule, MatSelectModule, MatFormFieldModule, MatInputModule, MatSnackBarModule, MatDividerModule],
  templateUrl: './project-details.html',
  styleUrl: './project-details.css'
})
export class ProjectDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private projectService = inject(ProjectService);
  private employeeService = inject(EmployeeService);
  private authService = inject(AuthService);
  private fb = inject(FormBuilder);
  private snackBar = inject(MatSnackBar);

  project: ProjectDto | null = null;
  allocations: AllocationDto[] = [];
  employees: any[] = [];
  displayedColumns = ['employeeName', 'roleOnProject', 'allocatedAt', 'actions'];
  isManager = false;
  showAllocate = false;

  allocateForm = this.fb.nonNullable.group({
    employeeId: [0, [Validators.required, Validators.min(1)]],
    roleOnProject: ['']
  });

  ngOnInit(): void {
    this.isManager = this.authService.hasRole('Admin', 'Manager');
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadProject(+id);
      this.loadAllocations(+id);
      this.employeeService.getAll({ pageSize: 100 }).subscribe(res => this.employees = res.data.items);
    }
  }

  private loadProject(id: number): void {
    this.projectService.getById(id).subscribe(res => this.project = res.data);
  }

  private loadAllocations(id: number): void {
    this.projectService.getAllocations(id).subscribe(res => this.allocations = res.data);
  }

  onAllocate(): void {
    if (this.allocateForm.invalid || !this.project) return;
    this.projectService.allocate({
      ...this.allocateForm.getRawValue(),
      projectId: this.project.projectId
    }).subscribe({
      next: () => {
        this.snackBar.open('Employee allocated', 'Close', { duration: 3000 });
        this.loadAllocations(this.project!.projectId);
        this.showAllocate = false;
        this.allocateForm.reset({ employeeId: 0, roleOnProject: '' });
      },
      error: (err) => this.snackBar.open(err.error?.message || 'Allocation failed', 'Close', { duration: 5000 })
    });
  }

  onRelease(allocationId: number): void {
    if (confirm('Release this employee from the project?')) {
      this.projectService.release(allocationId).subscribe({
        next: () => {
          this.snackBar.open('Employee released', 'Close', { duration: 3000 });
          if (this.project) this.loadAllocations(this.project.projectId);
        },
        error: (err) => this.snackBar.open(err.error?.message || 'Release failed', 'Close', { duration: 5000 })
      });
    }
  }
}
