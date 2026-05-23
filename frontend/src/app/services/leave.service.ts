import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../models/api-response';
import { LeaveDto, ApplyLeaveDto } from '../models/leave';

@Injectable({ providedIn: 'root' })
export class LeaveService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/Leaves`;

  getMyLeaves(): Observable<ApiResponse<LeaveDto[]>> {
    return this.http.get<ApiResponse<LeaveDto[]>>(`${this.baseUrl}/my`);
  }

  apply(dto: ApplyLeaveDto): Observable<ApiResponse<LeaveDto>> {
    return this.http.post<ApiResponse<LeaveDto>>(`${this.baseUrl}/apply`, dto);
  }

  getPending(): Observable<ApiResponse<LeaveDto[]>> {
    return this.http.get<ApiResponse<LeaveDto[]>>(`${this.baseUrl}/pending`);
  }

  approve(id: number): Observable<ApiResponse<LeaveDto>> {
    return this.http.put<ApiResponse<LeaveDto>>(`${this.baseUrl}/${id}/approve`, {});
  }

  reject(id: number): Observable<ApiResponse<LeaveDto>> {
    return this.http.put<ApiResponse<LeaveDto>>(`${this.baseUrl}/${id}/reject`, {});
  }

  cancel(id: number): Observable<ApiResponse<LeaveDto>> {
    return this.http.put<ApiResponse<LeaveDto>>(`${this.baseUrl}/${id}/cancel`, {});
  }
}
