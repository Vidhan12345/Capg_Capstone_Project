import { Component, OnInit, inject } from '@angular/core';
import { NgFor } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ClientService } from '../../../services/client.service';

@Component({
  selector: 'app-client-form',
  standalone: true,
  imports: [NgFor, ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, MatSnackBarModule],
  templateUrl: './client-form.html',
  styleUrl: './client-form.css'
})
export class ClientFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private clientService = inject(ClientService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  isEditMode = false;
  clientId: number | null = null;
  loading = false;

  form = this.fb.nonNullable.group({
    clientName: ['', Validators.required],
    clientAddress: [''],
    clientPhoneNumber: [''],
    clientLocation: [''],
    status: [true]
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.clientId = +id;
      this.clientService.getById(+id).subscribe(res => {
        const c = res.data;
        this.form.patchValue({
          clientName: c.clientName,
          clientAddress: c.clientAddress || '',
          clientPhoneNumber: c.clientPhoneNumber || '',
          clientLocation: c.clientLocation || '',
          status: c.status
        });
      });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading = true;
    const raw = this.form.getRawValue();
    const dto: any = {};
    for (const key of Object.keys(raw)) {
      if ((raw as any)[key]) (dto as any)[key] = (raw as any)[key];
    }

    if (this.isEditMode && this.clientId) {
      this.clientService.update(this.clientId, dto).subscribe({
        next: () => {
          this.snackBar.open('Client updated', 'Close', { duration: 3000 });
          this.router.navigate(['/clients']);
        },
        error: (err) => {
          this.loading = false;
          this.snackBar.open(err.error?.message || 'Update failed', 'Close', { duration: 5000 });
        }
      });
    } else {
      this.clientService.create(dto).subscribe({
        next: () => {
          this.snackBar.open('Client created', 'Close', { duration: 3000 });
          this.router.navigate(['/clients']);
        },
        error: (err) => {
          this.loading = false;
          this.snackBar.open(err.error?.message || 'Creation failed', 'Close', { duration: 5000 });
        }
      });
    }
  }
}
