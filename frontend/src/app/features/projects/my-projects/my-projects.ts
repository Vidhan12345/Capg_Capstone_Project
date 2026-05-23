import { Component, OnInit, inject } from '@angular/core';
import { NgIf, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { ProjectService } from '../../../services/project.service';
import { AllocationDto } from '../../../models/project';

@Component({
  selector: 'app-my-projects',
  standalone: true,
  imports: [NgIf, DatePipe, RouterLink, MatTableModule, MatButtonModule, MatIconModule, MatCardModule],
  templateUrl: './my-projects.html',
  styleUrl: './my-projects.css'
})
export class MyProjectsComponent implements OnInit {
  private projectService = inject(ProjectService);

  allocations: AllocationDto[] = [];
  displayedColumns = ['projectName', 'roleOnProject', 'allocatedAt', 'actions'];
  loading = false;

  ngOnInit(): void {
    this.loadMyAllocations();
  }

  loadMyAllocations(): void {
    this.loading = true;
    this.projectService.getMyAllocations().subscribe({
      next: (res) => { this.allocations = res.data; this.loading = false; },
      error: () => this.loading = false
    });
  }
}