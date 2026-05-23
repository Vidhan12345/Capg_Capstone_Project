import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AnnouncementService } from '../../../services/announcement.service';

@Component({
  selector: 'app-announcement-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule, MatDatepickerModule, MatNativeDateModule, MatButtonModule, MatSnackBarModule],
  templateUrl: './announcement-form.html',
  styleUrl: './announcement-form.css'
})
export class AnnouncementFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private announcementService = inject(AnnouncementService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  isEditMode = false;
  announcementId: number | null = null;
  loading = false;

  form = this.fb.nonNullable.group({
    title: ['', Validators.required],
    content: ['', Validators.required],
    expiresAt: ['']
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.announcementId = +id;
      this.announcementService.getById(+id).subscribe(res => {
        this.form.patchValue({
          title: res.data.title,
          content: res.data.content,
          expiresAt: res.data.expiresAt?.split('T')[0] || ''
        });
      });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading = true;
    const raw = this.form.getRawValue();
    const dto: any = { title: raw.title, content: raw.content };
    if (raw.expiresAt) dto.expiresAt = raw.expiresAt + 'T00:00:00Z';

    const action = this.isEditMode && this.announcementId
      ? this.announcementService.update(this.announcementId, dto)
      : this.announcementService.create(dto);

    action.subscribe({
      next: () => {
        this.snackBar.open(this.isEditMode ? 'Updated' : 'Created', 'Close', { duration: 3000 });
        this.router.navigate(['/announcements']);
      },
      error: (err) => {
        this.loading = false;
        this.snackBar.open(err.error?.message || 'Failed', 'Close', { duration: 5000 });
      }
    });
  }
}
