import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../models/api-response';
import { AttendanceDto, CheckInRequest, CheckOutRequest } from '../models/attendance';

@Injectable({ providedIn: 'root' })
export class AttendanceService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/Attendance`;

  checkIn(dto: CheckInRequest): Observable<ApiResponse<AttendanceDto>> {
    return this.http.post<ApiResponse<AttendanceDto>>(`${this.baseUrl}/checkin`, dto);
  }

  checkOut(dto: CheckOutRequest): Observable<ApiResponse<AttendanceDto>> {
    return this.http.post<ApiResponse<AttendanceDto>>(`${this.baseUrl}/checkout`, dto);
  }

  getMyAttendance(): Observable<ApiResponse<AttendanceDto[]>> {
    return this.http.get<ApiResponse<AttendanceDto[]>>(`${this.baseUrl}/my`);
  }

  getByDate(date: string): Observable<ApiResponse<AttendanceDto[]>> {
    let p = new HttpParams().set('date', date);
    return this.http.get<ApiResponse<AttendanceDto[]>>(`${this.baseUrl}/date`, { params: p });
  }
}
