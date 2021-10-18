import {Component, OnInit, ViewChild} from '@angular/core';
import {maxBy} from 'lodash';
import {StockPrice} from '../../../../shared/models/stock-price';
import {ChartService} from '../../../../shared/services/chart.service';
import {DxChartComponent} from 'devextreme-angular/ui/chart';
import {StockStoreService} from '../../../../shared/services/stock-store.service';
import {Chart} from '../../../../shared/models/chart';
import {Stock} from '../../../../shared/models/stock';
import {map} from 'rxjs/operators';

export interface StockPerformanceData {
  ticket: string;
  loss?: number;
  gain?: number;
}

@Component({
  selector: 'app-stocks-gain-loss',
  templateUrl: './stocks-gain-loss.component.html',
  styleUrls: ['./stocks-gain-loss.component.scss']
})
export class StocksGainLossComponent implements OnInit {
  @ViewChild(DxChartComponent, {static: false}) chartComponent: DxChartComponent;
  stockPerformances: StockPerformanceData[] = [];

  constructor(private chartService: ChartService, private stockStore: StockStoreService) {
  }

  customizeTooltip = (args: any) => {
    return {
      text: args.valueText + '%'
    };
  };

  customizeLabel = (args: any) => {
    return args.value + '%';
  };

  ngOnInit(): void {
    this.stockStore.stocks$
      .pipe(map((stocks: Stock[]) => {
        return stocks
          .filter(stock => stock.stockPositions.length > 0);
      }))
      .subscribe(async (stocks: Stock[]) => {
        this.stockPerformances.length = 0;

        for (const stock of stocks) {
          let chart: Chart = stock.chart;

          if (!chart?.values) {
            chart = await this.chartService.fetchChart(stock.ticket, '1d', '3d');
          }
          this.addStockPerformanceIndicator(stock, chart);
        }

        this.addOverallStockPerformanceIndicator(stocks);
      });
  }

  private addStockPerformanceIndicator(stock: Stock, chart: Chart): void {
    const item = {ticket: stock.ticket, gain: null, loss: null};
    const sharesTotalPrice = stock.stockPositions.reduce((sumPrice, {totalPrice}) => sumPrice + totalPrice, 0);
    const sharesCount = stock.stockPositions.reduce((count, {sharesAmount}) => count + sharesAmount, 0);

    const todayStockPrice: StockPrice = maxBy(chart.values, x => x.d);
    const todaySharesTotalPrice = todayStockPrice.c * sharesCount;
    const percentageChange = (todaySharesTotalPrice / sharesTotalPrice - 1) * 100;

    if (percentageChange > 0) {
      item.gain = +percentageChange.toFixed(2);
    } else {
      item.loss = +percentageChange.toFixed(2);
    }

    this.stockPerformances.push(item);
  }

  private addOverallStockPerformanceIndicator(stocks: Stock[]): void {
    const item = {ticket: 'Overall', gain: null, loss: null};
    let sharesTotalPrice = 0.0;
    let todaySharesTotalPrice = 0.0;

    for (const stock of stocks) {
      sharesTotalPrice += stock.stockPositions.reduce((sumPrice, {totalPrice}) => sumPrice + totalPrice, 0);
      const sharesCount = stock.stockPositions.reduce((count, {sharesAmount}) => count + sharesAmount, 0);

      const todayStockPrice: StockPrice = maxBy(stock.chart.values, x => x.d);
      todaySharesTotalPrice += todayStockPrice.c * sharesCount;
    }

    const percentageChange = (todaySharesTotalPrice / sharesTotalPrice - 1) * 100;

    if (percentageChange > 0) {
      item.gain = +percentageChange.toFixed(2);
    } else {
      item.loss = +percentageChange.toFixed(2);
    }

    this.stockPerformances.push(item);
  }

}
