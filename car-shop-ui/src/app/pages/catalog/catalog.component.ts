import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { debounceTime } from 'rxjs/operators';
import { CarsService } from '../../core/services/cars.service';
import { BrandsService } from '../../core/services/brands.service';
import { Brand, CarListItem } from '../../core/models/api.models';
import { CarCardComponent } from '../../shared/car-card/car-card.component';

@Component({
  selector: 'app-catalog',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule,
    CarCardComponent,
  ],
  templateUrl: './catalog.component.html',
  styleUrl: './catalog.component.scss',
})
export class CatalogComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly carsService = inject(CarsService);
  private readonly brandsService = inject(BrandsService);

  brands: Brand[] = [];
  cars: CarListItem[] = [];
  total = 0;
  loading = false;

  readonly filters = this.fb.group({
    brandId: [null as number | null],
    yearFrom: [null as number | null],
    yearTo: [null as number | null],
    priceFrom: [null as number | null],
    priceTo: [null as number | null],
    sort: ['price_asc'],
    page: [1],
  });

  ngOnInit(): void {
    this.brandsService.getAll().subscribe((b) => (this.brands = b));
    this.load();
    this.filters.valueChanges.pipe(debounceTime(300)).subscribe(() => {
      this.filters.patchValue({ page: 1 }, { emitEvent: false });
      this.load();
    });
  }

  load(): void {
    this.loading = true;
    const v = this.filters.getRawValue();
    this.carsService
      .getCars({
        brandId: v.brandId,
        yearFrom: v.yearFrom,
        yearTo: v.yearTo,
        priceFrom: v.priceFrom,
        priceTo: v.priceTo,
        sort: v.sort ?? 'price_asc',
        page: v.page ?? 1,
        pageSize: 12,
      })
      .subscribe({
        next: (res) => {
          this.cars = res.items;
          this.total = res.total;
          this.loading = false;
        },
        error: () => (this.loading = false),
      });
  }

  reset(): void {
    this.filters.reset({
      brandId: null,
      yearFrom: null,
      yearTo: null,
      priceFrom: null,
      priceTo: null,
      sort: 'price_asc',
      page: 1,
    });
    this.load();
  }

  nextPage(): void {
    const page = (this.filters.value.page ?? 1) + 1;
    if ((page - 1) * 12 >= this.total) return;
    this.filters.patchValue({ page });
    this.load();
  }

  prevPage(): void {
    const page = (this.filters.value.page ?? 1) - 1;
    if (page < 1) return;
    this.filters.patchValue({ page });
    this.load();
  }
}
