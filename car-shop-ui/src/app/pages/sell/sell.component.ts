import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { BrandsService } from '../../core/services/brands.service';
import { CarsService } from '../../core/services/cars.service';
import { Brand } from '../../core/models/api.models';

@Component({
  selector: 'app-sell',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
    MatIconModule,
  ],
  templateUrl: './sell.component.html',
  styleUrl: './sell.component.scss',
})
export class SellComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly brandsService = inject(BrandsService);
  private readonly carsService = inject(CarsService);
  private readonly router = inject(Router);
  private readonly snack = inject(MatSnackBar);

  brands: Brand[] = [];
  photos: File[] = [];
  loading = false;

  readonly form = this.fb.group({
    brandId: [null as number | null, Validators.required],
    model: ['', Validators.required],
    year: [new Date().getFullYear(), [Validators.required, Validators.min(1990)]],
    description: ['', Validators.required],
    price: [null as number | null, [Validators.required, Validators.min(1)]],
  });

  ngOnInit(): void {
    this.brandsService.getAll().subscribe((b) => (this.brands = b));
  }

  onFilesSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.photos = input.files ? Array.from(input.files) : [];
  }

  submit(): void {
    if (this.form.invalid) return;
    this.loading = true;
    const v = this.form.getRawValue();

    this.carsService
      .create({
        brandId: v.brandId!,
        model: v.model!,
        year: v.year!,
        description: v.description!,
        price: v.price!,
      })
      .pipe(
        switchMap((carId) =>
          this.photos.length
            ? this.carsService.uploadPhotos(carId, this.photos)
            : of(null)
        )
      )
      .subscribe({
        next: () => {
          this.snack.open('Оголошення опубліковано!', 'OK', { duration: 3000 });
          void this.router.navigate(['/my-cars']);
        },
        error: () => {
          this.loading = false;
          this.snack.open('Помилка публікації', 'OK', { duration: 4000 });
        },
      });
  }
}
