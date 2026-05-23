import { Component, OnInit, inject } from '@angular/core';
import { NgIf, DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { EmployeeService } from '../../../services/employee.service';
import { EmployeeDto } from '../../../models/employee';

@Component({
  selector: 'app-employee-details',
  standalone: true,
  imports: [NgIf, DatePipe, RouterLink, MatCardModule, MatChipsModule, MatButtonModule, MatIconModule, MatDividerModule],
  templateUrl: './employee-details.html',
  styleUrl: './employee-details.css'
})
export class EmployeeDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private employeeService = inject(EmployeeService);
  employee: EmployeeDto | null = null;
  loading = false;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) this.loadEmployee(+id);
  }

  private loadEmployee(id: number): void {
    this.loading = true;
    this.employeeService.getById(id).subscribe({
      next: (res) => { this.employee = res.data; this.loading = false; },
      error: () => this.loading = false
    });
  }
}
