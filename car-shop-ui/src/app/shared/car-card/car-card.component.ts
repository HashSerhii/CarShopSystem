import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CurrencyPipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { CarListItem } from '../../core/models/api.models';
import { resolveImageUrl } from '../../core/utils/image-url';

@Component({
  selector: 'app-car-card',
  standalone: true,
  imports: [RouterLink, CurrencyPipe, MatCardModule, MatButtonModule],
  templateUrl: './car-card.component.html',
  styleUrl: './car-card.component.scss',
})
export class CarCardComponent {
  @Input({ required: true }) car!: CarListItem;

  imageUrl(): string {
    return resolveImageUrl(this.car.primaryPhotoUrl);
  }
}
