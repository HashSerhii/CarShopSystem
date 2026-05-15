import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FavoriteItem, PagedResult } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class FavoritesService {
  constructor(private readonly http: HttpClient) {}

  getAll(page = 1, pageSize = 20) {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get<PagedResult<FavoriteItem>>('/api/favorites', {
      params,
    });
  }

  add(carId: number) {
    return this.http.post('/api/favorites', { carId });
  }

  remove(carId: number) {
    return this.http.delete(`/api/favorites/${carId}`);
  }
}
