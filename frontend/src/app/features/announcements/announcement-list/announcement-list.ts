import { Component, OnInit, inject } from '@angular/core';
import { NgIf, NgFor, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AnnouncementService } from '../../../services/announcement.service';
import { AuthService } from '../../../services/auth.service';
import { AnnouncementDto } from '../../../models/announcement';

@Component({
  selector: 'app-announcement-list',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, RouterLink, MatCardModule, MatButtonModule, MatIconModule, MatSnackBarModule],
  templateUrl: './announcement-list.html',
  styleUrl: './announcement-list.css'
})
export class AnnouncementListComponent implements OnInit {
  private announcementService = inject(AnnouncementService);
  private authService = inject(AuthService);
  private snackBar = inject(MatSnackBar);

  announcements: AnnouncementDto[] = [];
  canManage = false;

  ngOnInit(): void {
    this.canManage = this.authService.hasRole('Admin', 'Manager');
    this.load();
  }

  load(): void {
    this.announcementService.getAll().subscribe(res => this.announcements = res.data);
  }

  onDelete(id: number): void {
    if (confirm('Delete this announcement?')) {
      this.announcementService.delete(id).subscribe({
        next: () => { this.snackBar.open('Deleted', 'Close', { duration: 3000 }); this.load(); },
        error: (err) => this.snackBar.open(err.error?.message || 'Delete failed', 'Close', { duration: 5000 })
      });
    }
  }
}
