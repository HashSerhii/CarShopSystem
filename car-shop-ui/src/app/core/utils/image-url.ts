export function resolveImageUrl(url: string | null | undefined): string {
  if (!url) {
    return 'assets/car-placeholder.svg';
  }
  if (url.startsWith('http')) {
    return url;
  }
  return url.startsWith('/') ? url : `/${url}`;
}
