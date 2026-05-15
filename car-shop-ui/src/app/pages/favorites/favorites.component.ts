import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FavoritesService } from '../../core/services/favorites.service';
import { FavoriteItem } from '../../core/models/api.models';
import { CarCardComponent } from '../../shared/car-card/car-card.component';
import { CarListItem } from '../../core/models/api.models';

@Component({
  selector: 'app-favorites',
  standalone: true,
  imports: [
    RouterLink,
    CarCardComponent,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './favorites.component.html',
  styleUrl: './favorites.component.scss',
})
export class FavoritesComponent implements OnInit {
  private readonly favoritesService = inject(FavoritesService);
  private readonly snack = inject(MatSnackBar);

  items: FavoriteItem[] = [];
  loading = false;

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.favoritesService.getAll().subscribe({
      next: (res) => {
        this.items = res.items;
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }

  toCard(f: FavoriteItem): CarListItem {
    return {
      id: f.carId,
      brand: f.brand,
      model: f.model,
      year: f.year,
      price: f.price,
      mileage: 0,
      status: 'Approved',
      primaryPhotoUrl: f.mainPhotoUrl,
    };
  }

  remove(carId: number): void {
    this.favoritesService.remove(carId).subscribe({
      next: () => {
        this.snack.open('Прибрано з обраного', 'OK', { duration: 2500 });
        this.load();
      },
      error: () =>
        this.snack.open('Помилка', 'OK', { duration: 3000 }),
    });
  }
}
