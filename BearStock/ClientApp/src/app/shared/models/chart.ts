import {StockPrice} from './stock-price';

export class Chart {
  uuid: string;
  bookValue: number;
  currency: string;
  name: string;
  pe: number;
  sector: string;
  values: StockPrice[];
}
