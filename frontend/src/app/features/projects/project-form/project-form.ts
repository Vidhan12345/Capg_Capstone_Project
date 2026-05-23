import { Component, OnInit, inject } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ProjectService } from '../../../services/project.service';
import { ClientService } from '../../../services/client.service';
import { ClientDto } from '../../../models/client';

@Component({
  selector: 'app-project-form',
  standalone: true,
  imports: [NgFor, NgIf, ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatDatepickerModule, MatNativeDateModule, MatButtonModule, MatSnackBarModule],
  templateUrl: './project-form.html',
  styleUrl: './project-form.css'
})
export class ProjectFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private projectService = inject(ProjectService);
  private clientService = inject(ClientService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  isEditMode = false;
  projectId: number | null = null;
  loading = false;
  clients: ClientDto[] = [];

  statuses = ['NotStarted', 'InProgress', 'Completed', 'OnHold'];

  form = this.fb.nonNullable.group({
    name: ['', Validators.required],
    description: [''],
    startDate: ['', Validators.required],
    endDate: [''],
    clientId: [0, [Validators.required, Validators.min(1)]],
    status: ['NotStarted']
  });

  ngOnInit(): void {
    this.loadClients();
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.projectId = +id;
      this.loadProject(+id);
    }
  }

  private loadClients(): void {
    this.clientService.getAll().subscribe(res => this.clients = res.data);
  }

  private loadProject(id: number): void {
    this.projectService.getById(id).subscribe(res => {
      const p = res.data;
      this.form.patchValue({
        name: p.name,
        description: p.description || '',
        startDate: p.startDate.split('T')[0],
        endDate: p.endDate?.split('T')[0] || '',
        clientId: p.clientId,
        status: p.status
      });
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading = true;
    const raw = this.form.getRawValue();
    const dto: any = { ...raw };
    if (!dto.endDate) dto.endDate = undefined;
    if (!dto.description) dto.description = undefined;

    if (this.isEditMode && this.projectId) {
      this.projectService.update(this.projectId, dto).subscribe({
        next: () => {
          this.snackBar.open('Project updated', 'Close', { duration: 3000 });
          this.router.navigate(['/projects']);
        },
        error: (err) => {
          this.loading = false;
          this.snackBar.open(err.error?.message || 'Update failed', 'Close', { duration: 5000 });
        }
      });
    } else {
      this.projectService.create(dto).subscribe({
        next: () => {
          this.snackBar.open('Project created', 'Close', { duration: 3000 });
          this.router.navigate(['/projects']);
        },
        error: (err) => {
          this.loading = false;
          this.snackBar.open(err.error?.message || 'Creation failed', 'Close', { duration: 5000 });
        }
      });
    }
  }
}
