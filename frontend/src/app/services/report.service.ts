import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse, PagedResult } from '../models/api-response';
import { AttendanceReportDto, LeaveReportDto, EmployeeReportDto } from '../models/report';

@Injectable({ providedIn: 'root' })
export class ReportService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/Reports`;

  getAttendance(params: {
    page?: number; pageSize?: number; employeeId?: number;
    fromDate?: string; toDate?: string; department?: string; status?: string;
  }): Observable<ApiResponse<PagedResult<AttendanceReportDto>>> {
    let p = new HttpParams();
    if (params.page) p = p.set('page', params.page);
    if (params.pageSize) p = p.set('pageSize', params.pageSize);
    if (params.employeeId) p = p.set('employeeId', params.employeeId);
    if (params.fromDate) p = p.set('fromDate', params.fromDate);
    if (params.toDate) p = p.set('toDate', params.toDate);
    if (params.department) p = p.set('department', params.department);
    if (params.status) p = p.set('status', params.status);
    return this.http.get<ApiResponse<PagedResult<AttendanceReportDto>>>(`${this.baseUrl}/attendance`, { params: p });
  }

  getLeave(params: {
    page?: number; pageSize?: number; employeeId?: number;
    fromDate?: string; toDate?: string; status?: string;
  }): Observable<ApiResponse<PagedResult<LeaveReportDto>>> {
    let p = new HttpParams();
    if (params.page) p = p.set('page', params.page);
    if (params.pageSize) p = p.set('pageSize', params.pageSize);
    if (params.employeeId) p = p.set('employeeId', params.employeeId);
    if (params.fromDate) p = p.set('fromDate', params.fromDate);
    if (params.toDate) p = p.set('toDate', params.toDate);
    if (params.status) p = p.set('status', params.status);
    return this.http.get<ApiResponse<PagedResult<LeaveReportDto>>>(`${this.baseUrl}/leave`, { params: p });
  }

  getEmployee(params: {
    page?: number; pageSize?: number; departmentId?: number; isActive?: boolean;
  }): Observable<ApiResponse<PagedResult<EmployeeReportDto>>> {
    let p = new HttpParams();
    if (params.page) p = p.set('page', params.page);
    if (params.pageSize) p = p.set('pageSize', params.pageSize);
    if (params.departmentId) p = p.set('departmentId', params.departmentId);
    if (params.isActive !== undefined) p = p.set('isActive', params.isActive);
    return this.http.get<ApiResponse<PagedResult<EmployeeReportDto>>>(`${this.baseUrl}/employee`, { params: p });
  }
}
