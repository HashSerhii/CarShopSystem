import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CarsService } from '../../core/services/cars.service';
import { CarListItem } from '../../core/models/api.models';
import { CarCardComponent } from '../../shared/car-card/car-card.component';

@Component({
  selector: 'app-my-cars',
  standalone: true,
  imports: [
    RouterLink,
    CarCardComponent,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './my-cars.component.html',
  styleUrl: './my-cars.component.scss',
})
export class MyCarsComponent implements OnInit {
  private readonly carsService = inject(CarsService);
  private readonly snack = inject(MatSnackBar);

  cars: CarListItem[] = [];
  loading = false;

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.carsService.getMyCars().subscribe({
      next: (res) => {
        this.cars = res.items;
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }

  delete(id: number, event: Event): void {
    event.preventDefault();
    event.stopPropagation();
    if (!confirm('Видалити це оголошення?')) return;
    this.carsService.delete(id).subscribe({
      next: () => {
        this.snack.open('Видалено', 'OK', { duration: 2500 });
        this.load();
      },
      error: () =>
        this.snack.open('Не вдалось видалити', 'OK', { duration: 3000 }),
    });
  }
}
