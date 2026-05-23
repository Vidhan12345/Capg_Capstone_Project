import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../models/api-response';
import { AnnouncementDto, CreateAnnouncementDto, UpdateAnnouncementDto } from '../models/announcement';

@Injectable({ providedIn: 'root' })
export class AnnouncementService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/Announcements`;

  getAll(): Observable<ApiResponse<AnnouncementDto[]>> {
    return this.http.get<ApiResponse<AnnouncementDto[]>>(this.baseUrl);
  }

  getById(id: number): Observable<ApiResponse<AnnouncementDto>> {
    return this.http.get<ApiResponse<AnnouncementDto>>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateAnnouncementDto): Observable<ApiResponse<AnnouncementDto>> {
    return this.http.post<ApiResponse<AnnouncementDto>>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdateAnnouncementDto): Observable<ApiResponse<AnnouncementDto>> {
    return this.http.put<ApiResponse<AnnouncementDto>>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<ApiResponse<object>> {
    return this.http.delete<ApiResponse<object>>(`${this.baseUrl}/${id}`);
  }
}
