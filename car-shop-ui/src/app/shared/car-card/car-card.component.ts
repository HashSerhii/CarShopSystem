import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { CarListItem } from '../../core/models/api.models';
import { resolveImageUrl } from '../../core/utils/image-url';
import { statusClass, statusLabel } from '../../core/utils/listing-status';

@Component({
  selector: 'app-car-card',
  standalone: true,
  imports: [RouterLink, CurrencyPipe, DecimalPipe, MatCardModule, MatButtonModule],
  templateUrl: './car-card.component.html',
  styleUrl: './car-card.component.scss',
})
export class CarCardComponent {
  @Input({ required: true }) car!: CarListItem;
  @Input() showStatusBadge = false;

  readonly statusLabel = statusLabel;
  readonly statusClass = statusClass;

  imageUrl(): string {
    return resolveImageUrl(this.car.primaryPhotoUrl);
  }

  showStatus(): boolean {
    return this.showStatusBadge || this.car.status !== 'Approved';
  }
}
