import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import {
  CarDetail,
  CarListItem,
  CarsQuery,
  CreateCarPayload,
  PagedResult,
} from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class CarsService {
  constructor(private readonly http: HttpClient) {}

  getCars(query: CarsQuery = {}) {
    let params = new HttpParams();
    if (query.brandId != null) params = params.set('brandId', query.brandId);
    if (query.yearFrom != null) params = params.set('yearFrom', query.yearFrom);
    if (query.yearTo != null) params = params.set('yearTo', query.yearTo);
    if (query.priceFrom != null)
      params = params.set('priceFrom', query.priceFrom);
    if (query.priceTo != null) params = params.set('priceTo', query.priceTo);
    if (query.page != null) params = params.set('page', query.page);
    if (query.pageSize != null) params = params.set('pageSize', query.pageSize);
    if (query.sort) params = params.set('sort', query.sort);

    return this.http.get<PagedResult<CarListItem>>('/api/cars', { params });
  }

  getMyCars(page = 1, pageSize = 20) {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get<PagedResult<CarListItem>>('/api/cars/mine', { params });
  }

  getById(id: number) {
    return this.http.get<CarDetail>(`/api/cars/${id}`);
  }

  create(payload: CreateCarPayload) {
    return this.http.post<number>('/api/cars', payload);
  }

  delete(id: number) {
    return this.http.delete(`/api/cars/${id}`);
  }

  uploadPhotos(carId: number, files: File[]) {
    const form = new FormData();
    for (const file of files) {
      form.append('files', file, file.name);
    }
    return this.http.post(`/api/cars/${carId}/photos`, form);
  }
}
