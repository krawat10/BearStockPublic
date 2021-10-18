import {Chart} from './chart';
import {StockPosition} from './stock-position';

export class Stock {
  uuid: string;
  order: number;
  ticket: string;
  chart: Chart;
  stockPositions: StockPosition[];
}
