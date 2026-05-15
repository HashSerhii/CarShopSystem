import { ListingStatus } from '../models/api.models';

export function statusLabel(status: ListingStatus): string {
  switch (status) {
    case 'Pending':
      return 'На модерації';
    case 'Approved':
      return 'Опубліковано';
    case 'Rejected':
      return 'Відхилено';
    default:
      return status;
  }
}

export function statusClass(status: ListingStatus): string {
  switch (status) {
    case 'Pending':
      return 'status-pending';
    case 'Approved':
      return 'status-approved';
    case 'Rejected':
      return 'status-rejected';
    default:
      return '';
  }
}
