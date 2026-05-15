export interface AuthResponse {
  token: string;
  expiration: string;
}

export interface Brand {
  id: number;
  name: string;
}

export interface CarListItem {
  id: number;
  brand: string;
  model: string;
  year: number;
  price: number;
  primaryPhotoUrl: string | null;
}

export interface CarDetail {
  id: number;
  brand: string;
  model: string;
  year: number;
  price: number;
  description: string | null;
  mainPhotoUrl: string | null;
  allPhotoUrls: string[] | null;
  ownerPhoneNumber: string | null;
}

export interface FavoriteItem {
  carId: number;
  brand: string;
  model: string;
  year: number;
  price: number;
  mainPhotoUrl: string | null;
}

export interface PagedResult<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
}

export interface CarsQuery {
  brandId?: number | null;
  yearFrom?: number | null;
  yearTo?: number | null;
  priceFrom?: number | null;
  priceTo?: number | null;
  page?: number;
  pageSize?: number;
  sort?: string;
}

export interface CreateCarPayload {
  brandId: number;
  model: string;
  year: number;
  description: string;
  price: number;
}
