import { Component, ViewChild, AfterViewInit, inject } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';
import { EmployeeService } from '../../../services/employee.service';
import { AuthService } from '../../../services/auth.service';
import { RoleService } from '../../../services/role.service';
import { EmployeeDto } from '../../../models/employee';
import { RoleDto } from '../../../models/role';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [NgIf, NgFor, RouterLink, FormsModule, MatTableModule, MatPaginatorModule, MatSortModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, MatIconModule, MatChipsModule, MatSnackBarModule],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.css'
})
export class EmployeeListComponent implements AfterViewInit {
  private employeeService = inject(EmployeeService);
  private authService = inject(AuthService);
  private roleService = inject(RoleService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);

  displayedColumns: string[] = ['employeeCode', 'fullName', 'email', 'departmentName', 'roleName', 'isActive', 'actions'];
  dataSource = new MatTableDataSource<EmployeeDto>([]);
  totalCount = 0;
  loading = false;
  searchTerm = '';
  private searchSubject = new Subject<string>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  isAdmin = false;
  pageSize = 10;
  roles: RoleDto[] = [];
  selectedRoleId = 0;

  constructor() {
    this.isAdmin = this.authService.hasRole('Admin');
    this.searchSubject.pipe(debounceTime(400), distinctUntilChanged()).subscribe(() => this.loadEmployees());
  }

  ngAfterViewInit(): void {
    this.loadEmployees();
    this.loadRoles();
  }

  private loadRoles(): void {
    this.roleService.getAll().subscribe(res => this.roles = res.data);
  }

  onRoleFilterChange(): void {
    this.paginator.firstPage();
    this.loadEmployees();
  }

  onSearch(term: string): void {
    this.searchSubject.next(term);
  }

  loadEmployees(): void {
    this.loading = true;
    const page = this.paginator ? this.paginator.pageIndex + 1 : 1;
    const size = this.paginator ? this.paginator.pageSize : this.pageSize;
    const sortBy = this.sort?.active || 'firstName';
    const ascending = this.sort?.direction !== 'desc';

    const roleFilter = this.selectedRoleId ? { roleId: this.selectedRoleId } : {};
    this.employeeService.getAll({ page, pageSize: size, search: this.searchTerm, sortBy, ascending, ...roleFilter }).subscribe({
      next: (res) => {
        this.dataSource.data = res.data.items;
        this.totalCount = res.data.totalCount;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  onDelete(id: number, name: string): void {
    if (confirm(`Delete employee ${name}?`)) {
      this.employeeService.delete(id).subscribe({
        next: () => {
          this.snackBar.open('Employee deleted', 'Close', { duration: 3000 });
          this.loadEmployees();
        },
        error: (err) => this.snackBar.open(err.error?.message || 'Delete failed', 'Close', { duration: 5000 })
      });
    }
  }
}
