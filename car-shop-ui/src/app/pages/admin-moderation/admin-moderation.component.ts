import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CarsService } from '../../core/services/cars.service';
import { BrandsService } from '../../core/services/brands.service';
import { Brand, CarListItem } from '../../core/models/api.models';
import { CarCardComponent } from '../../shared/car-card/car-card.component';

@Component({
  selector: 'app-admin-moderation',
  standalone: true,
  imports: [
    RouterLink,
    ReactiveFormsModule,
    CarCardComponent,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
  ],
  templateUrl: './admin-moderation.component.html',
  styleUrl: './admin-moderation.component.scss',
})
export class AdminModerationComponent implements OnInit {
  private readonly carsService = inject(CarsService);
  private readonly brandsService = inject(BrandsService);
  private readonly snack = inject(MatSnackBar);
  private readonly fb = inject(FormBuilder);

  cars: CarListItem[] = [];
  brands: Brand[] = [];
  loading = false;

  readonly brandForm = this.fb.group({
    name: ['', Validators.required],
  });

  ngOnInit(): void {
    this.load();
    this.loadBrands();
  }

  loadBrands(): void {
    this.brandsService.getAll().subscribe((b) => (this.brands = b));
  }

  addBrand(): void {
    if (this.brandForm.invalid) return;
    const name = this.brandForm.value.name!.trim();
    this.brandsService.create(name).subscribe({
      next: () => {
        this.snack.open(`Марку «${name}» додано`, 'OK', { duration: 2500 });
        this.brandForm.reset();
        this.loadBrands();
      },
      error: () =>
        this.snack.open('Марка вже є або помилка', 'OK', { duration: 3000 }),
    });
  }

  load(): void {
    this.loading = true;
    this.carsService.getPending().subscribe({
      next: (res) => {
        this.cars = res.items;
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }

  approve(id: number): void {
    this.carsService.approve(id).subscribe({
      next: () => {
        this.snack.open('Оголошення схвалено', 'OK', { duration: 2500 });
        this.load();
      },
      error: () =>
        this.snack.open('Не вдалось схвалити', 'OK', { duration: 3000 }),
    });
  }

  reject(id: number): void {
    if (!confirm('Відхилити це оголошення?')) return;
    this.carsService.reject(id).subscribe({
      next: () => {
        this.snack.open('Оголошення відхилено', 'OK', { duration: 2500 });
        this.load();
      },
      error: () =>
        this.snack.open('Не вдалось відхилити', 'OK', { duration: 3000 }),
    });
  }

  remove(id: number): void {
    if (!confirm('Видалити це оголошення назавжди?')) return;
    this.carsService.delete(id).subscribe({
      next: () => {
        this.snack.open('Оголошення видалено', 'OK', { duration: 2500 });
        this.load();
      },
      error: () =>
        this.snack.open('Не вдалось видалити', 'OK', { duration: 3000 }),
    });
  }
}
