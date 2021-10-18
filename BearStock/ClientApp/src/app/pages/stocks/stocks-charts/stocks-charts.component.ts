import {Component, OnInit} from '@angular/core';
import {StockStoreService} from '../../../shared/services/stock-store.service';

@Component({
  selector: 'app-stocks-charts',
  templateUrl: './stocks-charts.component.html',
  styleUrls: ['./stocks-charts.component.scss']
})
export class StocksChartsComponent implements OnInit {

  constructor(public stockStoreService: StockStoreService) {
  }

  ngOnInit(): void {
  }
}
