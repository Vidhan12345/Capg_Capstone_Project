import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../models/api-response';
import { DepartmentDto, CreateDepartmentDto, UpdateDepartmentDto } from '../models/department';

@Injectable({ providedIn: 'root' })
export class DepartmentService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/Departments`;

  getAll(): Observable<ApiResponse<DepartmentDto[]>> {
    return this.http.get<ApiResponse<DepartmentDto[]>>(this.baseUrl);
  }

  getById(id: number): Observable<ApiResponse<DepartmentDto>> {
    return this.http.get<ApiResponse<DepartmentDto>>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateDepartmentDto): Observable<ApiResponse<DepartmentDto>> {
    return this.http.post<ApiResponse<DepartmentDto>>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdateDepartmentDto): Observable<ApiResponse<DepartmentDto>> {
    return this.http.put<ApiResponse<DepartmentDto>>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<ApiResponse<DepartmentDto>> {
    return this.http.delete<ApiResponse<DepartmentDto>>(`${this.baseUrl}/${id}`);
  }
}
