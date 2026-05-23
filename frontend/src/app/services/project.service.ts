import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse, PagedResult, PaginationParams } from '../models/api-response';
import { ProjectDto, CreateProjectDto, UpdateProjectDto, AllocationDto, CreateAllocationDto } from '../models/project';

@Injectable({ providedIn: 'root' })
export class ProjectService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/Projects`;

  getAll(params?: PaginationParams): Observable<ApiResponse<PagedResult<ProjectDto>>> {
    let p = new HttpParams();
    if (params) {
      if (params.page) p = p.set('page', params.page);
      if (params.pageSize) p = p.set('pageSize', params.pageSize);
      if (params.search) p = p.set('search', params.search);
      if (params.sortBy) p = p.set('sortBy', params.sortBy);
      if (params.ascending !== undefined) p = p.set('ascending', params.ascending);
    }
    return this.http.get<ApiResponse<PagedResult<ProjectDto>>>(this.baseUrl, { params: p });
  }

  getById(id: number): Observable<ApiResponse<ProjectDto>> {
    return this.http.get<ApiResponse<ProjectDto>>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateProjectDto): Observable<ApiResponse<ProjectDto>> {
    return this.http.post<ApiResponse<ProjectDto>>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdateProjectDto): Observable<ApiResponse<ProjectDto>> {
    return this.http.put<ApiResponse<ProjectDto>>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<ApiResponse<object>> {
    return this.http.delete<ApiResponse<object>>(`${this.baseUrl}/${id}`);
  }

  getAllocations(projectId: number): Observable<ApiResponse<AllocationDto[]>> {
    return this.http.get<ApiResponse<AllocationDto[]>>(`${this.baseUrl}/${projectId}/allocations`);
  }

  allocate(dto: CreateAllocationDto): Observable<ApiResponse<AllocationDto>> {
    return this.http.post<ApiResponse<AllocationDto>>(`${environment.apiUrl}/Allocations`, dto);
  }

  release(allocationId: number): Observable<ApiResponse<object>> {
    return this.http.put<ApiResponse<object>>(`${environment.apiUrl}/Allocations/${allocationId}/release`, {});
  }

  getMyAllocations(): Observable<ApiResponse<AllocationDto[]>> {
    return this.http.get<ApiResponse<AllocationDto[]>>(`${environment.apiUrl}/Allocations/my`);
  }
}
