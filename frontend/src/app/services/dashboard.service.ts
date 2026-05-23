import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../models/api-response';
import { DashboardStatsDto, AttendanceTrendDto, DepartmentDistributionDto } from '../models/dashboard';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/Dashboard`;

  getStats(): Observable<ApiResponse<DashboardStatsDto>> {
    return this.http.get<ApiResponse<DashboardStatsDto>>(`${this.baseUrl}/stats`);
  }

  getAttendanceTrend(days: number = 7): Observable<ApiResponse<AttendanceTrendDto[]>> {
    let p = new HttpParams().set('days', days);
    return this.http.get<ApiResponse<AttendanceTrendDto[]>>(`${this.baseUrl}/attendance-trend`, { params: p });
  }

  getDepartmentDistribution(): Observable<ApiResponse<DepartmentDistributionDto[]>> {
    return this.http.get<ApiResponse<DepartmentDistributionDto[]>>(`${this.baseUrl}/department-distribution`);
  }
}
