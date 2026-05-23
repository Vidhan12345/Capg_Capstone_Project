import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../models/api-response';
import { ClientDto, CreateClientDto, UpdateClientDto } from '../models/client';

@Injectable({ providedIn: 'root' })
export class ClientService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/Clients`;

  getAll(): Observable<ApiResponse<ClientDto[]>> {
    return this.http.get<ApiResponse<ClientDto[]>>(this.baseUrl);
  }

  getById(id: number): Observable<ApiResponse<ClientDto>> {
    return this.http.get<ApiResponse<ClientDto>>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateClientDto): Observable<ApiResponse<ClientDto>> {
    return this.http.post<ApiResponse<ClientDto>>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdateClientDto): Observable<ApiResponse<ClientDto>> {
    return this.http.put<ApiResponse<ClientDto>>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<ApiResponse<object>> {
    return this.http.delete<ApiResponse<object>>(`${this.baseUrl}/${id}`);
  }
}
