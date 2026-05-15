import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { statusClass, statusLabel } from '../../core/utils/listing-status';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CarsService } from '../../core/services/cars.service';
import { FavoritesService } from '../../core/services/favorites.service';
import { AuthService } from '../../core/services/auth.service';
import { CarDetail } from '../../core/models/api.models';
import { resolveImageUrl } from '../../core/utils/image-url';

@Component({
  selector: 'app-car-detail',
  standalone: true,
  imports: [
    RouterLink,
    CurrencyPipe,
    DecimalPipe,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
  ],
  templateUrl: './car-detail.component.html',
  styleUrl: './car-detail.component.scss',
})
export class CarDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly carsService = inject(CarsService);
  private readonly favoritesService = inject(FavoritesService);
  readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly snack = inject(MatSnackBar);

  car: CarDetail | null = null;
  loading = true;
  favoriteLoading = false;

  readonly statusLabel = statusLabel;
  readonly statusClass = statusClass;

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.carsService.getById(id).subscribe({
      next: (c) => {
        this.car = c;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        void this.router.navigate(['/']);
      },
    });
  }

  mainImage(): string {
    return resolveImageUrl(this.car?.mainPhotoUrl);
  }

  gallery(): string[] {
    const urls = this.car?.allPhotoUrls?.filter(Boolean) ?? [];
    if (urls.length) {
      return urls.map((u) => resolveImageUrl(u));
    }
    return [this.mainImage()];
  }

  callSeller(): void {
    const phone = this.car?.ownerPhoneNumber;
    if (phone) {
      window.location.href = `tel:${phone}`;
    }
  }

  toggleFavorite(): void {
    if (!this.auth.isLoggedIn() || !this.car) {
      void this.router.navigate(['/login']);
      return;
    }
    this.favoriteLoading = true;
    this.favoritesService.add(this.car.id).subscribe({
      next: () => {
        this.favoriteLoading = false;
        this.snack.open('Додано в обране', 'OK', { duration: 2500 });
      },
      error: () => {
        this.favoriteLoading = false;
        this.snack.open('Не вдалось додати', 'OK', { duration: 3000 });
      },
    });
  }

  deleteCar(): void {
    if (!this.car || !confirm('Видалити оголошення?')) return;
    this.carsService.delete(this.car.id).subscribe({
      next: () => {
        this.snack.open('Видалено', 'OK', { duration: 2500 });
        void this.router.navigate(['/my-cars']);
      },
      error: () =>
        this.snack.open('Немає прав або помилка', 'OK', { duration: 4000 }),
    });
  }
}
