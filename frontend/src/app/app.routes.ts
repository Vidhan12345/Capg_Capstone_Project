import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { roleGuard } from './guards/role.guard';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./pages/login/login').then(m => m.LoginComponent) },
  { path: 'unauthorized', loadComponent: () => import('./pages/unauthorized/unauthorized').then(m => m.UnauthorizedComponent) },
  {
    path: '',
    loadComponent: () => import('./layout/main-layout/main-layout').then(m => m.MainLayoutComponent),
    canActivate: [authGuard],
    children: [
      { path: 'dashboard', loadComponent: () => import('./features/dashboard/dashboard-home/dashboard-home').then(m => m.DashboardHomeComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager', 'Employee'] } },
      { path: 'change-password', loadComponent: () => import('./pages/change-password/change-password').then(m => m.ChangePasswordComponent) },
      { path: 'employees', loadComponent: () => import('./features/employees/employee-list/employee-list').then(m => m.EmployeeListComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'employees/new', loadComponent: () => import('./features/employees/employee-form/employee-form').then(m => m.EmployeeFormComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'employees/:id/edit', loadComponent: () => import('./features/employees/employee-form/employee-form').then(m => m.EmployeeFormComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'employees/:id', loadComponent: () => import('./features/employees/employee-details/employee-details').then(m => m.EmployeeDetailsComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'departments', loadComponent: () => import('./features/departments/department-list/department-list').then(m => m.DepartmentListComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'departments/new', loadComponent: () => import('./features/departments/department-form/department-form').then(m => m.DepartmentFormComponent), canActivate: [roleGuard], data: { roles: ['Admin'] } },
      { path: 'departments/:id/edit', loadComponent: () => import('./features/departments/department-form/department-form').then(m => m.DepartmentFormComponent), canActivate: [roleGuard], data: { roles: ['Admin'] } },
      { path: 'projects', loadComponent: () => import('./features/projects/project-list/project-list').then(m => m.ProjectListComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'projects/new', loadComponent: () => import('./features/projects/project-form/project-form').then(m => m.ProjectFormComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'projects/:id', loadComponent: () => import('./features/projects/project-details/project-details').then(m => m.ProjectDetailsComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager', 'Employee'] } },
      { path: 'projects/:id/edit', loadComponent: () => import('./features/projects/project-form/project-form').then(m => m.ProjectFormComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'clients', loadComponent: () => import('./features/clients/client-list/client-list').then(m => m.ClientListComponent), canActivate: [roleGuard], data: { roles: ['Admin'] } },
      { path: 'clients/new', loadComponent: () => import('./features/clients/client-form/client-form').then(m => m.ClientFormComponent), canActivate: [roleGuard], data: { roles: ['Admin'] } },
      { path: 'clients/:id/edit', loadComponent: () => import('./features/clients/client-form/client-form').then(m => m.ClientFormComponent), canActivate: [roleGuard], data: { roles: ['Admin'] } },
      { path: 'attendance', loadComponent: () => import('./features/attendance/attendance-list/attendance-list').then(m => m.AttendanceListComponent) },
      { path: 'attendance/checkin', loadComponent: () => import('./features/attendance/checkin-checkout/checkin-checkout').then(m => m.CheckinCheckoutComponent) },
      { path: 'attendance/monthly', loadComponent: () => import('./features/attendance/monthly-view/monthly-view').then(m => m.MonthlyViewComponent) },
      { path: 'attendance/timesheet', loadComponent: () => import('./features/attendance/timesheet-report/timesheet-report').then(m => m.TimesheetReportComponent) },
      { path: 'leaves', loadComponent: () => import('./features/leaves/leave-list/leave-list').then(m => m.LeaveListComponent) },
      { path: 'leaves/apply', loadComponent: () => import('./features/leaves/apply-leave/apply-leave').then(m => m.ApplyLeaveComponent) },
      { path: 'leaves/approvals', loadComponent: () => import('./features/leaves/leave-approval/leave-approval').then(m => m.LeaveApprovalComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'my-projects', loadComponent: () => import('./features/projects/my-projects/my-projects').then(m => m.MyProjectsComponent) },
      { path: 'announcements', loadComponent: () => import('./features/announcements/announcement-list/announcement-list').then(m => m.AnnouncementListComponent) },
      { path: 'announcements/new', loadComponent: () => import('./features/announcements/announcement-form/announcement-form').then(m => m.AnnouncementFormComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'announcements/:id/edit', loadComponent: () => import('./features/announcements/announcement-form/announcement-form').then(m => m.AnnouncementFormComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'reports/attendance', loadComponent: () => import('./features/reports/attendance-report/attendance-report').then(m => m.AttendanceReportComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'reports/leave', loadComponent: () => import('./features/reports/leave-report/leave-report').then(m => m.LeaveReportComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'reports/employee', loadComponent: () => import('./features/reports/employee-report/employee-report').then(m => m.EmployeeReportComponent), canActivate: [roleGuard], data: { roles: ['Admin', 'Manager'] } },
      { path: 'audit-logs', loadComponent: () => import('./features/audit-logs/audit-log-list/audit-log-list').then(m => m.AuditLogListComponent), canActivate: [roleGuard], data: { roles: ['Admin'] } },
      { path: '', redirectTo: '/dashboard', pathMatch: 'full' }
    ]
  },
  { path: '**', loadComponent: () => import('./pages/not-found/not-found').then(m => m.NotFoundComponent) }
];
