import { Component, Output, EventEmitter, inject } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../services/auth.service';

interface NavItem {
  label: string;
  icon: string;
  route: string;
  roles: string[];
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [NgFor, NgIf, RouterLink, RouterLinkActive, MatListModule, MatIconModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css'
})
export class SidebarComponent {
  private authService = inject(AuthService);

  navItems: NavItem[] = [
    { label: 'Dashboard', icon: 'dashboard', route: '/dashboard', roles: ['Admin'] },
    { label: 'Employees', icon: 'people', route: '/employees', roles: ['Admin', 'Manager'] },
    { label: 'Departments', icon: 'business', route: '/departments', roles: ['Admin', 'Manager'] },
    { label: 'My Projects', icon: 'assignment_ind', route: '/my-projects', roles: ['Admin', 'Manager', 'Employee'] },
    { label: 'Projects', icon: 'assignment', route: '/projects', roles: ['Admin', 'Manager'] },
    { label: 'Clients', icon: 'contacts', route: '/clients', roles: ['Admin'] },
    { label: 'Attendance', icon: 'calendar_today', route: '/attendance', roles: ['Admin', 'Manager', 'Employee'] },
    { label: 'Check In/Out', icon: 'login', route: '/attendance/checkin', roles: ['Admin', 'Manager', 'Employee'] },
    { label: 'Leaves', icon: 'flight_takeoff', route: '/leaves', roles: ['Admin', 'Manager', 'Employee'] },
    { label: 'Leave Approvals', icon: 'fact_check', route: '/leaves/approvals', roles: ['Admin', 'Manager'] },
    { label: 'Announcements', icon: 'campaign', route: '/announcements', roles: ['Admin', 'Manager', 'Employee'] },
    { label: 'Reports', icon: 'bar_chart', route: '/reports/attendance', roles: ['Admin', 'Manager'] },
    { label: 'Audit Logs', icon: 'history', route: '/audit-logs', roles: ['Admin'] },
  ];

  get visibleItems(): NavItem[] {
    return this.navItems.filter(item => this.authService.hasRole(...item.roles));
  }
}
