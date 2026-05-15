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
import { HttpErrorResponse } from '@angular/common/http';
import { BrandsService } from '../../core/services/brands.service';
import { CarsService } from '../../core/services/cars.service';
import { AuthService } from '../../core/services/auth.service';
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
  readonly auth = inject(AuthService);

  brands: Brand[] = [];
  photos: File[] = [];
  loading = false;

  readonly form = this.fb.group({
    brandId: [null as number | null, Validators.required],
    model: ['', Validators.required],
    year: [new Date().getFullYear(), [Validators.required, Validators.min(1990)]],
    mileage: [null as number | null, [Validators.required, Validators.min(0)]],
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
        mileage: v.mileage!,
        description: v.description!,
        price: v.price!,
      })
      .subscribe({
        next: (carId) => this.uploadPhotosAndFinish(carId),
        error: (err) => this.showError(err),
      });
  }

  private uploadPhotosAndFinish(carId: number): void {
    if (!this.photos.length) {
      this.finishSuccess();
      return;
    }

    this.carsService.uploadPhotos(carId, this.photos).subscribe({
      next: () => this.finishSuccess(),
      error: () => {
        this.loading = false;
        this.snack.open(
          'Заявку створено, але фото не завантажилось. Спробуйте ще раз у «Мої авто».',
          'OK',
          { duration: 5000 }
        );
        void this.router.navigate(['/my-cars']);
      },
    });
  }

  private finishSuccess(): void {
    this.loading = false;
    const msg = this.auth.isAdmin()
      ? 'Оголошення опубліковано!'
      : 'Заявку надіслано на модерацію. Після схвалення адміном вона зʼявиться в каталозі.';
    this.snack.open(msg, 'OK', { duration: 4500 });
    void this.router.navigate(['/my-cars']);
  }

  private showError(err: unknown): void {
    this.loading = false;
    let message = 'Помилка збереження';

    if (err instanceof HttpErrorResponse) {
      if (err.status === 401) {
        message = 'Увійдіть знову в акаунт';
      } else if (err.status === 0) {
        message = 'API недоступний. Запустіть CarShop.API у Rider';
      } else if (typeof err.error === 'string') {
        message = err.error;
      } else if (err.error?.title) {
        message = err.error.title;
      } else if (err.error?.detail) {
        message = err.error.detail;
      }
    }

    this.snack.open(message, 'OK', { duration: 5000 });
  }
}
