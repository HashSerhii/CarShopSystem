export interface AuthResponse {
  token: string;
  expiration: string;
}

export interface Brand {
  id: number;
  name: string;
}

export type ListingStatus = 'Pending' | 'Approved' | 'Rejected';

export interface CarListItem {
  id: number;
  brand: string;
  model: string;
  year: number;
  price: number;
  mileage: number;
  status: ListingStatus;
  primaryPhotoUrl: string | null;
}

export interface CarDetail {
  id: number;
  brand: string;
  model: string;
  year: number;
  price: number;
  mileage: number;
  status: ListingStatus;
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
  mileageFrom?: number | null;
  mileageTo?: number | null;
  model?: string | null;
  page?: number;
  pageSize?: number;
  sort?: string;
}

export interface CreateCarPayload {
  brandId: number;
  model: string;
  year: number;
  mileage: number;
  description: string;
  price: number;
}
