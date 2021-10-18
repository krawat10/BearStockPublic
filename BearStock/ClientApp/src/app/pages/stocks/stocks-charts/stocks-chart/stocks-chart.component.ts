import {Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild} from '@angular/core';
import {ChartService} from '../../../../shared/services/chart.service';
import {DxChartComponent} from 'devextreme-angular/ui/chart';
import {StockPrice} from '../../../../shared/models/stock-price';
import {Stock} from '../../../../shared/models/stock';
import {StockPosition} from 'src/app/shared/models/stock-position';
import {ScreenService} from '../../../../shared/services';
import {Subscription} from 'rxjs';
import {DxRangeSelectorComponent} from 'devextreme-angular/ui/range-selector';

export enum Interval {
  fifteenMinutes = '15m',
  oneHour = '1h',
  oneDay = '1d',
}

export class IntervalButtonSettings {
  hint: string;
  text: string;
  interval: Interval;
}

export class ChartTypeButtonSettings {
  hint: string;
  text: string;
}


export interface ScaleBreak {
  endValue: Date;
  startValue: Date;
}

@Component({
  selector: 'app-stocks-charts-item',
  templateUrl: './stocks-chart.component.html',
  styleUrls: ['./stocks-chart.component.scss']
})
export class StocksChartComponent implements OnInit, OnDestroy {
  @Input() stock: Stock;
  @ViewChild(DxChartComponent) chartComponent: DxChartComponent;
  @ViewChild(DxRangeSelectorComponent) rangeSelector: DxRangeSelectorComponent;
  @Output() removeEvent = new EventEmitter<Stock>();
  stockPrices: StockPrice[];
  visualRange: any = {};
  loading = true;

  intervals: IntervalButtonSettings[] = [
    {hint: '1 day', text: '1d', interval: Interval.oneDay},
    {hint: '1 hour', text: '1h', interval: Interval.oneHour},
    {hint: '15 minutes', text: '15min', interval: Interval.fifteenMinutes}
  ];
  chartTypes: ChartTypeButtonSettings[] = [
    {text: 'ohlc', hint: 'open/high/low/close'},
    {text: 'line', hint: 'linear'},
    {text: 'log', hint: 'logarithmic'},
  ];
  selectedInterval = this.intervals[0];
  selectedChartType = this.chartTypes[0];
  breaks: ScaleBreak[] = [];
  stockActionFormVisible: boolean;
  constantLines: Array<{
    color?: string;
    dashStyle?: string;
    label?: {
      text?: string;
    };
    value?: Date | number | string;
    width?: number;
  }> = [];
  changeSub: Subscription;

  constructor(private chartService: ChartService, private screen: ScreenService) {

  }

  get title(): string {
    if (this.stock?.chart?.sector) {
      if (this.stock.chart.sector !== 'Unknown') {
        return this.stock.ticket + ' - ' + this.stock.chart.sector;
      }
    }


    return this.stock?.ticket ?? '';
  }

  get chartId(): string {
    return 'chart-' + this.stock.ticket;
  }

  async ngOnInit(): Promise<void> {
    await this.setupChart(false);
  }


  ngOnDestroy(): void {
    this.changeSub?.unsubscribe();
  }

  remove(): void {
    this.removeEvent.emit(this.stock);

  }

  async changeInterval(e): Promise<void> {
    this.selectedInterval = e.itemData;
    await this.setupChart(true);
  }

  getAboveOneHourBreaks(values: StockPrice[]): ScaleBreak[] {
    const breaks: ScaleBreak[] = [];

    for (let i = 1; i < values.length; i++) {
      const earlierDate = new Date(values[i - 1].d);
      const laterDate = new Date(values[i].d);

      const minDiff = (laterDate.getTime() - earlierDate.getTime()) / (60 * 1000);

      if (minDiff > 60) {
        breaks.push({
          startValue: new Date(earlierDate.getTime() + (30 * 60 * 1000)),
          endValue: new Date(laterDate.getTime() - (30 * 60 * 1000))
        });
      }
    }

    return breaks;
  }

  onResize(): void {
    this.chartComponent.instance.refresh();
    this.rangeSelector.instance.render(true);
  }

  private async setupChart(forceReload: boolean): Promise<void> {
    this.loading = true;

    if (!this.stock.chart || forceReload) {
      this.stock.chart = await this.chartService.fetchChart(this.stock.ticket, this.selectedInterval.text);
    }

    for (const position of this.stock.stockPositions) {
      this.addConstantLine(position);
    }

    switch (this.selectedInterval.interval) {
      case Interval.fifteenMinutes:
        break;
      case Interval.oneHour:
        this.breaks = this.getAboveOneHourBreaks(this.stock.chart.values);
        break;
      case Interval.oneDay:
        this.breaks = [];
        break;
    }

    this.stockPrices = this.stock.chart.values;
    this.loading = false;
  }

  private addConstantLine(position: StockPosition): void {
    this.constantLines.push({
      value: position.pricePerShare,
      width: 1,
      color: '#8c8cff',
      label: {text: `${position.pricePerShare.toString()} (${position.date.toLocaleDateString()})`},
      dashStyle: 'dash'
    });
  }
}
