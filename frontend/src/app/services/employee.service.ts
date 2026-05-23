import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse, PagedResult, PaginationParams } from '../models/api-response';
import { EmployeeDto, CreateEmployeeDto, UpdateEmployeeDto } from '../models/employee';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/Employees`;

  getAll(params?: PaginationParams & { roleId?: number }): Observable<ApiResponse<PagedResult<EmployeeDto>>> {
    let p = new HttpParams();
    if (params) {
      if (params.page) p = p.set('page', params.page);
      if (params.pageSize) p = p.set('pageSize', params.pageSize);
      if (params.search) p = p.set('search', params.search);
      if (params.sortBy) p = p.set('sortBy', params.sortBy);
      if (params.ascending !== undefined) p = p.set('ascending', params.ascending);
      if (params.roleId) p = p.set('roleId', params.roleId);
    }
    return this.http.get<ApiResponse<PagedResult<EmployeeDto>>>(this.baseUrl, { params: p });
  }

  getById(id: number): Observable<ApiResponse<EmployeeDto>> {
    return this.http.get<ApiResponse<EmployeeDto>>(`${this.baseUrl}/${id}`);
  }

  getByDepartment(departmentId: number): Observable<ApiResponse<EmployeeDto[]>> {
    return this.http.get<ApiResponse<EmployeeDto[]>>(`${this.baseUrl}/department/${departmentId}`);
  }

  create(dto: CreateEmployeeDto): Observable<ApiResponse<EmployeeDto>> {
    return this.http.post<ApiResponse<EmployeeDto>>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdateEmployeeDto): Observable<ApiResponse<EmployeeDto>> {
    return this.http.put<ApiResponse<EmployeeDto>>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<ApiResponse<object>> {
    return this.http.delete<ApiResponse<object>>(`${this.baseUrl}/${id}`);
  }
}
