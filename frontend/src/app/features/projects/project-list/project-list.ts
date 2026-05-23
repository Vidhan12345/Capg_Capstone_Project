import { Component, ViewChild, AfterViewInit, inject } from '@angular/core';
import { NgIf, NgClass, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ProjectService } from '../../../services/project.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [NgIf, NgClass, DatePipe, RouterLink, MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatIconModule, MatChipsModule, MatSnackBarModule],
  templateUrl: './project-list.html',
  styleUrl: './project-list.css'
})
export class ProjectListComponent implements AfterViewInit {
  private projectService = inject(ProjectService);
  private authService = inject(AuthService);
  private snackBar = inject(MatSnackBar);

  displayedColumns: string[] = ['name', 'clientName', 'status', 'startDate', 'endDate', 'actions'];
  dataSource = new MatTableDataSource<any>([]);
  totalCount = 0;
  loading = false;
  isManager = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor() {
    this.isManager = this.authService.hasRole('Admin', 'Manager');
  }

  ngAfterViewInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.loading = true;
    const page = this.paginator ? this.paginator.pageIndex + 1 : 1;
    const size = this.paginator ? this.paginator.pageSize : 10;
    const sortBy = this.sort?.active || 'name';

    this.projectService.getAll({ page, pageSize: size, sortBy, ascending: this.sort?.direction !== 'desc' }).subscribe({
      next: (res) => {
        this.dataSource.data = res.data.items;
        this.totalCount = res.data.totalCount;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  getStatusClass(status: string): string {
    const map: Record<string, string> = {
      'NotStarted': 'status-not-started',
      'InProgress': 'status-in-progress',
      'Completed': 'status-completed',
      'OnHold': 'status-on-hold'
    };
    return map[status] || '';
  }

  onDelete(id: number, name: string): void {
    if (confirm(`Delete project "${name}"?`)) {
      this.projectService.delete(id).subscribe({
        next: () => {
          this.snackBar.open('Project deleted', 'Close', { duration: 3000 });
          this.loadProjects();
        },
        error: (err) => this.snackBar.open(err.error?.message || 'Delete failed', 'Close', { duration: 5000 })
      });
    }
  }
}
