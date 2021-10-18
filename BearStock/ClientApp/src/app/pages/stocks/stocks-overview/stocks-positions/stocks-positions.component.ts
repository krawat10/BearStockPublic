import {Component, OnInit} from '@angular/core';
import {StockPosition} from '../../../../shared/models/stock-position';
import {StockStoreService} from '../../../../shared/services/stock-store.service';
import {Stock} from '../../../../shared/models/stock';

@Component({
  selector: 'app-stocks-positions',
  templateUrl: './stocks-positions.component.html',
  styleUrls: ['./stocks-positions.component.scss']
})
export class StocksPositionsComponent implements OnInit {
  dataSource: StockPosition[] = [];
  events: Array<string> = [];

  constructor(private service: StockStoreService) {
  }

  logEvent(eventName): void {
    console.log(eventName);
    this.events.unshift(eventName);
  }

  clearEvents(): void {
    this.events = [];
  }

  ngOnInit(): void {
    this.service.stocks$.subscribe((stocks: Stock[]) => {
      this.dataSource.length = 0;
      for (const stock of stocks) {
        for (const stockPosition of stock.stockPositions) {
          stockPosition.id = stockPosition.date.toDateString() + stock.ticket;
          stockPosition.ticket = stock.ticket;
          this.dataSource.push(stockPosition);
        }
      }

    });
  }
}
