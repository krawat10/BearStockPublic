import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {DxPieChartComponent} from 'devextreme-angular/ui/pie-chart';
import {StockStoreService} from '../../../../shared/services/stock-store.service';
import {ScreenService} from '../../../../shared/services';
import {Subscription} from 'rxjs';
export class StockPositionData {
  ticket: string;
  total: number;
}

@Component({
  selector: 'app-stocks-portfolio',
  templateUrl: './stocks-portfolio.component.html',
  styleUrls: ['./stocks-portfolio.component.scss']
})
export class StocksPortfolioComponent implements OnInit, OnDestroy {
  @ViewChild(DxPieChartComponent, {static: false}) chartComponent: DxPieChartComponent;
  stockPositions: StockPositionData[] = [];
  changeSub: Subscription;

  constructor(private stockStore: StockStoreService, private screen: ScreenService) {
  }

  public customizeLabel(point): string {
    return point.argumentText + ': ' + point.valueText + ' USD';
  }

  ngOnInit(): void {
    this.stockStore.stocks$.subscribe(stocks => {
      this.stockPositions.length = 0;

      for (const stock of stocks) {
        const sum = stock.stockPositions.reduce((sumPrice, {totalPrice}) => sumPrice + totalPrice, 0);

        if (sum > 0) {
          this.stockPositions.push({ticket: stock.ticket, total: sum});
        }
      }
    });

    this.changeSub = this.screen.changed.subscribe(x => this.chartComponent.instance.refresh());
  }

  ngOnDestroy(): void {
    this.changeSub.unsubscribe();
  }
}
