import { Injectable } from '@angular/core';

@Injectable()
export class AppInfoService {
  constructor() {}

  public get title(): string {
    return 'BearStockApp';
  }

  public get currentYear(): number {
    return new Date().getFullYear();
  }
}
