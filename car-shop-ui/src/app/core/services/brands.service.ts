import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Brand } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class BrandsService {
  constructor(private readonly http: HttpClient) {}

  getAll() {
    return this.http.get<Brand[]>('/api/brands');
  }

  create(name: string) {
    return this.http.post<Brand>('/api/brands', { name });
  }
}
