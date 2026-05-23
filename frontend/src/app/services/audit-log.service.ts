import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse, PagedResult } from '../models/api-response';
import { AuditLogDto } from '../models/audit-log';

@Injectable({ providedIn: 'root' })
export class AuditLogService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/AuditLogs`;

  getAll(params: {
    page?: number; pageSize?: number; entityName?: string;
    recordId?: number; action?: string; createdBy?: string;
    fromDate?: string; toDate?: string;
  }): Observable<ApiResponse<PagedResult<AuditLogDto>>> {
    let p = new HttpParams();
    if (params.page) p = p.set('page', params.page);
    if (params.pageSize) p = p.set('pageSize', params.pageSize);
    if (params.entityName) p = p.set('entityName', params.entityName);
    if (params.recordId) p = p.set('recordId', params.recordId);
    if (params.action) p = p.set('action', params.action);
    if (params.createdBy) p = p.set('createdBy', params.createdBy);
    if (params.fromDate) p = p.set('fromDate', params.fromDate);
    if (params.toDate) p = p.set('toDate', params.toDate);
    return this.http.get<ApiResponse<PagedResult<AuditLogDto>>>(this.baseUrl, { params: p });
  }

  getByEntity(entityName: string, recordId: number): Observable<ApiResponse<AuditLogDto[]>> {
    return this.http.get<ApiResponse<AuditLogDto[]>>(`${this.baseUrl}/entity/${entityName}/${recordId}`);
  }

  getByUser(username: string): Observable<ApiResponse<AuditLogDto[]>> {
    return this.http.get<ApiResponse<AuditLogDto[]>>(`${this.baseUrl}/user/${username}`);
  }
}
