import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
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
    if (query.mileageFrom != null)
      params = params.set('mileageFrom', query.mileageFrom);
    if (query.mileageTo != null) params = params.set('mileageTo', query.mileageTo);
    if (query.model) params = params.set('model', query.model);
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

  getPending(page = 1, pageSize = 20) {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get<PagedResult<CarListItem>>('/api/cars/pending', {
      params,
    });
  }

  getById(id: number) {
    return this.http.get<CarDetail>(`/api/cars/${id}`);
  }

  create(payload: CreateCarPayload) {
    return this.http
      .post<{ id: number } | number>('/api/cars', payload)
      .pipe(map((res) => (typeof res === 'number' ? res : res.id)));
  }

  approve(id: number) {
    return this.http.post(`/api/cars/${id}/approve`, null);
  }

  reject(id: number) {
    return this.http.post(`/api/cars/${id}/reject`, null);
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
