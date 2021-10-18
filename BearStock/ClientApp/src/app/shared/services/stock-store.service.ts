import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';
import * as uuid from 'uuid';
import {Stock} from '../models/stock';
import * as _ from 'lodash';

@Injectable({providedIn: 'root'})
export class StockStoreService {
  private readonly _stocks = new BehaviorSubject<Stock[]>([]);
  readonly stocks$: Observable<Stock[]> = this._stocks.asObservable();

  constructor() {
  }

  get count(): number {
    return this._stocks.getValue().length;
  }

  private get stocks(): Stock[] {
    return this._stocks.getValue();
  }

  private set stocks(val: Stock[]) {
    this._stocks.next(_.sortBy(val, ['order']));
  }

  addStock(stock: Stock): void {
    this.stocks = [
      ...this.stocks,
      {uuid: uuid.v4(), ...stock}
    ];
  }

  async updateStock(stock: Stock): Promise<void> {
    try {
      // await this.stockDashboardService.updateDashboard(dashboard);
    } catch (e) {
      console.error(e);
    }

    this.stocks = this.stocks.map(old => {
      return old.uuid === stock.uuid ? {...stock} : old;
    });
  }

  removeStock(ticket: string): void {
    this.stocks = this.stocks.filter(stock => stock.ticket !== ticket);
  }

  clearStocks(): void {
    this.stocks = [];
  }
}
